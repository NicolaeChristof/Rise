using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeController : MonoBehaviour {

    // public references
    public GameObject reticle;
    public GameObject branch;

    // Local References
    private GameObject _tree;
    private GameObject _reticle;

    private string MOVE_VERTICAL;
    private string MOVE_LATERAL;

    // Local Constants
    private const string GROW = "LB";
    private const float VERTICAL_SPEED = 2.15F;
    private const float LATERAL_SPEED = 6.30F;
    private const float EPSILON = 0.01F;
    private const float DISTANCE = 2.0F;

    void Start() {

        // Establish local references
        _tree = GameObject.Find("Tree");
        _reticle = Instantiate(reticle, Vector3.zero, Quaternion.identity);

        if (GameModel.inputGamePad) {

            MOVE_LATERAL = "RS_h";

            MOVE_VERTICAL = "RS_v";

        } else {

            MOVE_LATERAL = "Keyboard_retical_h";

            MOVE_VERTICAL = "Keyboard_retical_v";

        }

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

        if (CheckEpsilon(grow)) {
            OnGrow();
        }
    }

    /// <summary>
    /// Performs the growth update logic.
    /// </summary>
    private void OnGrow() {
        Instantiate(branch, _reticle.transform.position, _reticle.transform.rotation);
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
