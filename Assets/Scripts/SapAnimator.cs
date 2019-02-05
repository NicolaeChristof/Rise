using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SapAnimator : MonoBehaviour {

    // Update is called once per frame
    void Update() {

        transform.Rotate(0.0f, 5.0f, 0.0f);
        // transform.Rotate(new Vector3 (15, 30, 45) * Time.deltaTime * 2);

    }

}
