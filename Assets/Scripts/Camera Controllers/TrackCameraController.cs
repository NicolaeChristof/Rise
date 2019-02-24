using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackCameraController : RiseBehavior {

    // Start is called before the first frame update
    void Start() {



    }

    // Update is called once per frame
    public override void UpdateTick() {



    }

    public override void UpdateAlways() {

        // Debug.Log(this.enabled);

        if (GameModel.paused) {

            // this.enabled = true;

        } else {

            // this.enabled = false;

        }

    }
}
