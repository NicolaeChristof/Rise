﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
using RiseExtensions;

public class SettingsController : MonoBehaviour {

    // Public References
    public Camera squirrelCamera;
    public Camera treeCamera;

    // Start is called before the first frame update
    void Start() {

        InputHelper.Initialize();

        // Can only use keyboard in single player mode
        if (!GameModel.inputGamePad) {

            GameModel.singlePlayer = true;

        }

        // Single/Multi Player Settings
        if (GameModel.singlePlayer) {

            if (GameModel.inputGamePad) {
			
                // by default controls are set for game pad

            } else {
				InputHelper.SetKeyboard(InputHelper.PlayerOne);
            }

        } else {

            // Force GamePad input in multiplayer
            GameModel.inputGamePad = true;

            // Force split screen mode in multiplayer
            GameModel.splitScreen = true;

            // by default controls are set for game pad

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

    }

    // Update is called once per frame
    void Update() {

        InputHelper.Check();

        // Listen for Pause
        if (InputHelper.Pause()) {

            pauseEvent();

        }

        if (InputHelper.Swap()) {

            GameModel.isSquirrel = !GameModel.isSquirrel;

            if (!GameModel.splitScreen) {

                if (GameModel.isSquirrel) {

                    squirrelCamera.enabled = true;

                    treeCamera.enabled = false;

                } else {

                    squirrelCamera.enabled = false;

                    treeCamera.enabled = true;

                }

            }

        }

    }

}
