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

    private int enabledSprites;

    private int cursor = 0;

    // This is what the size of the bar should usually be,
    // and is determined by whatever local size the bar is
    // when you first hit play in the editor
    private Vector3 startingSize;

    // This is the size of the bar when it inflates
    private float maxScaleUponImpact = 1.1f;


    void Start() {
        treeController.uiUpdateEvent += OnUpdateElement;
        cam = GameObject.FindGameObjectWithTag("Squirrel Camera").GetComponent<Camera>();
        startingSize = transform.localScale;
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

    // This function takes a location in world space,
    // transforms it into screen space, and sets tweens
    // that instantiate and transport the moveableLeaf picture
    // to the branch UI bar
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
        }
    }

    public void DropLeaf() {
        currentProvider = treeController.GetSelectedBranch();
        int currentSap = (int)currentProvider.Sap;

        moveableLeaf.sprite = currentProvider.leafSprite;
        Image currentMoveableLeaf = GameObject.Instantiate(moveableLeaf, leaves[currentSap].transform.position, Quaternion.identity, transform);
        currentMoveableLeaf.transform.localEulerAngles = leaves[currentSap].transform.localEulerAngles;

        Vector3 lastDestination = leaves[currentSap].transform.position;
        lastDestination.y -= 65f;

        Vector3 lastRotation = leaves[currentSap].transform.localEulerAngles;
        lastRotation.z += Random.Range(60f, 270f);

        currentMoveableLeaf.transform.DOMove(lastDestination, GameModel.tweenTime, false).OnComplete(() => DestroyMovingLeaf(currentMoveableLeaf));
        currentMoveableLeaf.transform.DOLocalRotate(lastRotation, GameModel.tweenTime);
        DOTween.ToAlpha(() => currentMoveableLeaf.color, x => currentMoveableLeaf.color = x, 0f, GameModel.tweenTime);
    }

    // This function is called when all the tweens are done.
    // It makes the branch inflate temporarily and disables
    // the moving leaf.
    private void BranchFullyUpdated(Image currentLeaf) {
        cursor--;
        currentLeaf.enabled = false;
        OnUpdateElement(currentProvider);
        transform.DOScale(startingSize * maxScaleUponImpact, .15f).OnComplete(() => transform.DOScale(startingSize, .15f));
    }

    private void DestroyMovingLeaf(Image currentLeaf) {
        currentLeaf.enabled = false;
    }
}
