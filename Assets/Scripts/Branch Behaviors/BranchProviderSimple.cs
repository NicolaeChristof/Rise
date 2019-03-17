﻿using DG.Tweening;
using UnityEngine;
using RiseExtensions;

public class BranchProviderSimple : BranchProvider {

    // Public Fields
    public GameObject branch;
    public SapType sapType;

    public override void Start() {
        base.Start();
    }

    public override void UpdateAlways() { }

    public override void UpdateTick() { }

    public override GameObject GetBranch() {
        return branch;
    }

    public override bool UpdateSap(SapType type, float quantity) {
        if (type == sapType) {
            base.UpdateSap(type, quantity);
            return true;
        }
        return false;
    }

    public override void OnBranchPlaced(GameObject placedBranch) {
        base.OnBranchPlaced(placedBranch);
    }
}
