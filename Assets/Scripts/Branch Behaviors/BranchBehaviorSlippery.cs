using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchBehaviorSlippery : BranchBehavior {

	// Public Fields
	public float slipFactor = 1000.0F;

	void Start() {

	}

	void Update() {

	}

	public override void OnTriggerEnter(Collider collision) {
		if (collision.gameObject.tag.Equals("Player")) {
			PlayerController controller = collision.gameObject.GetComponent<PlayerController>();
			Vector3 slipVelocity = new Vector3(Random.Range(1.0F, 10.0F), 0, Random.Range(1.0F, 10.0F));
			controller.SetVelocity(slipVelocity * slipFactor);
		}
	}

	public override void OnTriggerExit(Collider collision) {
		if (collision.gameObject.tag.Equals("Player")) {
			PlayerController controller = collision.gameObject.GetComponent<PlayerController>();
			controller.SetVelocity(Vector3.zero);
		}
	}
}