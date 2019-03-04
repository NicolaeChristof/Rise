using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;
using RiseExtensions;

public class UIController : RiseBehavior {

    // The Game Objects that hold each menu
    public GameObject mainMenuObject;
    public GameObject pauseMenuObject;
    public GameObject optionsMenuObject;

    public PostProcessProfile postProcessProfile;

    public List<List<GameObject>> listsOfOptionLists;
    private List<GameObject> _optionList;

    private List<GameObject> menuObjects;
    private static int currentMenu = 0;

    // Select Actions are the functions that get called
    // when you hit a menu option. You pass in a bool
    // that indicates whether you want to perform this
    // function or if you want to revert any changes that
    // were made by this function.
    private delegate void _selectAction(bool isTrue);
    private _selectAction _currentSelectAction;

    private List<List<_selectAction>> _listsOfSelectActions;
    private List<_selectAction> _selectActionsList;

    private int _buttonSelected = 0;
    private bool _justSelected = false;

    private int _buttonToSelect = 0;

    public float pauseDOF;
    private DepthOfField depthOfField;
    private float defaultDOF = 2.94f;

    public Camera _trackCam;

    private bool justPaused = false;

    private float _currentAxis = 0f;
    private bool _pressedSelect = false;
    private bool _pressedPause = false;

    private void Start() {
        // Setting menuObjects to store all the menus in the game
        menuObjects = new List<GameObject> { mainMenuObject, pauseMenuObject, optionsMenuObject };

        // For each option on each menu, we're adding its function to _listsOfSelectActions
        _listsOfSelectActions = new List<List<_selectAction>>();
        _listsOfSelectActions.Add(new List<_selectAction> { SinglePlayerEvent, TwoPlayerEvent, OptionsEvent, ExitGameEvent });
        _listsOfSelectActions.Add(new List<_selectAction> { PauseEvent, RestartEvent, ExitFromPauseEvent });
        _listsOfSelectActions.Add(new List<_selectAction> { QualityEvent, CreditsEvent, ExitFromOptionsEvent });

        // Similar to setting _listsOfSelectActions,
        // this involves configuring which game objects correspond to each thing
        // option
        listsOfOptionLists = new List<List<GameObject>>();
        for (int i = 0; i < _listsOfSelectActions.Count; i++) {
            listsOfOptionLists.Add(new List<GameObject>());
            for (int j = 0; j < menuObjects[i].transform.childCount; j++) {
                GameObject currentlySelectedGameObject = menuObjects[i].transform.GetChild(j).gameObject;

                if (currentlySelectedGameObject.tag != "Graphic") {
                    listsOfOptionLists[i].Add(currentlySelectedGameObject);
                }
            }
        }

        if (GameModel.startAtMenu) {
            GameModel.paused = true;
            MenuEvent(true);
        } else {
            OpenMenu(1, false);
        }
    }


    public override void UpdateAlways() {
        Debug.Log(GameModel.paused);

        if (GameModel.isSquirrel) {
            _currentAxis = InputHelper.GetAxis(SquirrelInput.MOVE_VERTICAL);
            _pressedSelect = InputHelper.GetButtonDown(SquirrelInput.JUMP);
        } else {
            _currentAxis = InputHelper.GetAxis(TreeInput.MOVE_VERTICAL);
            _pressedSelect = InputHelper.GetButtonDown(TreeInput.BRANCH_PLACE);
        }

        if (GameModel.paused) {

            // Here we're testing which option in the current menu the
            // user has select and storing it in the _buttonSelected variable
            if ((_currentAxis > 0f) && !_justSelected) {
                _buttonToSelect++;
                _buttonToSelect = (int)Mathf.Clamp(_buttonToSelect, 0, listsOfOptionLists[currentMenu].Count - 1);
                _justSelected = true;
            } else if ((_currentAxis < 0f) && !_justSelected) {
                _buttonToSelect--;
                _buttonToSelect = (int)Mathf.Clamp(_buttonToSelect, 0, listsOfOptionLists[currentMenu].Count - 1);
                _justSelected = true;
            }

            // This is to ensure that you can't select multiple options
            // in one stick push
            if (_currentAxis == 0) {
                _justSelected = false;
            }

            // This calls a function that updates the menu visually as well as
            // seting what _currentSelectAction is
            if (_justSelected) {
                Select(_buttonToSelect);
            }

            // This calls whatever _currentSelectAction is pointing to
            if (_pressedSelect) {
                _currentSelectAction(true);
            }

            if (currentMenu == 0 || currentMenu == 2) {
                _trackCam.depth = 0;
            } else {
                _trackCam.depth = -2;
            }
        }
    }

