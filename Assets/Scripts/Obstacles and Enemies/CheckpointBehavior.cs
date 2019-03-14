using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CheckpointBehavior : MonoBehaviour {

    // Public References
    public GameObject web;

    // Private References
    private GameObject _tree;

    private Collider _trigger;

    private Vector3 _originalScale;

    private Vector3 _newScale;

    private bool _activated;

    void Start() {

        _tree = GameObject.FindGameObjectWithTag("Tree");

        _trigger = GetComponent<BoxCollider>();

        transform.position = new Vector3(_tree.transform.position.x, transform.position.y, _tree.transform.position.z);

        _originalScale = web.transform.localScale;

        _newScale = new Vector3(_originalScale.x + 0.1f, _originalScale.y, _originalScale.z + 0.1f);

        _activated = false;

    }

    void Update() {



    }

    void OnTriggerEnter (Collider collider) {

        if (collider.gameObject.tag.Equals("Player")) {

            if (!_activated) {

                web.SetActive(true);

                _activated = true;

            }

            web.transform.DOScale(_newScale, 2.0f)
                .SetEase(Ease.OutElastic);

            web.transform.DOScale(_originalScale, 2.0f)
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

}
