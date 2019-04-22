using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CheckpointBehavior : MonoBehaviour {

    // Public References

    // Public Fields
    [Range(0, 5)]
    public int mistletoeNeeded;

    // Private References
    private GameObject _web;

    private GameObject _tree;

    private Collider _trigger;

    // Private Fields
    private Vector3 _originalScale;

    private Vector3 _newScale;

    private bool _activated;

    private int _mistletoeCollected;
    private UIController UI;
    void Start() {

        _web = transform.GetChild(0).gameObject;

        _tree = GameObject.FindGameObjectWithTag("Tree");

        UI = GameObject.Find("Canvas").GetComponent<UIController>();

        _trigger = GetComponent<BoxCollider>();

        transform.position = new Vector3(_tree.transform.position.x, transform.position.y, _tree.transform.position.z);

        _originalScale = _web.transform.localScale;

        _newScale = new Vector3(_originalScale.x + 0.1f, _originalScale.y, _originalScale.z + 0.1f);

        _activated = false;

        _mistletoeCollected = 0;

        if (mistletoeNeeded > 0) {

            _web.SetActive(true);

        } else {

            _web.SetActive(false);

        }

    }

    void Update() {



    }

    void OnTriggerEnter (Collider collider) {

        if (collider.gameObject.tag.Equals("Player")) {

            if (!_activated) {

                _web.SetActive(true);

                _activated = true;

            }

            _web.transform.DOScale(_newScale, 2.0f)
                .SetEase(Ease.OutElastic);

            _web.transform.DOScale(_originalScale, 2.0f)
                .SetEase(Ease.OutElastic);

            UI.NextCheckpoint();
        }

    }

    void OnTriggerStay (Collider collider) {

        if (collider.gameObject.tag.Equals("Player")) {

            

        }

    }

    void OnTriggerExit (Collider collider) {

        if (collider.gameObject.tag.Equals("Player")) {



        }

    }

    public void DeactivateWeb () {

        _mistletoeCollected++;
        UI.UpdateMistletoe();
        if (_mistletoeCollected >= mistletoeNeeded) {

            _web.SetActive(false);

        }

    }

    public int MistletoeCount ()
    {
        return _mistletoeCollected;
    }

}
