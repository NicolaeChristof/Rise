using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchBehaviorSlippery : BranchBehavior {

	// Public Fields
	public float slipFactor = 0.1F;

	public override void OnTriggerEnter(Collider collider) {

        base.OnTriggerEnter(collider);

		if (collider.gameObject.tag.Equals("Player")) {
			// TODO: Make it slippery using only instantaneous velocity! Wheee!
		}
	}

    public override void OnTriggerStay(Collider collider) {

        base.OnTriggerStay(collider);

    }

	public override void OnTriggerExit(Collider collider) {

        base.OnTriggerExit(collider);

		if (collider.gameObject.tag.Equals("Player")) {
			// TODO: Make it slippery using only instantaneous velocity! Wheee!
		}
	}
}