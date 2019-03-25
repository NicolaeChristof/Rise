using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameController : MonoBehaviour {

    // Public References
    public GameObject vcam1;

    public GameObject vcam2;

    // Private References

    // Public Fields

    // Private Fields

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void OnTriggerEnter (Collider collider) {

        if (collider.gameObject.tag.Equals("Player")) {

            Debug.Log("hello");

            GameModel.endGame = true;

        }

    }

    public void OnTriggerStay (Collider collider) {



    }

    public void OnTriggerExit (Collider collider) {



    }
}
