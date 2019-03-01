﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreeController : RiseBehavior {

    // public references
    public GameObject reticle;
    public GameObject branch1;
    public GameObject branch2;
    public GameObject branch3;
    public GameObject branch4;

    public GameObject player;
    public AudioClip growSound;
    public Image[] sapBranchBars = new Image[4];

    // Public Fields
    public float[] maxSap;
    public float minDistance;
    public float sapCost;
    public float startingSap;

    [Range(0.0f, 10.0f)]
    public float groundHeight;

    [Range(1.0f, 10.0f)]
    public float playerDistance;

    // Local References
    private GameObject _tree;
    private GameObject _reticle;
    // private Text _uitext;
    private AudioSource _source;

    // Local Fields
    private float[] _currentSap;
    private int _selectedBranch;
    private GameObject[] _branches;

    // Local Constants
    private const string BRANCH_TAG = "Branch";
    private const string DEAD_ZONE_TAG = "Dead Zone";
    private const float VERTICAL_SPEED = 2.15F;
    private const float LATERAL_SPEED = 6.30F;
    private const float EPSILON = 0.01F;
    private const float DISTANCE = 2.0F;

    private List<GameObject>[] _branchLeaves;

    void Start() {
        // Establish local references
        _tree = GameObject.FindGameObjectWithTag("Tree");
        _reticle = Instantiate(reticle, Vector3.zero, Quaternion.identity);

        transform.Translate(0.0f, 3.0f, 0.0f);

        _source = _reticle.AddComponent<AudioSource>() as AudioSource;
        _source.playOnAwake = false;
        _source.spatialBlend = 1.0f;

        _branches = new GameObject[]{ branch1, branch2, branch3, branch4 };

        _currentSap = new float[_branches.Length];

        maxSap = new float[] { 8f, 10f, 8f, 9f };

        _branchLeaves = new List<GameObject>[sapBranchBars.Length];

        for (int i = 0; i < sapBranchBars.Length; i++) {
            _branchLeaves[i] = SetBranchLeaves(i);
        }

        Select(0);

        for (int i = 0; i < _branches.Length; i++) {
            UpdateSap(startingSap, i);
            if (_selectedBranch != i) {
                sapBranchBars[i].gameObject.SetActive(false);
            }
        }

        Select(0);

        UpdateReticle();
    }

    public override void UpdateTick() {

        float moveVertical;
        float moveLateral;
        float grow;

        if (GameModel.singlePlayer) {

            if (!GameModel.isSquirrel) {

                // Poll Input
                moveVertical = Input.GetAxis(GameModel.VERTICAL_TREE_INPUT);
                moveLateral = Input.GetAxis(GameModel.HORIZONTAL_TREE_INPUT);

                grow = Input.GetAxis(GameModel.GROW);

            } else {

                moveVertical = 0.0f;
                moveLateral = 0.0f;
                grow = 0.0f;

            }

        } else {

            // Poll Input
            moveVertical = Input.GetAxis(GameModel.VERTICAL_TREE_INPUT);
            moveLateral = Input.GetAxis(GameModel.HORIZONTAL_TREE_INPUT);

            grow = Input.GetAxis(GameModel.GROW);

        }

        bool moved = false;

        // Keep tree player close to squirrel player and out of the ground
        if ((transform.position.y - moveVertical > groundHeight) &&
            (transform.position.y - moveVertical > player.transform.position.y - playerDistance) &&
            (transform.position.y - moveVertical < player.transform.position.y + playerDistance)) {

            // If vertical axis is actuated beyond epsilon value, translate reticle vertically
            if (CheckEpsilon(moveVertical)) {
                transform.Translate(Vector3.up * (moveVertical * Time.deltaTime * VERTICAL_SPEED) * -1, Space.World);
                moved = true;
            }

        } else if ((transform.position.y > player.transform.position.y + playerDistance) &&
                   (moveVertical > 0)) {

            // If vertical axis is actuated beyond epsilon value, translate reticle vertically
            if (CheckEpsilon(moveVertical)) {
                transform.Translate(Vector3.up * (moveVertical * Time.deltaTime * VERTICAL_SPEED) * -1, Space.World);
                 moved = true;
            }

        } else if ((transform.position.y < player.transform.position.y - playerDistance) &&
                   (moveVertical < 0)) {

            // If vertical axis is actuated beyond epsilon value, translate reticle vertically
            if (CheckEpsilon(moveVertical)) {
                transform.Translate(Vector3.up * (moveVertical * Time.deltaTime * VERTICAL_SPEED) * -1, Space.World);
                moved = true;
            }

        }

        // If horizontal axis is actuated beyond epsilon value, translate reticle horizontally
        if (CheckEpsilon(moveLateral)) {
            transform.Translate(Vector3.right * (moveLateral * Time.deltaTime * LATERAL_SPEED), Space.Self);
            moved = true;
        }

        // If the controller was actuated, move the reticle object
        if (moved) {
            UpdateReticle();
        }

        // Handle Branch Selection
        if (Input.GetButtonDown(GameModel.SELECT)) {

            if (GameModel.singlePlayer) {

                if (!GameModel.isSquirrel) {

                    int scrollDirection = Mathf.RoundToInt(Input.GetAxis(GameModel.SELECT));
                    int selected = Mathf.Abs((_branches.Length + scrollDirection + _selectedBranch) % _branches.Length);
                    Select(selected);

                }

            } else {

                int scrollDirection = Mathf.RoundToInt(Input.GetAxis(GameModel.SELECT));
                int selected = Mathf.Abs((_branches.Length + scrollDirection + _selectedBranch) % _branches.Length);
                Select(selected);

            }

        }

        // Handle Growth
        if (GameModel.inputGamePad) {

            if (GameModel.singlePlayer) {

                if (!GameModel.isSquirrel) {

                    if (grow > 0) {
						AttemptGrowBranch();
					}

                }

            } else {

                if (grow > 0) {
					AttemptGrowBranch();
				}

            }

		}
		// Handle Break
		if (CheckEpsilon(Input.GetAxis(GameModel.BREAK)) || (Input.GetButtonDown(GameModel.BREAK))) {
			float distance = float.MaxValue;
			GameObject closestBranch = null;
			Collider[] colliders = Physics.OverlapSphere(_reticle.transform.position, minDistance);
			foreach (Collider iteratedCollider in colliders) {
				if (iteratedCollider.gameObject.tag.Equals(BRANCH_TAG)) {
					float currentDistance = Vector3.Distance(iteratedCollider.transform.position, _reticle.transform.position);
					if (currentDistance < distance) {
						distance = currentDistance;
						closestBranch = iteratedCollider.gameObject;
					}
				}
			}
			if (closestBranch != null) {
				closestBranch.GetComponent<BranchBehavior>().OnBreak();
				Object.Destroy(closestBranch);
			}
		}
		else {

            if (!GameModel.isSquirrel) {

                if (Input.GetButtonDown(GameModel.GROW)) {
					AttemptGrowBranch();
				}

            }

        }

    }

    public override void UpdateAlways() {



    }

	/// <summary>
	/// Attempts to grow a branch.
	/// </summary>
	public void AttemptGrowBranch() {
		if (CanGrow()) {
			// TODO: Switch to growth over time, add vibration and 
			float _volume = Random.Range(GameModel.volLowRange, GameModel.volHighRange);
			_source.PlayOneShot(growSound, _volume);
			Instantiate(_branches[_selectedBranch], _reticle.transform.position, _reticle.transform.rotation);
			UpdateSap(-sapCost, _selectedBranch);
		}
		else {
			// TODO: Feedback if we can't grow!
		}
	}

	/// <summary>
	/// Returns the current reticle transform
	/// </summary>
	public Transform getReticleTransform () {
        return _reticle.transform;
    }

    /// <summary>
    /// The quantity of currently-stored sap.
    /// 
    /// When setting, the passed value will be clamped to be between 0 and the maximum value.
    /// </summary>
    /// <value>The sap quantity.</value>
    public float Sap {
        get => _currentSap[_selectedBranch];
        set => _currentSap[_selectedBranch] = Mathf.Clamp(value, 0.0F, maxSap[_selectedBranch]);
    }

    /// <summary>
    /// Modifies the current sap quantity by the passed value.
    /// </summary>
    /// <param name="passedValue">The value to modify the current sap by.</param>
    public void UpdateSap(float passedValue, int branchType) {
        _currentSap[branchType] = Mathf.Clamp(_currentSap[branchType] + passedValue, 0.0F, maxSap[_selectedBranch]);

        int i = 0;
        foreach (GameObject leaf in _branchLeaves[branchType]) {
            if (i < _currentSap[branchType]) {
                leaf.SetActive(true);
            } else {
                leaf.SetActive(false);
            }
            i++;
        }
    }

    /// <summary>
    /// Handles changes when a branch is selected.
    /// </summary>
    /// <param name="passedIndex">Passed index.</param>
    private void Select(int passedIndex) {
        sapBranchBars[_selectedBranch].gameObject.SetActive(false);
        _selectedBranch = passedIndex;
        sapBranchBars[_selectedBranch].gameObject.SetActive(true);
		// _uitext.text = _branches[_selectedBranch].GetComponent<BranchBehavior>().GetReadableName();
	}

	/// <summary>
	/// Checks to see whether a branch can currently be placed.
	/// </summary>
	/// <returns><c>true</c>, If a branch can be placed, <c>false</c> otherwise.</returns>
	private bool CanGrow() {
        // Check Sap Level
        if (_currentSap[_selectedBranch] < sapCost) {
            return false;
        }

        // Check Branch Closeness (No Branch colliders in min distance)
        Collider[] colliders = Physics.OverlapSphere(_reticle.transform.position, minDistance);
        foreach (Collider iteratedCollider in colliders) {
            if (iteratedCollider.gameObject.tag.Equals(BRANCH_TAG) || iteratedCollider.gameObject.tag.Equals(DEAD_ZONE_TAG)) {
                Debug.Log(iteratedCollider.gameObject.name);
                return false;
            }
        }

        // TODO: Check Cooldown?
        return true;
    }

    /// <summary>
    /// Updates the position and rotation of the reticle object.
    /// </summary>
    private void UpdateReticle() {
        // Horizontal-only raycast
        Vector3 position = _tree.transform.position;
        position.y = transform.position.y;
        _tree.GetComponent<Collider>().Raycast(new Ray(transform.position, position - transform.position), out RaycastHit raycast, 500F);

        // Resolve Position
        transform.position = ((transform.position - raycast.point).normalized * DISTANCE) + raycast.point;

        // Resolve Facing
        transform.LookAt(_tree.transform.position);

        // Resolve Reticle Position
        _reticle.transform.position = raycast.point;

        // Resolve Reticle Facing
        _reticle.transform.LookAt(raycast.point + (raycast.normal * 2.0F));
    }

    /// <summary>
    /// Checks the epsilon of the passed value.
    /// </summary>
    /// <returns><c>true</c>, if epsilon passed, <c>false</c> otherwise.</returns>
    /// <param name="passedValue">Passed value.</param>
    private bool CheckEpsilon(float passedValue) {
        return (System.Math.Abs(passedValue) > EPSILON);
    }

    /// <summary>
    /// Assigns a list of leaf Game Objects to each UI branch.
    /// </summary>
    private List<GameObject> SetBranchLeaves(int branch) {
        List<GameObject> leaves = new List<GameObject>();

        for(int j = 0; j < sapBranchBars[branch].transform.childCount; j++) {
            leaves.Add(sapBranchBars[branch].transform.GetChild(j).gameObject);
        }

        leaves.Reverse();

        return leaves;
    }
}
