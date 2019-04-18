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
    private Camera cam;

    // Internal Fields
    private static System.Action<MonoBehaviour> pauseComponent = (component) => { component.enabled = GameModel.paused; };

    private Tween currentTweenable;

    public Image moveableLeaf;

    private BranchProvider currentProvider;

    void Start() {
        treeController.uiUpdateEvent += OnUpdateElement;
        cam = GameObject.FindGameObjectWithTag("Squirrel Camera").GetComponent<Camera>();
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
        currentProvider = provider;
        branch.sprite = provider.branchSprite;
        int enabledSprites = Mathf.RoundToInt((provider.Sap / provider.maxSap) * leaves.Count);
        for (int index = 0; index < leaves.Count; index += 1) {
            Image iteratedLeaf = leaves[index];
            iteratedLeaf.sprite = provider.leafSprite;
            iteratedLeaf.enabled = (index < enabledSprites);
        }
    }

    public void MoveLeaf(Vector3 startPos) {
        Vector3 uiPos = cam.WorldToScreenPoint(startPos);
        Image currentMoveableLeaf = GameObject.Instantiate(moveableLeaf, uiPos, Quaternion.identity, transform);

        int sap = (int)currentProvider.Sap - 1;

        leaves[sap].enabled = false;

        currentMoveableLeaf.transform.DOLocalRotate(leaves[sap].transform.localEulerAngles, 2f).OnComplete(() => leaves[sap].enabled = true);
        currentMoveableLeaf.transform.DOMove(leaves[sap].transform.position, 2f, false).OnComplete(() => currentMoveableLeaf.enabled = false);
        currentMoveableLeaf.transform.DOScale(currentMoveableLeaf.transform.localScale * .7f, 2f);
    }

    private void BranchFullyUpdated(Image currentLeaf, int currentSap) {
        currentLeaf.enabled = false;
        leaves[currentSap].enabled = true;
    }
}
