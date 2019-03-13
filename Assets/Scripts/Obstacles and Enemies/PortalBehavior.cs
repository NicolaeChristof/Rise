using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalBehavior : MonoBehaviour {

    // Public References
    public GameObject exitPortal;

    // Public Fields

    // Private References
    private GameObject _portalExit;

    private GameObject _tree;

    // Private Fields
    private Vector3 _target;

    // Start is called before the first frame update
    void Start() {

        _tree = GameObject.FindGameObjectWithTag("Tree");

        _target = new Vector3(_tree.transform.position.x,
                              transform.position.y,
                              _tree.transform.position.z);

        transform.LookAt(_target);

    }

    // Update is called once per frame
    void Update() {
        
    }

    void OnTriggerEnter (Collider collider) {

        if (collider.gameObject.tag.Equals("Player")) {

            Debug.Log("hello");

        }

    }

    void OnTriggerStay (Collider collider) {



    }

    void OnTriggerExit (Collider collider) {



    }
}
