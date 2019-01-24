using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SapController : MonoBehaviour {

    // Public references
    public AudioClip pickupSound;

    // Public Fields
    public float sapPickup;

    // Local References
    private TreeController _treeController;

    private AudioSource _source;

    void Start() {

        _source = GetComponent<AudioSource>();

        // Resolve local references
        _treeController = GameObject.Find("Tree Controller").GetComponent<TreeController>();
    
    }

    private void Update() {

    }

    private void OnTriggerEnter(Collider other) {

        if (other.tag == "Sap") {
            
            // Adjust held sap
            _treeController.UpdateSap(sapPickup);

            // Play sound
            float _volume = Random.Range(GameModel.volLowRange, GameModel.volHighRange);
            _source.PlayOneShot(pickupSound, _volume);

            // Remove sap object
            other.gameObject.SetActive(false);
            Destroy(other.gameObject);
        
        }
    }
}
