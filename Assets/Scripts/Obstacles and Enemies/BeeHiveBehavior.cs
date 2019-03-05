using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BeeHiveBehavior : MonoBehaviour {

    // Public References
    public GameObject hive;

    // Private Fields
    private Vector3 _originalScale;

    private Vector3 _newScale;

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

            Debug.Log("Player Detected! (Bee Hive)");

            // Push the player

            hive.transform.DOScale(_newScale, 2.0f)
                .SetEase(Ease.OutElastic);

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
