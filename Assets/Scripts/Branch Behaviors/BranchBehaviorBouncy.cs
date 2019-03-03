using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchBehaviorBouncy : BranchBehavior {

	public override void OnTriggerEnter (Collider collider) {

        base.OnTriggerEnter(collider);

		if (collider.gameObject.tag.Equals("Player")) {
			// TODO: Add acceleration using only instantaneousy velocity! Whee!
		}
	}

    public override void OnTriggerStay (Collider collider) {

        base.OnTriggerStay(collider);

    }

    public override void OnTriggerExit (Collider collider) {

        base.OnTriggerExit(collider);

    }
}
