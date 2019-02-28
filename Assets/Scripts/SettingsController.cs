﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class SettingsController : MonoBehaviour {

    public Camera squirrelCamera;
    public Camera treeCamera;
    /*public PostProcessProfile postProcessProfile;

    public Text[] buttonArray = new Text[3];
    public Image[] selectorArray = new Image[3];
    public GameObject pauseMenu;

    public GameObject mainMenu;

    public float pauseDOF;

    private int _buttonSelected = 0;
    private bool _justSelected;

    private delegate void _selectAction();
    private _selectAction[] _selectActions;
    private _selectAction _currentSelectAction;

    private DepthOfField depthOfField;
    private float defaultDOF;

    private enum menus { Main, Pause, Options, None };
    private menus currentMenu;*/

    // Start is called before the first frame update
    void Start() {

        // Can only use keyboard in single player mode
        if (!GameModel.inputGamePad) {

            GameModel.singlePlayer = true;

        }

        // Single/Multi Player Settings
        if (GameModel.singlePlayer) {

            if (GameModel.inputGamePad) {

                GameModel.HORIZONTAL_SQUIRREL_INPUT = "LS_h_P1";

                GameModel.VERTICAL_SQUIRREL_INPUT = "LS_v_P1";

                GameModel.HORIZONTAL_SQUIRREL_CAMERA_INPUT = "RS_h_P1";

                GameModel.VERTICAL_SQUIRREL_CAMERA_INPUT = "RS_v_P1";

                GameModel.HORIZONTAL_TREE_INPUT = "LS_h_P1";

                GameModel.VERTICAL_TREE_INPUT = "LS_v_P1";

                GameModel.HORIZONTAL_TREE_CAMERA_INPUT = "RS_h_P1";

                GameModel.VERTICAL_TREE_CAMERA_INPUT = "RS_v_P1";

                GameModel.GROW = "RT_P1";

                GameModel.SELECT = "RB_P1";

            } else {

                GameModel.HORIZONTAL_SQUIRREL_INPUT = "Keyboard_player_h";

                GameModel.VERTICAL_SQUIRREL_INPUT = "Keyboard_player_v";

                GameModel.HORIZONTAL_SQUIRREL_CAMERA_INPUT = "Keyboard_camera_h";

                GameModel.VERTICAL_SQUIRREL_CAMERA_INPUT = "Keyboard_camera_v";

                GameModel.HORIZONTAL_TREE_INPUT = "Keyboard_player_h";

                GameModel.VERTICAL_TREE_INPUT = "Keyboard_player_v";

                GameModel.HORIZONTAL_TREE_CAMERA_INPUT = "Keyboard_camera_h";

                GameModel.VERTICAL_TREE_CAMERA_INPUT = "Keyboard_camera_v";

                GameModel.JUMP = "Keyboard_jump";

                GameModel.SWAP = "Keyboard_swap_player";

                GameModel.PAUSE = "Keyboard_pause";

                GameModel.GROW = "Keyboard_trigger";

                GameModel.SELECT = "Keyboard_next";

				GameModel.BREAK = "Keyboard_break";

            }

        } else {

            // Force split screen mode in multiplayer since swap is disabled
            GameModel.splitScreen = true;

            GameModel.HORIZONTAL_SQUIRREL_INPUT = "LS_h_P1";

            GameModel.VERTICAL_SQUIRREL_INPUT = "LS_v_P1";

            GameModel.HORIZONTAL_SQUIRREL_CAMERA_INPUT = "RS_h_P1";

            GameModel.VERTICAL_SQUIRREL_CAMERA_INPUT = "RS_v_P1";

            GameModel.HORIZONTAL_TREE_INPUT = "LS_h_P2";

            GameModel.VERTICAL_TREE_INPUT = "LS_v_P2";

            GameModel.HORIZONTAL_TREE_CAMERA_INPUT = "RS_h_P2";

            GameModel.VERTICAL_TREE_CAMERA_INPUT = "RS_v_P2";

            GameModel.GROW = "RT_P2";

			GameModel.BREAK = "LT_P2";

            GameModel.SELECT = "RB_P2";

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
        /*

        currentMenu = menus.Main;

        GameModel.paused = false;

        _selectActions = new _selectAction[3] { pauseEvent, restartEvent, menuEvent };

        Select(0);

        postProcessProfile.TryGetSettings<DepthOfField>(out depthOfField);

        defaultDOF = depthOfField.focusDistance;*/

    }

    // Update is called once per frame
    void Update() {

        if (Input.GetButtonDown(GameModel.SWAP) && GameModel.singlePlayer) {

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

        /*
        if (GameModel.paused) {

            if ((Input.GetAxis(GameModel.VERTICAL_SQUIRREL_INPUT) < 0) && (_buttonSelected < (buttonArray.Length - 1)) && !_justSelected) {
                _buttonSelected++;
                _justSelected = true;
            } else if ((Input.GetAxis(GameModel.VERTICAL_SQUIRREL_INPUT) > 0) && (_buttonSelected > 0) && !_justSelected) {
                _buttonSelected--;
                _justSelected = true;
            }

            if (_justSelected) {
                Select(_buttonSelected);
            }

            if (Input.GetAxis(GameModel.VERTICAL_SQUIRREL_INPUT) == 0) {
                _justSelected = false;
            }

            if (Input.GetButtonDown(GameModel.JUMP)) {
                _currentSelectAction();
            }

        }*/

    }

    /*
    public void OnApplicationQuit() {
        postProcessProfile.TryGetSettings<DepthOfField>(out depthOfField);

        depthOfField.focusDistance.value = defaultDOF;
    }

    void ChangeMenu(menus passedMenu) {
        currentMenu = passedMenu;

        switch (currentMenu) {
            case menus.Pause:
                pauseEvent();
                break;
            case menus.Main:
                menuEvent();
                break;
            case menus.Options:
                // Set optionmenu active
                break;
            case menus.None:
                break;
        }
    }

    void Select(int button) {
        Debug.Log(button);

        for(int i=0; i<buttonArray.Length; i++) {
            if (i == button) {
                selectorArray[i].gameObject.SetActive(true);
            } else {
                selectorArray[i].gameObject.SetActive(false);
            }
        }

        _currentSelectAction = _selectActions[button];
    }

    void pauseEvent() {
        GameModel.paused = !GameModel.paused;

        if (!GameModel.paused) {
            currentMenu = menus.
            pauseMenu.SetActive(false);
            postProcessProfile.TryGetSettings(out depthOfField);
            depthOfField.focusDistance.value = defaultDOF;
        } else {
            currentMenu = menus.Pause;
            pauseMenu.SetActive(true);
            postProcessProfile.TryGetSettings(out depthOfField);
            depthOfField.focusDistance.value = pauseDOF;
        }
    }

    void restartEvent() {
        pauseEvent();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void menuEvent() {
        currentMenu = menus.Main;

        pauseEvent();

        mainMenu.SetActive(true);
    }
    */

}
