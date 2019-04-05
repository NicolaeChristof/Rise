using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RiseExtensions;

public class BranchProviderRandom : BranchProvider {

    // Public Fields
    public List<GameObject> branches;

    public override GameObject GetBranch() {
        return branches[Random.Range(0, branches.Count)];
    }

    public override bool UpdateSap(SapType type, float quantity) {
        // TODO: Override for sap-sensitive logic - What specific type of sap should be used for the Random branch, if any?
        return base.UpdateSap(type, quantity);
    }

    public override void UpdateAlways() {

    }

    public override void UpdateTick() {

    }
}
