using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackCameraController : RiseBehavior {

    private Camera _trackCam;

    // Start is called before the first frame update
    void Start() {

        _trackCam = GetComponent<Camera>();

    }

    // Update is called once per frame
    public override void UpdateTick() {



    }

    public override void UpdateAlways() {

        if (GameModel.paused) {

            _trackCam.depth = 0;

        } else {

            _trackCam.depth = -2;

        }

    }
}
