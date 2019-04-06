using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CheckpointBehavior : MonoBehaviour {

    // Public References

    // Public Fields
    [Range(0, 3)]
    public int misletoeNeeded;

    // Private References
    private GameObject _web;

    private GameObject _tree;

    private Collider _trigger;

    // Private Fields
    private Vector3 _originalScale;

    private Vector3 _newScale;

    private bool _activated;

    private int _misletoeCollected;

    void Start() {

        _web = transform.GetChild(0).gameObject;

        _tree = GameObject.FindGameObjectWithTag("Tree");

        _trigger = GetComponent<BoxCollider>();

        transform.position = new Vector3(_tree.transform.position.x, transform.position.y, _tree.transform.position.z);

        _originalScale = _web.transform.localScale;

        _newScale = new Vector3(_originalScale.x + 0.1f, _originalScale.y, _originalScale.z + 0.1f);

        _activated = false;

        _misletoeCollected = 0;

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

        _misletoeCollected++;

        if (_misletoeCollected >= misletoeNeeded) {

            _web.SetActive(false);

        }

    }

}
