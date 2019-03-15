using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchProviderRandom : BranchProvider {

    // Public Fields
    public List<GameObject> branches;

    public override GameObject GetBranch() {
        return branches[Random.Range(0, branches.Count)];
    }

    public override void UpdateAlways() {

    }

    public override void UpdateTick() {

    }
}
