using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BeeHiveBehavior : MonoBehaviour {

    // Public References
    public GameObject hive;

    public GameObject referencePoint;

    // Public Fields
    public float pushForce;

    // Private References

    // Private Fields
    private Vector3 _originalScale;

    private Vector3 _newScale;

    private bool _canDamage = true;

    // private Vector3 _heading;

    // private float _distance;

    // private Vector3 _direction;

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

            // _heading = collider.gameObject.transform.position - referencePoint.transform.position;
            // _distance = _heading.magnitude;
            // _direction = _heading / _distance;

            // _direction.z = 0.0f;

            // _direction *= pushForce;

            // // Clamp x push
            // if (_direction.x > 0.0f) {

            //     if (_direction.x > 10.0f) {

            //         _direction.x = 10.0f;

            //     } else if (_direction.x < 4.0f) {

            //         _direction.x = 4.0f;

            //     }

            // } else {

            //     if (_direction.x < -10.0f) {

            //         _direction.x = -10.0f;

            //     } else if (_direction.x > -4.0f) {

            //         _direction.x = -4.0f;

            //     }

            // }

            // // Clamp y push
            // if (_direction.y > 4.0f) {

            //     _direction.y = 4.0f;

            // } else if (_direction.y < -4.0f) {

            //     _direction.y = -4.0f;

            // }

            // Debug.Log(_direction);

            hive.transform.DOScale(_newScale, 2.0f)
                .SetEase(Ease.OutElastic);

            // collider.gameObject.GetComponent<PlayerController>().stunPlayer(0.25f);

            // collider.gameObject.GetComponent<PlayerController>().addExternalForce(_direction);

        }

    }

    void OnTriggerStay (Collider collider) {

        if (collider.gameObject.tag.Equals("Player")) {

            if (_canDamage) {

                DisableDamage(1.0f);

                GameModel.squirrelHealth--;

                Debug.Log(GameModel.squirrelHealth);

            }

        }

    }

    void OnTriggerExit (Collider collider) {

        if (collider.gameObject.tag.Equals("Player")) {

            hive.transform.DOScale(_originalScale, 2.0f)
                .SetEase(Ease.OutElastic);

        }

    }

    private void DisableDamage (float disableTime) {

        StartCoroutine(Disable(disableTime));

    }

    private IEnumerator Disable (float disableTime) {

        _canDamage = false;

        yield return new WaitForSeconds(disableTime);

        _canDamage = true;

    }

}
