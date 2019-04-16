using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using RiseExtensions;
using System;

[RequireComponent(typeof(TreeController))]
public abstract class BranchProvider : RiseBehavior, IComparable<BranchProvider> {

    // Public Fields
    public float maxSap = 8.0F;
    public float startingSap = 5.0F;
    public float sapCost = 1.0F;
    public Sprite branchSprite;
    public Sprite leafSprite;
    public AudioClip growSound;
    public AudioClip cantGrowSound;
    public float minDistance = 0.5F;
    public int sortOrder;

    // Internal Fields
    protected float _currentSap;

    // Properties

    /// <summary>
    /// Gets or sets the quantity of sap directly, bypassing any potential type-sensitive logic.
    /// 
    /// When setting, the value is clamped to [0.0F, maxSap].
    /// </summary>
    /// <value>The sap.</value>
    public float Sap { get => _currentSap; set => _currentSap = value.Clamp(0.0F, maxSap); }

    public virtual void Start() {
        _currentSap = startingSap;
    }

    public virtual void Update() { }

    /// <summary>
    /// Updates the sap using type-sensitive logic.
    /// 
    /// This method should be overridden if type-sensitive logic is needed, and should return <c>true</c> if the sap value was updated, and <c>false</c> otherwise.
    /// </summary>
    /// <returns><c>true</c>, if sap was updated, <c>false</c> otherwise.</returns>
    /// <param name="type">Type.</param>
    /// <param name="quantity">Quantity.</param>
    public virtual bool UpdateSap(SapType type, float quantity) {
        Sap += quantity;
        return true;
    }

    /// <summary>
    /// This method is called after a branch is placed successfully. The passed GameObject is the newly-placed branch.
    /// </summary>
    /// <param name="placedBranch">The placed branch.</param>
    public virtual void OnBranchPlaced(GameObject placedBranch) {
        // Update held Sap
        Sap -= sapCost;

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
        return CheckSap() && CheckCollision(position, rotation);
    }

    /// <summary>
    /// Checks whether there is enough sap to place this branch.
    /// </summary>
    /// <returns><c>true</c>, if sap level is sufficient, <c>false</c> otherwise.</returns>
    public virtual bool CheckSap() {
        return (_currentSap >= sapCost);
    }

    /// <summary>
    /// Checks whether there is enough space to place the branch at the passed position and rotation.
    /// </summary>
    /// <returns><c>true</c>, if there is enough space, <c>false</c> otherwise.</returns>
    /// <param name="position">The position the branch could be placed at.</param>
    /// <param name="rotation">The rotation the branch could be placed at.</param>
    public virtual bool CheckCollision(Vector3 position, Quaternion rotation) {
        // Check Branch Closeness (No Branch colliders in min distance)
        Collider[] colliders = Physics.OverlapSphere(position, minDistance);
        foreach (Collider iteratedCollider in colliders) {
            if (iteratedCollider.gameObject.tag.Equals(Tags.BRANCH) ||
                iteratedCollider.gameObject.tag.Equals(Tags.DEADZONE) ||
                iteratedCollider.gameObject.tag.Equals(Tags.SAP) ||
                iteratedCollider.gameObject.tag.Equals(Tags.PLAYER)) {
                return false;
            }
        }
        return true;
    }

    // Abstract Methods

    /// <summary>
    /// This method should return the branch GameObject that will be placed by this BranchProvider instance.
    /// </summary>
    /// <returns>The branch to be placed.</returns>
    public abstract GameObject GetBranch();

    // IComparable Compliance Method

    public int CompareTo(BranchProvider other) {
        return sortOrder.CompareTo(other.sortOrder);
    }
}
