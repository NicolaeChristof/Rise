using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchBehaviorBouncy : BranchBehavior {
	void Start() {

	}

	public override void OnTriggerEnter(Collider collision) {
		if (collision.gameObject.tag.Equals("Player")) {
			PlayerController controller = collision.gameObject.GetComponent<PlayerController>();
			controller.SetVelocity(new Vector3(0, 20, 0));
		}
	}

	void Update() {

	}
}
