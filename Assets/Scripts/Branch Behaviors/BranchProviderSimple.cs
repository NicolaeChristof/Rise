using DG.Tweening;
using UnityEngine;

public class BranchProviderSimple : BranchProvider {

    // Public Fields
    public GameObject branch;
    public float minDistance;

    // Constants
    private const string BRANCH_TAG = "Branch";
    private const string DEAD_ZONE_TAG = "Dead Zone";
    private const string SAP_TAG = "Sap";
    private const string PLAYER_TAG = "Player";

    public override void Start() {
        base.Start();
    }

    public override void UpdateAlways() { }

    public override void UpdateTick() { }

    public override GameObject GetBranch() {
        return branch;
    }

    public override bool CanPlaceBranch(Vector3 position, Quaternion rotation) {
        return base.CanPlaceBranch(position, rotation) && CheckCollision(position, rotation);
    }

    public override void OnBranchPlaced(GameObject placedBranch) {
        base.OnBranchPlaced(placedBranch);
    }

    // Internal Methods
    private bool CheckCollision(Vector3 position, Quaternion rotation) {
        // Check Branch Closeness (No Branch colliders in min distance)
        Collider[] colliders = Physics.OverlapSphere(position, minDistance);
        foreach (Collider iteratedCollider in colliders) {
            if (iteratedCollider.gameObject.tag.Equals(BRANCH_TAG) ||
                iteratedCollider.gameObject.tag.Equals(DEAD_ZONE_TAG) ||
                iteratedCollider.gameObject.tag.Equals(SAP_TAG) ||
                iteratedCollider.gameObject.tag.Equals(PLAYER_TAG)) {
                return false;
            }
        }
        return true;
    }
}
