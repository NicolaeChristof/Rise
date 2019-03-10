using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BeeHiveBehavior : MonoBehaviour {

    // Public References
    public GameObject hive;

    // Public Fields
    public float pushForce;

    // Private Fields
    private Vector3 _originalScale;

    private Vector3 _newScale;

    private Vector3 _heading;

    private float _distance;

    private Vector3 _direction;

    // Start is called before the first frame update
    void Start() {

        _originalScale = hive.transform.localScale;

        _newScale = new Vector3(_originalScale.x + 0.1f, _originalScale.y, _originalScale.z + 0.1f);
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    void OnTriggerEnter (Collider collider) {

        if (collider.gameObject.tag.Equals("Player")) {

            _heading = collider.gameObject.transform.position - hive.transform.position;
            _distance = _heading.magnitude;
            _direction = _heading / _distance;

            hive.transform.DOScale(_newScale, 2.0f)
                .SetEase(Ease.OutElastic);

            collider.gameObject.GetComponent<PlayerController>().stunPlayer(0.25f);

            collider.gameObject.GetComponent<PlayerController>().addExternalForce(new Vector3(_direction.x * pushForce, _direction.y * pushForce, 0.0f));

        }

    }

    void OnTriggerStay (Collider collider) {

        if (collider.gameObject.tag.Equals("Player")) {

            

        }

    }

    void OnTriggerExit (Collider collider) {

        if (collider.gameObject.tag.Equals("Player")) {

            hive.transform.DOScale(_originalScale, 2.0f)
                .SetEase(Ease.OutElastic);

        }

    }
}
