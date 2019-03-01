using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BranchBehavior : MonoBehaviour {

    // Public Fields
    public string readableName = "Normal Branch";
    public GameObject knot;

    // Private fields
    private Vector3 _originalRotation;

    private float _deformationAngle;

    void Start() {

        _originalRotation = transform.localEulerAngles;

        _deformationAngle = 6.0f;

    }

    void Update() {



    }

    public virtual void OnTriggerEnter(Collider collider) {

        if (collider.gameObject.tag.Equals("Player")) {

            transform.DORotate(_originalRotation + Quaternion.AngleAxis(_deformationAngle, Vector3.right).eulerAngles, 2.0f, RotateMode.Fast)
                .SetEase(Ease.OutElastic);

        }

    }

    public virtual void OnTriggerStay(Collider collider) {



    }

    public virtual void OnTriggerExit(Collider collider) {

        if (collider.gameObject.tag.Equals("Player")) {

            transform.DORotate(_originalRotation, 2.0f, RotateMode.Fast)
                .SetEase(Ease.OutElastic);

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
