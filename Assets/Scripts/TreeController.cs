using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using RiseExtensions;

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

    // Local Fields
    private int _selectedBranch;

    // Local Constants
    private const float EPSILON = 0.01F;
    private const float DISTANCE = 2.0F;

    public delegate void SapChangeEvent(float sapValue, int branchType);
    public event SapChangeEvent sapUpdated;

    public delegate void BranchChangeEvent(int branchType);
    public event BranchChangeEvent branchUpdated;

    void Start() {
        _reticleInstance = Instantiate(reticle, transform.position, Quaternion.identity);
        Select(0);
        UpdateReticle();
    }

    public override void UpdateTick() {

        float moveVertical, moveLateral;
        bool grow;

        moveVertical = InputHelper.GetAxis(TreeInput.MOVE_VERTICAL);
        moveLateral = InputHelper.GetAxis(TreeInput.MOVE_HORIZONTAL);
        grow = InputHelper.GetAnyDown(TreeInput.BRANCH_PLACE);

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
                    moved = true;
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

    }

    public override void UpdateAlways() {

    }

    /// <summary>
    /// Automatically updates all sap of a particular type throughout the TreeController's managed Branchproviders.
    /// </summary>
    /// <param name="type">The sap type to update.</param>
    /// <param name="quantity">The quantity to modify by.</param>
    public void UpdateSap(SapType type, float quantity) {
        foreach (BranchProvider provider in GetComponents<BranchProvider>()) {
            provider.UpdateSap(type, quantity);
        }
    }

    /// <summary>
    /// Attempts to grow a branch.
    /// </summary>
    public void AttemptGrowBranch() {
        GameObject newBranch = GetSelectedBranch().PlaceBranch(_reticleInstance.transform.position, _reticleInstance.transform.rotation);
        AudioClip feedbackSound = (newBranch is null) ? GetSelectedBranch().cantGrowSound : GetSelectedBranch().growSound;
        _reticleInstance.GetComponent<AudioSource>()?.PlayOneShot(feedbackSound, Random.Range(GameModel.volLowRange, GameModel.volHighRange));
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
    }

    /// <summary>
    /// Returns the current reticle transform
    /// </summary>
    public Transform getReticleTransform () {
        return _reticleInstance.transform;
    }

    public BranchProvider GetSelectedBranch() {
        return GetComponents<BranchProvider>()[_selectedBranch];
    }

    // Internal Methods

    private void Scroll(int scrollValue) {
        BranchProvider[] branches = GetComponents<BranchProvider>();
        Select(Mathf.Abs((branches.Length + scrollValue + _selectedBranch) % branches.Length));
    }

    /// <summary>
    /// Handles changes when a branch is selected.
    /// </summary>
    /// <param name="passedIndex">Passed index.</param>
    private void Select(int passedIndex) {
        if (passedIndex != _selectedBranch) {
            _selectedBranch = passedIndex.Clamp(0, GetComponents<BranchProvider>().Length - 1);
            branchUpdated?.Invoke(passedIndex);
        }
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
}
