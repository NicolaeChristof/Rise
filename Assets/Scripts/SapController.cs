using UnityEngine;
using RiseExtensions;
using System.Collections;

public class SapController : MonoBehaviour {

    // Public references
    public TreeController treeController;

    // Local References

    void Start() {
        
    }

    private void Update() {

    }

    private void OnTriggerEnter(Collider otherCollider) {
        if (otherCollider.CompareTag(Tags.SAP)) {
            SapBehavior sap = otherCollider.GetComponent<SapBehavior>();
            if (!(sap is null)) {
                // Update Tree Sap
                if (!sap.hasTouched) {
                    // We'll wait until the tweens are just about done
                    // to actually update the values of health and sap
                    StartCoroutine(WaitToUpdateSap(sap));
                }

                // Call Sap-Specific OnCollected Effect
                sap.OnSapCollected();

            }
        }
    }

    private IEnumerator WaitToUpdateSap(SapBehavior sap) {
        // The .05f is included to ensure that there's a bit of wiggle room
        // so that the leaves/health indicators don't disappear for a frame
        yield return new WaitForSeconds(GameModel.tweenTime-.05f);
        treeController.UpdateSap(sap.sapType, sap.sapValue);
    }
}
