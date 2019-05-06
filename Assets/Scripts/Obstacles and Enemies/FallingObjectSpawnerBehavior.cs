using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObjectSpawnerBehavior : MonoBehaviour {

    // Public References
    public GameObject nut;

    // Public Fields
    [Range(1.0f, 5.0f)]
    public float frequency;

    // Start is called before the first frame update
    void Start() {

        InvokeRepeating("SpawnNut", 0.0f, frequency);

    }

    // Update is called once per frame
    void Update() {
        
    }

    private void SpawnNut () {

        if(!GameModel.paused)
        {
            Instantiate(nut, transform.position, transform.rotation);
        }
        

    }
}
