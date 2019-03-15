using UnityEngine;
using DG.Tweening;
using RiseExtensions;

public class SapBehavior : RiseBehavior {

    // Public Fields
    public AudioClip pickupSound;
    public SapType sapType;
    public int sapValue = 1;
    public bool canCollect = true;

    public override void UpdateAlways() {

    }

    public override void UpdateTick() {

    }

    public virtual void OnSapCollected() {
        transform.DOScale(Vector3.zero, 0.75f).OnComplete(() => Destroy(this));
    }
}