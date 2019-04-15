using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RiseExtensions;
using DG.Tweening;

public class UIBranchManager : RiseBehavior {

    // Public Fields
    public TreeController treeController;
    public Image branch;
    public List<Image> leaves;
    public Image moveableLeaf;
    public Camera cam;

    // Internal Fields
    private static System.Action<MonoBehaviour> pauseComponent = (component) => { component.enabled = GameModel.paused; };

    private Tween currentTweenable;

    void Start() {
        treeController.uiUpdateEvent += OnUpdateElement;
        moveableLeaf.enabled = false;
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

    public void MoveLeaf(Vector3 startPos) {
        if (currentTweenable != null) {
            currentTweenable.Kill();
        }

        moveableLeaf.enabled = true;

        Vector3 uiPos = cam.WorldToScreenPoint(startPos);
        moveableLeaf.transform.position = uiPos;

        currentTweenable = moveableLeaf.transform.DOMove(transform.position, 2f, false).OnComplete(() => moveableLeaf.enabled = false);
    }
}
