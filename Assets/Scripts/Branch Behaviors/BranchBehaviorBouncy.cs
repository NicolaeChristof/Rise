using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchBehaviorBouncy : BranchBehavior {

    private Vector3 _bounceHeight = new Vector3(0.0f, 10.0f, 0.0f);

    public override void OnTriggerEnter (Collider collider) {

        base.OnTriggerEnter(collider);

        if (collider.gameObject.tag.Equals("Player")) {

            collider.gameObject.GetComponent<PlayerController>().addExternalForce(_bounceHeight);

        }
    }

    public override void OnTriggerStay (Collider collider) {

        base.OnTriggerStay(collider);

    }

    public override void OnTriggerExit (Collider collider) {

        base.OnTriggerExit(collider);

    }
}
