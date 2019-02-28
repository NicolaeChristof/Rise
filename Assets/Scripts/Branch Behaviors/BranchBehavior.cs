using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BranchBehavior : MonoBehaviour {

	// Public Fields
	public string readableName = "Normal Branch";
	public GameObject knot;

    // Private fields
    private Transform restingPosition;

    private float _deformationAngle;

	void Start() {

        restingPosition = transform;

        _deformationAngle = 8.0f;

	}

	void Update() {

	}

	public virtual void OnTriggerEnter(Collider collider) {

        if (collider.gameObject.name == "Squirrel") {

            transform.DORotate(transform.localEulerAngles + Quaternion.AngleAxis(_deformationAngle, Vector3.right).eulerAngles, 0.2f);

            // transform.DORotate(transform.localEulerAngles + Quaternion.AngleAxis(-25, Vector3.right).eulerAngles, 0.5f);

        }

	}

	public virtual void OnTriggerStay(Collider collider) {

	}

	public virtual void OnTriggerExit(Collider collider) {

        if (collider.gameObject.name == "Squirrel") {

            transform.DORotate(transform.localEulerAngles + Quaternion.AngleAxis(-_deformationAngle, Vector3.right).eulerAngles, 0.2f);

        }

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
