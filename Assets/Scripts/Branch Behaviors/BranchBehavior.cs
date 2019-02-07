using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchBehavior : MonoBehaviour {

	// Public Fields
	public string readableName = "Normal Branch";
	public GameObject knot;

	void Start() {

	}

	void Update() {

	}

	public virtual void OnTriggerEnter(Collider collision) {

	}

	public virtual void OnTriggerStay(Collider collision) {

	}

	public virtual void OnTriggerExit(Collider collision) {

	}

	public virtual void OnBreak() {
		// TODO: Play break sound
		Instantiate(knot, transform.position, transform.rotation);
	}

	// TODO: Internalize? I18n?
	public virtual string GetReadableName() {
		return readableName;
	}
}
