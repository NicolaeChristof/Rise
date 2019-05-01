using UnityEngine;
using RiseExtensions;

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
                    treeController.UpdateSap(sap.sapType, sap.sapValue);
                }

                // Call Sap-Specific OnCollected Effect
                sap.OnSapCollected();

            }
        }
    }
}
