using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(TreeController))]
public abstract class BranchProvider : RiseBehavior {

    // Public Fields
    public float maxSap = 8.0F;
    public float startingSap = 5.0F;
    public float sapCost = 1.0F;
    public Sprite branchSprite;
    public Sprite leafSprite;
    public AudioClip growSound;
    public AudioClip cantGrowSound;

    // Internal Fields
    protected float _currentSap;

    public virtual void Start() {
        _currentSap = startingSap;
    }

    public virtual void Update() { }

    /// <summary>
    /// This method is called to update the currently held sap quantity by the passed value.
    /// </summary>
    /// <param name="passedValue">The value to change the sap quantity by.</param>
    public virtual void UpdateSap(float passedValue) {
        _currentSap = Mathf.Clamp(_currentSap + passedValue, 0.0F, maxSap);
    }

    /// <summary>
    /// This method is called after a branch is placed successfully. The passed GameObject is the newly-placed branch.
    /// </summary>
    /// <param name="placedBranch">The placed branch.</param>
    public virtual void OnBranchPlaced(GameObject placedBranch) {
        // Update held Sap
        UpdateSap(-sapCost);

        // Perform tween effect
        placedBranch.transform.DOScale(Vector3.zero, 0.75f)
            // .SetEase(Ease.OutElastic)
            .From();
    }

    /// <summary>
    /// This method performs the actual placement logic of the branch, using the passed position and rotation. These parameters typically come from the reticle position and rotation.
    /// 
    /// If a branch was placed successfully, the instantiated branch instance is returned. If no branch was placed, null is returned.
    /// </summary>
    /// <returns>The branch that was placed, null if no branch was placed.</returns>
    /// <param name="position">Position to place the branch at.</param>
    /// <param name="rotation">Rotation to place the branch at.</param>
    public virtual GameObject PlaceBranch(Vector3 position, Quaternion rotation) {
        if (CanPlaceBranch(position, rotation)) {
            GameObject newBranch = Instantiate(GetBranch(), position, rotation);
            OnBranchPlaced(newBranch);
            return newBranch;
        }
        return null;
    }

    /// <summary>
    /// Returns whether or not a branch can be placed at the passed position and rotation.
    /// 
    /// By default, only checks to see whether the currently-held sap exceeds the sap cost.
    /// </summary>
    /// <returns><c>true</c>, if place branch was caned, <c>false</c> otherwise.</returns>
    /// <param name="position">Position.</param>
    /// <param name="rotation">Rotation.</param>
    public virtual bool CanPlaceBranch(Vector3 position, Quaternion rotation) {
        return (_currentSap >= sapCost);
    }

    /// <summary>
    /// This method should return the branch GameObject that will be placed by this BranchProvider instance.
    /// </summary>
    /// <returns>The branch to be placed.</returns>
    public abstract GameObject GetBranch();
}
