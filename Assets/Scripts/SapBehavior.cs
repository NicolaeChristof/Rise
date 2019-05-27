using UnityEngine;
using DG.Tweening;
using RiseExtensions;

public class SapBehavior : RiseBehavior {

    // Public References
    // Public Fields
    public AudioClip pickupSound;
    public SapType sapType;
    public int sapValue = 1;
    public float scaleFactor = 0.6F;

    // Private References
    private AudioSource _source;
    
    // Private Fields
    private bool _canCollect = true;

    private UIBranchManager uiBranchManager;
    private HealthUI healthUI;

    // This is set to true when the player has touched the sap
    // (to ensure the sap doesn't continue to provide resources
    // while it's going through its animation)
    public bool hasTouched = false;

    public void Start() {
        float scale = (sapValue > 1.0F) ? 1 + (scaleFactor * sapValue) : 1.0F;
        transform.localScale.Set(scale, scale, scale);

        _source = GetComponent<AudioSource>();

        uiBranchManager = FindObjectOfType<UIBranchManager>();
        healthUI = FindObjectOfType<HealthUI>();
    }

    public override void UpdateAlways() {

    }

    public override void UpdateTick() {

    }

    public virtual void OnSapCollected() {
        if (!hasTouched) {
            Vector3 collisionLocation = transform.position;
            uiBranchManager.MoveLeaf(collisionLocation);
            healthUI.MoveAcorn(collisionLocation);
        }

        hasTouched = true;

        if (_canCollect) {
            _canCollect = false;

            // Play Pickup Sound
            float _volume = Random.Range(GameModel.volLowRange, GameModel.volHighRange);
            _source.PlayOneShot(pickupSound, _volume);

            transform.DOScale(Vector3.zero, 0.75f).OnComplete(() => Destroy(this));
        }

    }
}