using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour {

    public Camera squirrelCamera;

    public Camera treeCamera;

    public Button[] buttonArray = new Button[3];

    //private int buttonSelected = 1;

    //private bool justSelected;

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

        buttonArray[0].onClick.AddListener(menuEvent);
        buttonArray[1].onClick.AddListener(pauseEvent);
        buttonArray[2].onClick.AddListener(restartEvent);

        GameModel.paused = false;

        //buttonArray[buttonSelected].Select();
    }

    // Update is called once per frame
    void Update() {

        // Listen for Pause
        if (Input.GetButtonDown(GameModel.PAUSE)) {

            pauseEvent();

        }

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
            if ((Input.GetAxis(GameModel.HORIZONTAL_SQUIRREL_INPUT) > 0) && (buttonSelected < (buttonArray.Length - 1)) && !justSelected) {
                buttonSelected++;
                justSelected = true;
            } else if ((Input.GetAxis(GameModel.HORIZONTAL_SQUIRREL_INPUT) < 0) && (buttonSelected > 0) && !justSelected) {
                buttonSelected--;
                justSelected = true;
            }

            if (justSelected) {
                buttonArray[buttonSelected].Select();
            }

            if (Input.GetAxis(GameModel.HORIZONTAL_SQUIRREL_INPUT) == 0) {
                justSelected = false;
            }
        } */

    }

    void pauseEvent() {
        buttonArray[0].gameObject.SetActive(!buttonArray[0].IsActive());
        buttonArray[1].gameObject.SetActive(!buttonArray[1].IsActive());
        buttonArray[2].gameObject.SetActive(!buttonArray[2].IsActive());
        GameModel.paused = !GameModel.paused;
    }

    void restartEvent() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void menuEvent() {
        Debug.Log("You hit the menu event");
    }

}
