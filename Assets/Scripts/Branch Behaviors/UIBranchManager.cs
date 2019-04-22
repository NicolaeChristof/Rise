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
    // public List<bool> leavesEnabled;
    private Camera cam;

    // Internal Fields
    private static System.Action<MonoBehaviour> pauseComponent = (component) => { component.enabled = GameModel.paused; };

    private Tween currentTweenable;

    public Image moveableLeaf;

    private BranchProvider currentProvider;

    private int enabledSprites;

    private int cursor = 0;

    void Start() {
        treeController.uiUpdateEvent += OnUpdateElement;
        cam = GameObject.FindGameObjectWithTag("Squirrel Camera").GetComponent<Camera>();
    }

    void Update() {

    }

    public override void UpdateAlways() {
        if(currentProvider == null && treeController.GetSelectedBranch() != null) {
            Debug.Log("thing");
        }
        pauseComponent(branch);
        leaves.ForEach(pauseComponent);
    }

    public override void UpdateTick() {

    }

    public void OnUpdateElement(BranchProvider provider) {
        branch.sprite = provider.branchSprite;
        enabledSprites = Mathf.RoundToInt(((provider.Sap) / provider.maxSap) * leaves.Count);
        for (int index = 0; index < leaves.Count; index += 1) {
            Image iteratedLeaf = leaves[index];
            iteratedLeaf.sprite = provider.leafSprite;
            iteratedLeaf.enabled = (index < enabledSprites);
        }
    }

    public void MoveLeaf(Vector3 startPos) {
        currentProvider = treeController.GetSelectedBranch();
        int currentSap = (int)currentProvider.Sap - 1;

        if (currentSap+cursor < (leaves.Count-1)) {
            cursor++;
            Vector3 uiPos = cam.WorldToScreenPoint(startPos);
            moveableLeaf.sprite = currentProvider.leafSprite;
            Image currentMoveableLeaf = GameObject.Instantiate(moveableLeaf, uiPos, Quaternion.identity, transform);

            currentMoveableLeaf.transform.DOLocalRotate(leaves[currentSap+cursor].transform.localEulerAngles, GameModel.tweenTime).OnComplete(() => BranchFullyUpdated(currentMoveableLeaf));
            currentMoveableLeaf.transform.DOMove(leaves[currentSap+cursor].transform.position, GameModel.tweenTime, false);
            currentMoveableLeaf.transform.DOScale(currentMoveableLeaf.transform.localScale * .7f, GameModel.tweenTime);
        }
    }

    private void BranchFullyUpdated(Image currentLeaf) {
        cursor--;
        currentLeaf.enabled = false;
        OnUpdateElement(currentProvider);
        transform.DOScale(transform.localScale * 1.1f, .15f).OnComplete(() => transform.DOScale(transform.localScale * (1f/1.1f), .15f));
    }
}
