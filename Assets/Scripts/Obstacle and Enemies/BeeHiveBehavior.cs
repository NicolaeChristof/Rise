using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeHiveBehavior : MonoBehaviour {

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    void OnTriggerEnter (Collider collider) {

        if (collider.gameObject.name == "Player") {

            Debug.Log("Player Detected! (Bee Hive)");

            // Push the player

        }

    }
}
