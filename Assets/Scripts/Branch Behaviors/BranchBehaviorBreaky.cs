using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchBehaviorBreaky : BranchBehavior {

	// Public Fields
	public int maxLandings;

	// Local Fields
	private int _landings;

	void Start() {

	}

	void Update() {

	}

	public override void OnTriggerEnter(Collider collision) {
		_landings += 1;
		if (_landings > maxLandings) {
			OnBreak();
			Object.Destroy(gameObject);
		}
	}
}
