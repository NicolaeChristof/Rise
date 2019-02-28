using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class UIController : RiseBehavior {

    // The Game Objects that hold each menu
    public GameObject mainMenuObject;
    public GameObject pauseMenuObject;
    public GameObject optionsMenuObject;

    public List<List<GameObject>> listsOfOptionLists;
    private List<GameObject> _optionList;

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

    void Select(int button) {

    }

    public void PauseEvent(bool isTrue) {
        // IMPLMENT : Go to pause screen
    }

    public void MenuEvent(bool isTrue) {
        // IMPLEMENT : Go to menu
    }

    public void SinglePlayerEvent(bool isTrue) {
        // IMPLEMENT : Go to game
    }

    public void TwoPlayerEvent(bool isTrue) {
        Debug.Log("Two Players Acivated");
    }

    public void OptionsEvent(bool isTrue) {
        // IMPLEMENT : Go to options
    }

    public void RestartEvent(bool isTrue) {
        Debug.Log("Restart Acivated");
    }

    public void QualityEvent(bool isTrue) {
        Debug.Log("Quality Activated");
    }

    public void CreditsEvent(bool isTrue) {
        Debug.Log("Credits Activated");
    }

    public void ExitGameEvent(bool isTrue) {
        Debug.Log("Exit Game Activated");
    }

    public void ExitFromPauseEvent(bool isTrue) {
        // IMPLEMENT : Go to main menu
    }

    public void ExitFromOptionsEvent(bool isTrue) {
        // IMPLEMENT : Go to main menu
    }

    private void Start() {
        _listsOfSelectActions[0] = new List<_selectAction> { SinglePlayerEvent, TwoPlayerEvent, OptionsEvent, ExitGameEvent };
        _listsOfSelectActions[1] = new List<_selectAction> { PauseEvent, RestartEvent, ExitFromPauseEvent };
        _listsOfSelectActions[2] = new List<_selectAction> { QualityEvent, CreditsEvent, ExitFromOptionsEvent };

        _selectActionsList = _listsOfSelectActions[0];
    }

    public override void UpdateAlways() {
        // Here we're testing which option in the current menu the
        // user has select and storing it in the _buttonSelected variable
        if((Input.GetAxis(GameModel.VERTICAL_SQUIRREL_INPUT) < 0) && !_justSelected){
            _buttonSelected++;
            _justSelected = true;
        } else if ((Input.GetAxis(GameModel.VERTICAL_SQUIRREL_INPUT) > 0) && !_justSelected) {
            _buttonSelected--;
            _justSelected = true;
        }

        // This is to ensure that you can't select multiple options
        // in one stick push
        if(Input.GetAxis(GameModel.VERTICAL_SQUIRREL_INPUT) == 0) {
            _justSelected = false;
        }

        // This calls a function that updates the menu visually as well as
        // seting what _currentSelectAction is
        if (_justSelected) {
            Select(_buttonSelected);
        }

        // This calls whatever _currentSelectAction is pointing to
        if (Input.GetButtonDown(GameModel.JUMP)) {
            _currentSelectAction(true);
        }
    }

    public override void UpdateTick() {}
}
