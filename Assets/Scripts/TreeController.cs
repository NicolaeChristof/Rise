using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using RiseExtensions;
using System;
using System.Collections;

public class TreeController : RiseBehavior {

    // Public references
    public GameObject reticle;
    public GameObject squirrel;
    public GameObject tree;

    // Public Fields
    public float speedVertical = 2.15F;
    public float speedLateral = 6.30F;
    public float minDistance = 0.5F;

    [Range(0.0f, 10.0f)]
    public float groundHeight;

    [Range(1.0f, 10.0f)]
    public float playerDistance;

    // Local References
    private GameObject _reticleInstance;
    private GameObject canvas;
    private UIController ui;
    private bool triggered = false;
    private IEnumerator coroutine;
    // Local Fields
    private int _selectedBranch;
    private List<BranchProvider> branches;

    // Local Constants
    private const float EPSILON = 0.01F;
    private const float DISTANCE = 2.0F;

    public delegate void UIUpdateEvent(BranchProvider provider);
    public event UIUpdateEvent uiUpdateEvent;

    #region DEPRECATED
    /************* vvvv TODO: REMOVE PENDING UI OVERHAUL vvvv *************/
    private const string obsoleteMessage = "Prefer UIUpdateEvent";
    [Obsolete(obsoleteMessage)]
    public delegate void SapChangeEvent(float sapValue, int branchType);
    [Obsolete(obsoleteMessage)]
    public event SapChangeEvent sapUpdated;

    [Obsolete(obsoleteMessage)]
    public delegate void BranchChangeEvent(int branchType);
    [Obsolete(obsoleteMessage)]
    public event BranchChangeEvent branchUpdated;
    /**********************************************************************/
    #endregion DEPRECATED


    void Start() {
        _reticleInstance = Instantiate(reticle, transform.position, Quaternion.identity);
        canvas = GameObject.Find("Canvas");
        ui = canvas.GetComponent<UIController>();
        UpdateReticle();
        UpdateComponents();
        Select(0);
    }

    public override void UpdateTick() {

        float moveVertical, moveLateral;
        bool grow;

        if (!GameModel.endGame) {

            moveVertical = InputHelper.GetAxis(TreeInput.MOVE_VERTICAL);
            moveLateral = InputHelper.GetAxis(TreeInput.MOVE_HORIZONTAL);
            grow = InputHelper.GetAnyDown(TreeInput.BRANCH_PLACE);

        } else {

            moveVertical = 0.0f;
            moveLateral = 0.0f;
            grow = false;

        }

        bool moved = false;

        // If vertical axis is actuated beyond epsilon value, translate reticle vertically
        if (moveVertical.CheckEpsilon(EPSILON)) {
            float idealMove = (moveVertical * Time.deltaTime * speedVertical) * -1;
            // Ensure reticle is out of the ground
            if (idealMove + transform.position.y > groundHeight) {
                // Ensure reticle is close to squirrel
                // Case 1: verticallyClose - If ideal move doesn't put the reticle out of range
                bool verticallyClose = System.Math.Abs((idealMove + transform.position.y) - squirrel.transform.position.y) < playerDistance;
                // Case 2: If squirrel is out of range below reticle, but reticle is moving downwards towards squirrel, permit motion
                bool squirrelBelow = (transform.position.y > squirrel.transform.position.y) && (idealMove + transform.position.y < transform.position.y);
                // Case 3: if squirrel is out of range above reticle, but reticle is moving upwards towards squirrel, permit motion
                bool squirrelAbove = (transform.position.y < squirrel.transform.position.y) && (idealMove + transform.position.y > transform.position.y);
                if (verticallyClose || squirrelBelow || squirrelAbove) {
                    transform.Translate(Vector3.up * idealMove, Space.World);
                }

            }
        }

        // If horizontal axis is actuated beyond epsilon value, translate reticle horizontally
        if (moveLateral.CheckEpsilon(EPSILON)) {
            transform.Translate(Vector3.right * (moveLateral * Time.deltaTime * speedLateral), Space.Self);
            moved = true;
        }

        // If the controller was actuated, move the reticle object
        if (moved) {
            UpdateReticle();
        }

        // Handle Branch Selection
        if (InputHelper.GetAnyDown(TreeInput.SELECT_RIGHT)) {
            Scroll(Mathf.RoundToInt(InputHelper.GetAxisRaw(TreeInput.SELECT_RIGHT)));
        }

		// Handle Break
		if (InputHelper.GetAnyDown(TreeInput.BRANCH_REMOVE)) {
            AttemptBreakBranch();
		}

        // Handle Growth 
        else if (grow) {
            AttemptGrowBranch();
        }

        // Handle Teleport
        if (InputHelper.GetAnyDown(TreeInput.TELEPORT)) {

            // Debug.Log("TELEPORT");

            transform.position = squirrel.transform.position;

            UpdateReticle();

        }

    }

    public override void UpdateAlways() {

    }

