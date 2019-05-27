using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchBehaviorBouncy : BranchBehavior {

    // Public References

    // Public Fields
    [Range(0.0f, 20.0f)]
    public float bounceHeight;

    // Private References

    // Private Fields

    public override void OnTriggerEnter (Collider collider) {

        base.OnTriggerEnter(collider);

        if (collider.gameObject.tag.Equals("Player")) {

            collider.gameObject.GetComponent<PlayerController>().addExternalForce(new Vector3(0.0f, bounceHeight, 0.0f));

        }
    }

    public override void OnTriggerStay (Collider collider) {

        base.OnTriggerStay(collider);

    }

    public override void OnTriggerExit (Collider collider) {

        base.OnTriggerExit(collider);

    }
}
