using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SapController : MonoBehaviour {

    // Public Fields
    public float sapPickup;

    // Local References
    private GameObject _treeController;

    void Start() {
        // Resolve local references
        _treeController = GameObject.Find("Tree Controller");
    }

    private void Update() {

    }

    private void OnTriggerEnter(Collider other) {

        if (other.tag == "Sap") {
            // Adjust held sap
            _treeController.GetComponent<TreeController>().UpdateSap(sapPickup);

            // Remove sap object
            other.gameObject.SetActive(false);
            Destroy(other.gameObject);
        }
    }
}
