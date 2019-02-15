using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchBehaviorBouncy : BranchBehavior {
	void Start() {

	}

	public override void OnTriggerEnter(Collider collision) {
		if (collision.gameObject.tag.Equals("Player")) {
			// TODO: Add acceleration using only instantaneousy velocity! Whee!
		}
	}

	void Update() {

	}
}
