using DG.Tweening;
using UnityEngine;

public class BranchProviderSimple : BranchProvider {

    // Public Fields
    public GameObject branch;

    public override void Start() {
        base.Start();
    }

    public override void UpdateAlways() { }

    public override void UpdateTick() { }

    public override GameObject GetBranch() {
        return branch;
    }

    public override void OnBranchPlaced(GameObject placedBranch) {
        base.OnBranchPlaced(placedBranch);
    }
}
