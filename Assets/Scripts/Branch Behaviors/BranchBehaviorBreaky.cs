﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchBehaviorBreaky : BranchBehavior {
	void Start() {

	}

	void Update() {

	}

	public override void OnTriggerEnter(Collider collision) {
		print("Entered " + readableName);
	}
}
