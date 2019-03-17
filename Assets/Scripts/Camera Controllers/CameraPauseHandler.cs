using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CameraPauseHandler : RiseBehavior {

    // Public Fields
    public Camera managedCamera;
    public bool enableOnSingleplayer;
    public bool enableOnPause;
    public bool enableOnPlay;

    void Start() {
        if (GameModel.singlePlayer) {
            SetComponentStatus(enableOnSingleplayer);
        }
    }

    public override void UpdateAlways() {
        if (GameModel.paused) {
            SetComponentStatus(enableOnPause);
        }
        else {
            SetComponentStatus(enableOnPlay);
        }
    }

    public override void UpdateTick() {

    }

    // Internal Methods
    private void SetComponentStatus(bool status) {
        managedCamera.enabled = status;
        SetStatusIfExisting(GetComponent<AudioSource>(), status);
        SetStatusIfExisting(GetComponent<PostProcessLayer>(), status);
        SetStatusIfExisting(GetComponent<PostProcessVolume>(), status);
    }

    private void SetStatusIfExisting(Behaviour type, bool status) {
        if (type != null) {
            type.enabled = status;
        }
    }
}
