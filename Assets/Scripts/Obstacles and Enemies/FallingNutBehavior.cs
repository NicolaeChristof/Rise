using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingNutBehavior : MonoBehaviour {

    // Public References
    public GameObject self;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {

        if (transform.position.y < 3) {

            Invoke("SelfDestruct", 0.2f);

        }
        
    }

    void OnCollisionEnter (Collision collision) {

        if (collision.gameObject.name == "Squirrel") {

            Debug.Log("Player Detected! (Nut)");

        }

        Invoke("SelfDestruct", 0.2f);

    }

    private void SelfDestruct () {

        Destroy(self);

        // add animation/particle effect + sound effect

    }
}
