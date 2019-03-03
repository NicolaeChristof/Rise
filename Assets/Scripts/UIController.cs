using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class UIController : RiseBehavior {

    public InputHelper inputHelper;

    // The Game Objects that hold each menu
    public GameObject mainMenuObject;
    public GameObject pauseMenuObject;
    public GameObject optionsMenuObject;

    public List<List<GameObject>> listsOfOptionLists;
    private List<GameObject> _optionList;

    private List<GameObject> menuObjects;
    private int currentMenu = 0;

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
        for(int i=0; i<_listsOfSelectActions.Count; i++) {
            listsOfOptionLists.Add(new List<GameObject>());
            for (int j = 0; j < menuObjects[i].transform.childCount; j++) {
                GameObject currentlySelectedGameObject = menuObjects[i].transform.GetChild(j).gameObject;

                if (currentlySelectedGameObject.tag != "Graphic") {
                    listsOfOptionLists[i].Add(currentlySelectedGameObject);
                }
            }
        }

        OpenMenu(currentMenu);

        /* TAKE OUT */
        _currentSelectAction = _selectActionsList[0];


        /*
        foreach (List<GameObject> listOfGameObjects in listsOfOptionLists) {
            foreach (GameObject currGameObect in listOfGameObjects) {
                Debug.Log(currGameObect);
            }
        }
        */
    }


    public override void UpdateAlways() {
        // Here we're testing which option in the current menu the
        // user has select and storing it in the _buttonSelected variable
        if((inputHelper.GetAxis(InputHelper.SquirrelInput.MOVE_VERTICAL) < 0) && !_justSelected){
            _buttonToSelect++;
            _buttonToSelect = (int)Mathf.Clamp(_buttonToSelect, 0, listsOfOptionLists[currentMenu].Count-1);
            _justSelected = true;
        } else if ((inputHelper.GetAxis(InputHelper.SquirrelInput.MOVE_VERTICAL) > 0) && !_justSelected) {
            _buttonToSelect--;
            _buttonToSelect = (int)Mathf.Clamp(_buttonToSelect, 0, listsOfOptionLists[currentMenu].Count-1);
            _justSelected = true;
        }

        // This is to ensure that you can't select multiple options
        // in one stick push
        if((inputHelper.GetAxis(InputHelper.SquirrelInput.MOVE_VERTICAL) == 0)) {
            _justSelected = false;
        }

        // This calls a function that updates the menu visually as well as
        // seting what _currentSelectAction is
        if (_justSelected) {
            Select(_buttonToSelect);
        }

        // This calls whatever _currentSelectAction is pointing to
        if (inputHelper.GetButtonDown(InputHelper.SquirrelInput.JUMP)) {
            _currentSelectAction(true);
        }
    }

    public override void UpdateTick() {}

    void Select(int button) {
        //for (int i = 0; i < listsOfOptionLists.Count; i++) {
            for(int i = 0; i < listsOfOptionLists[currentMenu].Count; i++) {
                for (int j = 0; j < listsOfOptionLists[currentMenu][i].transform.childCount; j++) {
                    listsOfOptionLists[currentMenu][i].transform.GetChild(j).gameObject.SetActive(false);
                }
            }
        //}
        _buttonSelected = button;
        for (int l = 0; l < listsOfOptionLists[currentMenu][_buttonSelected].transform.childCount; l++) {
            listsOfOptionLists[currentMenu][_buttonSelected].transform.GetChild(l).gameObject.SetActive(true);
        }
        _currentSelectAction = _listsOfSelectActions[currentMenu][_buttonSelected];
    }

    public void PauseEvent(bool isTrue) {
        OpenMenu(1);
    }

    public void MenuEvent(bool isTrue) {
        OpenMenu(0);
    }

    public void SinglePlayerEvent(bool isTrue) {
        RestartEvent(true);
    }

    public void TwoPlayerEvent(bool isTrue) {
        Debug.Log("Two Players Acivated");
    }

    public void OptionsEvent(bool isTrue) {
        OpenMenu(2);
    }

    public void RestartEvent(bool isTrue) {
        SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex));
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
        OpenMenu(0);
    }

    public void OpenMenu(int menu) {
        menuObjects[currentMenu].SetActive(false);
        currentMenu = menu;
        menuObjects[currentMenu].SetActive(true);
        _selectActionsList = _listsOfSelectActions[menu];
        _optionList = listsOfOptionLists[currentMenu];

        for (int i = 0; i <_optionList.Count; i++) {
            _optionList[i].transform.GetChild(0).gameObject.SetActive(false);
        }

        _buttonToSelect = 0;

        Select(_buttonToSelect);
    }

}