    public override void UpdateTick() {
        if (GameModel.isSquirrel) {
            _pressedPause = InputHelper.GetButtonDown(SquirrelInput.PAUSE);
        } else {
            _pressedPause = InputHelper.GetButtonDown(TreeInput.PAUSE);
        }

        if (_pressedPause) {
            PauseEvent(true);
        }

    }

    public void OnApplicationQuit() {
        postProcessProfile.TryGetSettings(out depthOfField);
        depthOfField.focusDistance.value = defaultDOF;
    }

    void Select(int button) {
        for(int i = 0; i < listsOfOptionLists[currentMenu].Count; i++) {
            for (int j = 0; j < listsOfOptionLists[currentMenu][i].transform.childCount; j++) {
                listsOfOptionLists[currentMenu][i].transform.GetChild(j).gameObject.SetActive(false);
            }
        }

        _buttonSelected = button;
        for (int l = 0; l < listsOfOptionLists[currentMenu][_buttonSelected].transform.childCount; l++) {
            listsOfOptionLists[currentMenu][_buttonSelected].transform.GetChild(l).gameObject.SetActive(true);
        }
        _currentSelectAction = _listsOfSelectActions[currentMenu][_buttonSelected];
    }

    public void PauseEvent(bool isTrue) {
        if (!GameModel.paused) {
            GameModel.paused = true;
            OpenMenu(1, true);
        } else {
            GameModel.paused = false;
            OpenMenu(1, false);
        }
    }

    public void MenuEvent(bool isTrue) {
        GameModel.isSquirrel = true;
        OpenMenu(0, true);
    }

    public void SinglePlayerEvent(bool isTrue) {
        GameModel.singlePlayer = true;
        OpenMenu(1, false);
        GameModel.paused = false;
    }

    public void TwoPlayerEvent(bool isTrue) {
        GameModel.singlePlayer = false;
        OpenMenu(1, false);
        GameModel.paused = false;
    }

    public void OptionsEvent(bool isTrue) {
        OpenMenu(2, true);
    }

    public void RestartEvent(bool isTrue) {
        SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex));
        GameModel.startAtMenu = false;
        GameModel.isSquirrel = true;
        OpenMenu(1, false);
        GameModel.paused = false;
    }

    public void QualityEvent(bool isTrue) {
        Debug.Log("Quality Activated");
    }

    public void CreditsEvent(bool isTrue) {
        Debug.Log("Credits Activated");
    }

    public void ExitGameEvent(bool isTrue) {
        Application.Quit();
    }

    public void ExitFromPauseEvent(bool isTrue) {
        SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex));
        GameModel.startAtMenu = true;
        OpenMenu(0, true);
        GameModel.paused = true;
    }

    public void ExitFromOptionsEvent(bool isTrue) {
        OpenMenu(0, true);
    }

    public void OpenMenu(int menu, bool inMenu) {
        menuObjects[currentMenu].SetActive(false);
        currentMenu = menu;

        if (inMenu) {
            GameModel.inMenu = true;
            menuObjects[currentMenu].SetActive(true);
            _selectActionsList = _listsOfSelectActions[menu];
            _optionList = listsOfOptionLists[currentMenu];

            for (int i = 0; i < _optionList.Count; i++) {
                _optionList[i].transform.GetChild(0).gameObject.SetActive(false);
            }

            _buttonToSelect = 0;

            Select(_buttonToSelect);

            postProcessProfile.TryGetSettings(out depthOfField);
            depthOfField.focusDistance.value = pauseDOF;
        } else {
            GameModel.inMenu = false;
            GameModel.paused = false;

            postProcessProfile.TryGetSettings(out depthOfField);
            depthOfField.focusDistance.value = defaultDOF;
        }
    }

}
