using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeController : MonoBehaviour
{
    // Local References
    private GameObject _tree;
    private GameObject _reticle;

    // Local Constants
    private const string MOVE_VERTICAL = "DPAD_v";
    private const string MOVE_LATERAL = "DPAD_h";
    private const string GROW = "LB";
    private const float SPEED = 2.15F;
    private const float EPSILON = 0.01F;

    void Start()
    {
        // Establish local references
        _tree = GameObject.Find("Tree");
        _reticle = GameObject.Find("Tree Reticle");
    }

    void Update() {
        // Poll Input
        float moveVertical = Input.GetAxis(MOVE_VERTICAL);
        float moveLateral = Input.GetAxis(MOVE_LATERAL);

        bool moved = false;

        // If vertical axis is actuated beyond epsilon value, translate reticle vertically
        if (System.Math.Abs(moveVertical) > EPSILON) {
            transform.Translate(Vector3.up * (moveVertical * Time.deltaTime * SPEED) * -1, Space.World);
            moved = true;
        }

        // If horizontal axis is actuated beyond epsilon value, translate reticle horizontally
        if (System.Math.Abs(moveLateral) > EPSILON) {
            transform.Translate(Vector3.right * (moveLateral * Time.deltaTime * SPEED), Space.Self);
            moved = true;
        }

        // If the controller was actuated, move the reticle object
        if (moved) {
            print(moveVertical);
            print(moveLateral);
        }
    }
}
