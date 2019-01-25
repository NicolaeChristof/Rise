using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchBehavior : MonoBehaviour {

	// Public Fields
	public string readableName = "Normal Branch";

	void Start() {

	}

	void Update() {

	}

	// TODO: Internalize? I18n?
	public virtual string GetReadableName() {
		return readableName;
	}
}
