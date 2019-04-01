using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RiseExtensions;

public class UIBranchManager : RiseBehavior {

    // Public Fields
    public TreeController treeController;
    public Image branch;
    public List<Image> leaves;

    // Internal Fields
    private static System.Action<MonoBehaviour> pauseComponent = (component) => { component.enabled = GameModel.paused; };

    void Start() {
        treeController.uiUpdateEvent += OnUpdateElement;
    }

    void Update() {

    }

    public override void UpdateAlways() {
        pauseComponent(branch);
        leaves.ForEach(pauseComponent);
    }

    public override void UpdateTick() {

    }

    public void OnUpdateElement(BranchProvider provider) {
        branch.sprite = provider.branchSprite;
        int enabledSprites = Mathf.RoundToInt((provider.Sap / provider.maxSap) * leaves.Count);
        for (int index = 0; index < leaves.Count; index += 1) {
            Image iteratedLeaf = leaves[index];
            iteratedLeaf.sprite = provider.leafSprite;
            iteratedLeaf.enabled = (index < enabledSprites);
        }
    }
}
