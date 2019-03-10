using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchBehaviorSlippery : BranchBehavior {

    // Public Fields
    public float slipForce;

    // Private Fields
    private Vector3 _slipDirection;

    public override void OnTriggerEnter (Collider collider) {

        base.OnTriggerEnter(collider);

        if (collider.gameObject.tag.Equals("Player")) {

            if (Random.Range(-1.0f, 1.0f) > 0) {

                _slipDirection = new Vector3(slipForce, 0.0f, 0.0f);

            } else {

                _slipDirection = new Vector3(-slipForce, 0.0f, 0.0f);

            }

            Debug.Log(_slipDirection);

            collider.gameObject.GetComponent<PlayerController>().stunPlayer(0.25f);

            collider.gameObject.GetComponent<PlayerController>().addExternalForce(_slipDirection);

        }
    }

    public override void OnTriggerStay (Collider collider) {

        base.OnTriggerStay(collider);

    }

    public override void OnTriggerExit (Collider collider) {

        base.OnTriggerExit(collider);

    }
}