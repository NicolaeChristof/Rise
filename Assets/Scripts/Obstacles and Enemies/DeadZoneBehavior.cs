using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZoneBehavior : MonoBehaviour {

    // Private References
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
}
