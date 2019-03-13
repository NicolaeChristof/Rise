using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RiseExtensions;

public class PortalBehavior : MonoBehaviour {

    // Public References
    public GameObject exitPortal;

    // Public Fields

    // Private References
    private GameObject _portalExit;

    private GameObject _tree;

    // Private Fields
    private Vector3 _target;

    private bool _portalActive = true;

    // Start is called before the first frame update
    void Start() {

        _portalExit = exitPortal.transform.GetChild(0).gameObject;

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

            // exitPortal.GetComponent<PortalBehavior>().ActivatePortal();

            // _portalActive = true;

        }

    }

    void OnTriggerStay (Collider collider) {

        if (collider.gameObject.tag.Equals("Player")) {

            if (InputHelper.GetButtonDown(SquirrelInput.ACTION_1) && _portalActive) {

                _portalActive = false;

                exitPortal.GetComponent<PortalBehavior>().DeactivatePortal();

                Debug.Log(this.name + " my location " + transform.position + " exit location " + _portalExit.transform.position);

                collider.gameObject.transform.position = _portalExit.transform.position;

                // exitPortal.GetComponent<PortalBehavior>().ActivatePortal();

                // _portalActive = true;

            }

        }

    }

    void OnTriggerExit (Collider collider) {

        if (collider.gameObject.tag.Equals("Player")) {

            _portalActive = true;

        }

    }

    public void ActivatePortal () {

        _portalActive = true;

    }

    public void DeactivatePortal () {

        _portalActive = false;

    }

}
