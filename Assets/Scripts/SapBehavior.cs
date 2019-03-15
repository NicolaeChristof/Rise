using UnityEngine;
using DG.Tweening;
using RiseExtensions;

public class SapBehavior : RiseBehavior {

    // Public Fields
    public AudioClip pickupSound;
    public SapType sapType;
    public int sapValue = 1;
    public float scaleFactor = 0.6F;
    public bool canCollect = true;

    public void Start() {
        float scale = (sapValue > 1.0F) ? 1 + (scaleFactor * sapValue) : 1.0F;
        transform.localScale.Set(scale, scale, scale);
    }

    public override void UpdateAlways() {

    }

    public override void UpdateTick() {

    }

    public virtual void OnSapCollected() {
        transform.DOScale(Vector3.zero, 0.75f).OnComplete(() => Destroy(this));
    }
}