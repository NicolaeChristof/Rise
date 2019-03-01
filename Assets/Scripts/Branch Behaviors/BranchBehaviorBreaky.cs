using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchBehaviorBreaky : BranchBehavior {

    // Public Fields
    public int maxLandings;

    // Local Fields
    private int _landings;

	public override void OnTriggerEnter (Collider collider) {

        base.OnTriggerEnter(collider);

		_landings += 1;
		if (_landings > maxLandings) {
			OnBreak();
			Object.Destroy(gameObject);
		}
	}

    public override void OnTriggerStay (Collider collider) {

        base.OnTriggerStay(collider);

    }

    public override void OnTriggerExit (Collider collider) {

        base.OnTriggerExit(collider);

    }
}
