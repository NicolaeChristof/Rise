using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchBehaviorSlippery : BranchBehavior {

	// Public Fields
	public float slipFactor = 0.1F;

	void Start() {

	}

	void Update() {

	}

	public override void OnTriggerEnter(Collider collision) {
		if (collision.gameObject.tag.Equals("Player")) {
			// TODO: Make it slippery using only instantaneous velocity! Wheee!
		}
	}

	public override void OnTriggerExit(Collider collision) {
		if (collision.gameObject.tag.Equals("Player")) {
			// TODO: Make it slippery using only instantaneous velocity! Wheee!
		}
	}
}