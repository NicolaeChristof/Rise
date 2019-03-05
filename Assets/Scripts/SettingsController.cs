using System.Collections;
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
    public PostProcessProfile postProcessProfile;

    public Text[] buttonArray = new Text[3];
    public Image[] selectorArray = new Image[3];
    public GameObject pauseMenu;

    // Public Fields
    public float pauseDOF;

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

        GameModel.paused = false;

        _selectActions = new _selectAction[3] { pauseEvent, restartEvent, menuEvent };

        Select(0);

        postProcessProfile.TryGetSettings<DepthOfField>(out depthOfField);

        defaultDOF = depthOfField.focusDistance;

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

        if (GameModel.paused) {

            if (((InputHelper.GetAxis(SquirrelInput.MOVE_VERTICAL) > 0) || (InputHelper.GetAxis(TreeInput.MOVE_VERTICAL) > 0)) && (_buttonSelected < (buttonArray.Length - 1)) && !_justSelected) {
                _buttonSelected++;
                _justSelected = true;
            } else if (((InputHelper.GetAxis(SquirrelInput.MOVE_VERTICAL) < 0) || (InputHelper.GetAxis(TreeInput.MOVE_VERTICAL) < 0)) && (_buttonSelected > 0) && !_justSelected) {
                _buttonSelected--;
                _justSelected = true;
            }

            if (_justSelected) {
                Select(_buttonSelected);
            }

            if ((InputHelper.GetAxis(SquirrelInput.MOVE_VERTICAL) == 0) || (InputHelper.GetAxis(TreeInput.MOVE_VERTICAL) == 0)) {
                _justSelected = false;
            }

            if (InputHelper.GetButtonDown(SquirrelInput.JUMP) || InputHelper.GetButtonDown(TreeInput.BRANCH_PLACE)) {
                _currentSelectAction();
            }

        }

    }

    public void OnApplicationQuit() {
        postProcessProfile.TryGetSettings<DepthOfField>(out depthOfField);

        depthOfField.focusDistance.value = defaultDOF;
    }

    void Select(int button) {
        // Debug.Log(button);

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
        pauseMenu.SetActive(!pauseMenu.activeSelf);

        if (GameModel.paused) {
            postProcessProfile.TryGetSettings(out depthOfField);
            depthOfField.focusDistance.value = defaultDOF;
        } else {
            postProcessProfile.TryGetSettings(out depthOfField);
            depthOfField.focusDistance.value = pauseDOF;
        }

        GameModel.paused = !GameModel.paused;
    }

    void restartEvent() {
        pauseEvent();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void menuEvent() {
        Debug.Log("You hit the menu event");
    }

}
