using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BranchBehavior : MonoBehaviour {

    // Public References
    public AudioClip breakSound;

    public AudioClip rustleSound;

    // Public Fields
    public string readableName = "Normal Branch";
    public GameObject knot;

    // Private References
    private AudioSource _source;

    // Private fields
    private Vector3 _originalRotation;

    private Vector3 _newRotation;

    void Start() {

        _source = GetComponent<AudioSource>();

        _originalRotation = transform.localEulerAngles;

        _newRotation = _originalRotation + Quaternion.AngleAxis(6.0f, Vector3.right).eulerAngles;

    }

    void Update() {



    }

    public virtual void OnTriggerEnter (Collider collider) {

        if (collider.gameObject.tag.Equals("Player")) {

            float _volume = Random.Range(GameModel.volLowRange, GameModel.volHighRange);
            _source.PlayOneShot(rustleSound, _volume);

            transform.DORotate(_newRotation, 2.0f, RotateMode.Fast)
                .SetEase(Ease.OutElastic);

        }

    }

    public virtual void OnTriggerStay (Collider collider) {



    }

    public virtual void OnTriggerExit (Collider collider) {

        if (collider.gameObject.tag.Equals("Player")) {

            transform.DORotate(_originalRotation, 2.0f, RotateMode.Fast)
                .SetEase(Ease.OutElastic);

        }

    }

    public virtual void OnBreak () {
        float _volume = Random.Range(GameModel.volLowRange, GameModel.volHighRange);
        _source.PlayOneShot(breakSound, _volume);

        Instantiate(knot, transform.position, transform.rotation);
    }

    // TODO: Internalize? I18n?
    public virtual string GetReadableName () {
        return readableName;
    }
}
