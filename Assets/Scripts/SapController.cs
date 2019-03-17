using UnityEngine;
using RiseExtensions;

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
                // Call Sap-Specific OnCollected Effect
                sap.OnSapCollected();

                // Update Tree Sap
                treeController.UpdateSap(sap.sapType, sap.sapValue);

                // Play Pickup Sound
                float _volume = Random.Range(GameModel.volLowRange, GameModel.volHighRange);
                _source.PlayOneShot(sap.pickupSound, _volume);
            }
        }
    }
}
