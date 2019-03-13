using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FallingNutBehavior : MonoBehaviour {

    // Public References
    public GameObject self;

    public AudioClip fallingSound;

    public AudioClip thunkSound;

    // Private References
    private AudioSource _source;

    // Private Fields
    private Vector3 _newScale;

    // Start is called before the first frame update
    void Start() {

        _source = GetComponent<AudioSource>();

        _newScale = Vector3.zero;

    }

    // Update is called once per frame
    void Update() {

        if (transform.position.y < 3) {

            Invoke("SelfDestruct", 0.2f);

        }
        
    }

    void OnTriggerEnter (Collider collider) {

        if (collider.gameObject.tag.Equals("Player")) {

            float _volume = Random.Range(GameModel.volLowRange, GameModel.volHighRange);
            _source.PlayOneShot(fallingSound, _volume);

        }

    }

    void OnTriggerStay (Collider collider) {



    }

    void OnTriggerExit (Collider collider) {



    }

    void OnCollisionEnter (Collision collision) {

        if (collision.gameObject.tag.Equals("Player")) {

            collision.gameObject.GetComponent<PlayerController>().stunPlayer(0.5f);

        }

        float _volume = Random.Range(GameModel.volLowRange, GameModel.volHighRange);
        _source.PlayOneShot(thunkSound, _volume);

        transform.DOScale(_newScale, 0.75f)
                .OnComplete(SelfDestruct);

    }

    void OnCollisionStay (Collision collision) {



    }

    void OnCollisionExit (Collision collision) {



    }

    private void SelfDestruct () {

        Destroy(self);

    }
}
