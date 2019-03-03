using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointBehavior : MonoBehaviour {

    // Public References
    public GameObject web;

    // Private References
    private GameObject _tree;

    private Collider _trigger;

    void Start() {

        _tree = GameObject.FindGameObjectWithTag("Tree");

        _trigger = GetComponent<BoxCollider>();

        transform.position = new Vector3(_tree.transform.position.x, transform.position.y, _tree.transform.position.z);

    }

    void Update() {



    }

    void OnTriggerEnter (Collider collider) {

        if (collider.gameObject.tag.Equals("Player")) {

            web.SetActive(true);

            _trigger.enabled = false;

        }

    }

    void OnTriggerStay (Collider collider) {

        

    }

    void OnTriggerExit (Collider collider) {

        

    }

}