    /// <summary>
    /// Automatically updates all sap of a particular type throughout the TreeController's managed Branchproviders.
    /// </summary>
    /// <param name="type">The sap type to update.</param>
    /// <param name="quantity">The quantity to modify by.</param>
    public void UpdateSap(SapType type, float quantity) {
        foreach (BranchProvider provider in branches) {
            provider.UpdateSap(type, quantity);
            sapUpdated?.Invoke(quantity, (int)type);
        }
        if(GameModel.squirrelHealth < 10)
        {
            GameModel.squirrelHealth++;
            GameObject Health = GameObject.Find("Health Bar");
            Health.GetComponent<HealthUI>().UpdateHealth();
        }
        UpdateUI();
    }

    /// <summary>
    /// Attempts to grow a branch.
    /// </summary>
    public void AttemptGrowBranch() {
        GameObject newBranch = GetSelectedBranch().PlaceBranch(_reticleInstance.transform.position, _reticleInstance.transform.rotation);
        AudioClip feedbackSound = (newBranch is null) ? GetSelectedBranch().cantGrowSound : GetSelectedBranch().growSound;
        _reticleInstance.GetComponent<AudioSource>()?.PlayOneShot(feedbackSound, UnityEngine.Random.Range(GameModel.volLowRange, GameModel.volHighRange));
        UpdateUI();
	}

    /// <summary>
    /// Attempts to break the nearest branch within range.
    /// </summary>
    public void AttemptBreakBranch() {
        float distance = float.MaxValue;
        GameObject closestBranch = null;
        Collider[] colliders = Physics.OverlapSphere(_reticleInstance.transform.position, minDistance);
        foreach (Collider iteratedCollider in colliders) {
            if (iteratedCollider.CompareTag(Tags.BRANCH)) {
                float currentDistance = Vector3.Distance(iteratedCollider.transform.position, _reticleInstance.transform.position);
                if (currentDistance < distance) {
                    distance = currentDistance;
                    closestBranch = iteratedCollider.gameObject;
                }
            }
        }
        closestBranch?.GetComponent<BranchBehavior>().OnBreak();
        UpdateUI();
    }

    /// <summary>
    /// Returns the current reticle transform
    /// </summary>
    public Transform getReticleTransform () {
        return _reticleInstance.transform;
    }

    /// <summary>
    /// Returns the currently-selected BranchProvider.
    /// </summary>
    /// <returns>The selected branch.</returns>
    public BranchProvider GetSelectedBranch() {
        return branches[_selectedBranch];
    }

    /// <summary>
    /// Updates the internally-maintained list of BranchProvider components.
    /// 
    /// If BranchProvider instances are added to or removed from the parent Object at runtime, this method MUST be called, otherwise silly things will happen.
    /// </summary>
    public void UpdateComponents() {
        branches = new List<BranchProvider>(GetComponents<BranchProvider>());
        branches.Sort();
    }

    // Internal Methods

    /// <summary>
    /// Scrolls by the passed value to the next non-null and enabled BranchProvider.
    /// 
    /// The method will make at most branches.Count attempts to find the next non-null, enabled BranchProvider in the "direction" of scrollValue.
    /// </summary>
    /// <param name="scrollValue">Scroll value.</param>
    private void Scroll(int scrollValue) {
        for (int attempts = 0; attempts < branches.Count; attempts += 1) {
            Select(Mathf.Abs((scrollValue + attempts + _selectedBranch) % branches.Count));
            BranchProvider selectedBranch = GetSelectedBranch();
            if (!(selectedBranch is null) && selectedBranch.enabled) {
                // If the BranchProvider is not null and is enabled, that's that
                return;
            }
        }
        Debug.Log("No enabled BranchProviders on TreeController Instance!");
    }

    /// <summary>
    /// Handles changes when a branch is selected.
    /// </summary>
    /// <param name="passedIndex">Passed index.</param>
    private void Select(int passedIndex) {
        if (passedIndex != _selectedBranch) {
            _selectedBranch = passedIndex.Clamp(0, branches.Count - 1);
            branchUpdated?.Invoke(passedIndex);
        }
        UpdateUI();
	}

    /// <summary>
    /// Updates the position and rotation of the reticle object.
    /// </summary>
    private void UpdateReticle() {
        // Horizontal-only raycast
        Vector3 position = tree.transform.position;
        position.y = transform.position.y;
        tree.GetComponent<Collider>().Raycast(new Ray(transform.position, position - transform.position), out RaycastHit raycast, 500F);

        // Resolve position and facing
        transform.position = ((transform.position - raycast.point).normalized * DISTANCE) + raycast.point;
        transform.LookAt(tree.transform.position);

        // Resolve Reticle position and facing
        _reticleInstance.transform.position = raycast.point;
        _reticleInstance.transform.LookAt(raycast.point + (raycast.normal * 2.0F));
    }

    private void UpdateUI() {
        uiUpdateEvent?.Invoke(GetSelectedBranch());
    }

    private IEnumerator Delay(float wait)
    {
        while (true)
        {
            yield return new WaitForSeconds(wait);
            triggered = false;
        }
    }
}
