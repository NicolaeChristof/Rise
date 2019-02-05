using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotLightController : MonoBehaviour {

    private Light _light;

    // Start is called before the first frame update
    void Start() {

        _light = GetComponent<Light>();

    }

    // Update is called once per frame
    void Update() {
        
        if (transform.position.y > 5) {

            _light.enabled = true;

        } else {

            _light.enabled = false;

        }

    }
}
