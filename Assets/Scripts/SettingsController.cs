using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsController : MonoBehaviour {

    public Camera squirrelCamera;

    public Camera treeCamera;

    // Start is called before the first frame update
    void Start() {

        // Single/Multi Player Settings
        if (GameModel.singlePlayer) {

            //TODO

        } else {

            //TODO

        }

        // Split Screen Settings
        if (GameModel.splitScreen) {

            squirrelCamera.rect = new Rect(0.0f, 0.0f, 0.5f, 1.0f);

            treeCamera.rect = new Rect(0.5f, 0.0f, 0.5f, 1.0f);

            squirrelCamera.enabled = true;

            treeCamera.enabled = true;

        } else {

            squirrelCamera.rect = new Rect(0.0f, 0.0f, 1.0f, 1.0f);

            treeCamera.rect = new Rect(0.0f, 0.0f, 1.0f, 1.0f);

            squirrelCamera.enabled = true;

            treeCamera.enabled = false;

        }

        // Input Settings
        if (GameModel.inputGamePad) {

            GameModel.HORIZONTAL_INPUT = "LS_h";

            GameModel.VERTICAL_INPUT = "LS_v";

            GameModel.JUMP = "A";

            GameModel.SWAP = "RS_B";

            GameModel.PAUSE = "Start";

            GameModel.GROW = "RT";

            GameModel.SELECT = "RB";

        } else {

            GameModel.HORIZONTAL_INPUT = "Keyboard_player_h";

            GameModel.VERTICAL_INPUT = "Keyboard_player_v";

            GameModel.JUMP = "Keyboard_jump";

            GameModel.SWAP = "Keyboard_swap_player";

            GameModel.PAUSE = "Pause";

            GameModel.GROW = "Keyboard_trigger";

            GameModel.SELECT = "Keyboard_next";

        }

    }

    // Update is called once per frame
    void Update() {

        // Listen for Pause
        if (Input.GetButtonDown(GameModel.PAUSE)) {

            GameModel.paused = !GameModel.paused;

        }

    }
}
