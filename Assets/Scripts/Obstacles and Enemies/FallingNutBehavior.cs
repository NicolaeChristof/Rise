using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingNutBehavior : MonoBehaviour {

    // Public References
    public GameObject self;

    public AudioClip fallingSound;

    public AudioClip thunkSound;

    // Private References
    private AudioSource _source;

    // Start is called before the first frame update
    void Start() {

        _source = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update() {

        if (transform.position.y < 3) {

            Invoke("SelfDestruct", 0.2f);

        }
        
    }

    void OnTriggerEnter (Collider collider) {

        if (collider.gameObject.tag == "Player") {

            float _volume = Random.Range(GameModel.volLowRange, GameModel.volHighRange);
            _source.PlayOneShot(fallingSound, _volume);

        }

    }

    void OnTriggerStay (Collider collider) {



    }

    void OnTriggerExit (Collider collider) {



    }

    void OnCollisionEnter (Collision collision) {

        if (collision.gameObject.tag == "Player") {

            Debug.Log("Player Detected! (Nut)");

        }

        float _volume = Random.Range(GameModel.volLowRange, GameModel.volHighRange);
        _source.PlayOneShot(thunkSound, _volume);

        Invoke("SelfDestruct", 0.2f);

    }

    void OnCollisionStay (Collision collision) {



    }

    void OnCollisionExit (Collision collision) {



    }

    private void SelfDestruct () {

        Destroy(self);

        // add animation/particle effect + sound effect

    }
}
