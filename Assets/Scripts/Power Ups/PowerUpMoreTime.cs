using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PowerUpMoreTime : MonoBehaviour {

    // Public References
    public AudioClip collectSound;

    // Public Fields

    // Private References
    private AudioSource _source;

    // Private Fields
    private Vector3 _newScale;

    private bool _canCollect = true;

    // Start is called before the first frame update
    void Start() {

        _source = GetComponent<AudioSource>();

        _newScale = Vector3.zero;
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    void OnTriggerEnter (Collider collider) {

        if (collider.gameObject.tag.Equals("Player") && _canCollect) {

            _canCollect = false;

            GameModel.timer += 20.0f;

            float _volume = Random.Range(GameModel.volLowRange, GameModel.volHighRange);
            _source.PlayOneShot(collectSound, _volume);

            transform.DOScale(_newScale, 0.75f)
                .OnComplete(()=>Destroy(this));

        }

    }

    void OnTriggerStay (Collider collider) {



    }

    void OnTriggerExit (Collider collider) {



    }
}
