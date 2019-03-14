using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SapController : MonoBehaviour {

    // Public references
    public AudioClip pickupSound;

    // Public Fields
    public float sapPickup;

    // Local References
    private TreeController _treeController;

    private AudioSource _source;

    private SapType _sapType;

    private Vector3 _newScale;

    void Start() {

        _source = GetComponent<AudioSource>();

        // Resolve local references
        _treeController = GameObject.Find("Tree Controller").GetComponent<TreeController>();

        _newScale = Vector3.zero;

    }

    private void Update() {

    }

    private void OnTriggerEnter(Collider collider) {

        if (collider.gameObject.tag.Equals("Sap") && collider.gameObject.GetComponent<SapType>().canCollect) {

            collider.gameObject.GetComponent<SapType>().canCollect = false;

            if (collider.GetComponent<SapType>() != null) {

                _treeController.UpdateSap(collider.GetComponent<SapType>().sapValue, collider.GetComponent<SapType>().sapType);

            } else {
                // Adjust held sap
                _treeController.UpdateSap(sapPickup, 0);
            }

            // Play sound
            float _volume = Random.Range(GameModel.volLowRange, GameModel.volHighRange);
            _source.PlayOneShot(pickupSound, _volume);

            collider.transform.DOScale(_newScale, 0.75f)
                .OnComplete(()=>kill(collider.gameObject));

        }
    }

    private void kill (GameObject sap) {

        // Remove sap object
        Destroy(sap);

    }
}
