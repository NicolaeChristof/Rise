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

	public void OnTriggerEnter(Collider collision) {

	}

	public void OnTriggerStay(Collider collision) {

	}

	public void OnTriggerExit(Collider collision) {

	}

	// TODO: Internalize? I18n?
	public virtual string GetReadableName() {
		return readableName;
	}
}
