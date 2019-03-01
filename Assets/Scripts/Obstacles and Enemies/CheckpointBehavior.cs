using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointBehavior : MonoBehaviour {

    public GameObject web;

    private GameObject _tree;

    void Start() {

        _tree = GameObject.FindGameObjectWithTag("Tree");

        transform.position = new Vector3(_tree.transform.position.x, transform.position.y, _tree.transform.position.z);

    }

    void Update() {



    }

    void OnTriggerEnter (Collider collider) {

        if (collider.gameObject.tag.Equals("Player")) {

            Debug.Log("Hello Squirrel");

        }

    }

    void OnTriggerStay (Collider collider) {

        

    }

    void OnTriggerExit (Collider collider) {

        

    }

}
