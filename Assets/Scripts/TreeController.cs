using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreeController : MonoBehaviour {

    // public references
    public GameObject reticle;
    public GameObject branch;
    public Slider slider;

    // Public Fields
    public float maxSap;
    public float minDistance;
    public float sapCost;
    public float startingSap;

    // Local References
    private GameObject _tree;
    private GameObject _reticle;

    // Local Fields
    private string MOVE_VERTICAL = "RS_v";
    private string MOVE_LATERAL = "RS_h";
    private string GROW = "LB";

    private float _currentSap;

    // Local Constants
    private const string BRANCH_TAG = "Branch";
    private const float VERTICAL_SPEED = 2.15F;
    private const float LATERAL_SPEED = 6.30F;
    private const float EPSILON = 0.01F;
    private const float DISTANCE = 2.0F;

    void Start() {
        // Establish local references
        _tree = GameObject.Find("Tree");
        _reticle = Instantiate(reticle, Vector3.zero, Quaternion.identity);

        // If not using gamepad, switch input bindings
        if(!GameModel.inputGamePad) {
            MOVE_LATERAL = "Keyboard_retical_h";
            MOVE_VERTICAL = "Keyboard_retical_v";
            GROW = "Keyboard_trigger";
        }

        UpdateSap(startingSap);
        UpdateReticle();
    }

    void Update() {
        // Poll Input
        float moveVertical = Input.GetAxis(MOVE_VERTICAL);
        float moveLateral = Input.GetAxis(MOVE_LATERAL);
        float grow = Input.GetAxis(GROW);

        bool moved = false;

        // If vertical axis is actuated beyond epsilon value, translate reticle vertically
        if (CheckEpsilon(moveVertical)) {
            transform.Translate(Vector3.up * (moveVertical * Time.deltaTime * VERTICAL_SPEED) * -1, Space.World);
            moved = true;
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

        if (Input.GetButtonDown(GROW)) {
            if (CanGrow()) {
                // TODO: Switch to growth over time, add vibration and 
                Instantiate(branch, _reticle.transform.position, _reticle.transform.rotation);
                UpdateSap(-sapCost);
            }
            else {
                // TODO: Feedback if we can't grow!
            }
        }
    }

    /// <summary>
    /// Modifies the current sap quantity by the passed value.
    /// </summary>
    /// <param name="passedValue">The value to modify the current sap by.</param>
    public void UpdateSap(float passedValue) {
        _currentSap = Mathf.Clamp(_currentSap + passedValue, 0.0F, maxSap);
        slider.value = (_currentSap / maxSap);
    }

    /// <summary>
    /// Checks to see whether a branch can currently be placed.
    /// </summary>
    /// <returns><c>true</c>, If a branch can be placed, <c>false</c> otherwise.</returns>
    private bool CanGrow() {
        // Check Sap Level
        if (_currentSap < sapCost) {
            return false;
        }

        // Check Branch Closeness (No Branch colliders in min distance)
        Collider[] colliders = Physics.OverlapSphere(_reticle.transform.position, minDistance);
        foreach (Collider iteratedCollider in colliders) {
            if (iteratedCollider.gameObject.tag.Equals(BRANCH_TAG)) {
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
}
