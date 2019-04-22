using UnityEngine;
using RiseExtensions;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class SapController : MonoBehaviour {

    // Public references
    public TreeController treeController;

    // Local References
    private AudioSource _source;

    void Start() {
        _source = GetComponent<AudioSource>();
    }

    private void Update() {

    }

    private void OnTriggerEnter(Collider otherCollider) {
        if (otherCollider.CompareTag(Tags.SAP)) {
            SapBehavior sap = otherCollider.GetComponent<SapBehavior>();
            if (!(sap is null)) {
                // Update Tree Sap
                if (!sap.hasTouched) {
                    StartCoroutine(WaitToUpdateSap(sap));
                }

                // Call Sap-Specific OnCollected Effect
                sap.OnSapCollected();

                // Play Pickup Sound
                float _volume = Random.Range(GameModel.volLowRange, GameModel.volHighRange);
                _source.PlayOneShot(sap.pickupSound, _volume);
            }
        }
    }

    private IEnumerator WaitToUpdateSap(SapBehavior sap) {
        yield return new WaitForSeconds(GameModel.tweenTime-.1f);
        treeController.UpdateSap(sap.sapType, sap.sapValue);
    }
}
