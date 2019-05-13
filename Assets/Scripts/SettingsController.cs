using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
using RiseExtensions;

public class SettingsController : RiseBehavior {

    // Public References
    public Camera squirrelCamera;
    public Camera treeCamera;

    public PostProcessProfile postProcessProfile;

    public Text[] buttonArray = new Text[3];
    public Image[] selectorArray = new Image[3];
    public GameObject pauseMenu;
    public GameObject canvas;
    // Public Fields
    public float pauseDOF;
    public bool enforceModes;

    // Private References

    // Private Fields
    private int _buttonSelected = 0;
    private bool _justSelected;

    private delegate void _selectAction();
    private _selectAction[] _selectActions;
    private _selectAction _currentSelectAction;

    private DepthOfField depthOfField;
    private float defaultDOF;

    // Start is called before the first frame update
    void Start() {

        InputHelper.Initialize();

        if (enforceModes) {
            EnforceModes();
        } else {
            if (!GameModel.inputGamePad) {
                InputHelper.SetKeyboard(InputHelper.PlayerOne);
            }
        }

        SetCameras();

    }

    // Update is called once per frame
    public override void UpdateTick() {

        InputHelper.Check();

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

        if (GameModel.enableTimer) {

            if (GameModel.timer > 0) {

                GameModel.timer -= Time.deltaTime;

                
                if(Mathf.Floor(GameModel.timer % 60) < 10)
                {
                    GameModel.displayTime = Mathf.Floor((GameModel.timer / 60)).ToString("F0") + ":0" +  Mathf.Floor((GameModel.timer % 60)).ToString("F0"); 
                }
                else
                {
                    GameModel.displayTime = Mathf.Floor((GameModel.timer / 60)).ToString("F0") + ":" + Mathf.Floor((GameModel.timer % 60)).ToString("F0");
                }
                
                

            } else {

                Debug.Log("GAME OVER! You Ran Out Of Time!!");

                GameModel.displayTime = "0:0";

                GameModel.paused = true;
                UIController temp = canvas.GetComponent<UIController>();
                temp.GameOverEvent(false);
            }

            // Debug.Log(GameModel.displayTime);

        }

        

    }

    public void SetCameras() {
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

    public override void UpdateAlways() {



    }

    void LateUpdate() {
        InputHelper.LateCheck();
    }

    // Internal Methods

    private void EnforceModes() {
        // Can only use keyboard in single player mode
        if (!GameModel.inputGamePad) {

            GameModel.singlePlayer = true;

        }

        // Single/Multi Player Settings
        if (GameModel.singlePlayer) {

            GameModel.splitScreen = false;

            if (GameModel.inputGamePad) {

                // by default controls are set for game pad

                GameModel.singlePlayer = true;

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
    }

}
