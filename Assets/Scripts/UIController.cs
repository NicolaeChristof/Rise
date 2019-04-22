using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;
using RiseExtensions;

public class UIController : RiseBehavior {

    // A reference to the settings controller for ensuring cameras
    // are set up correctly
    public SettingsController settingsController;

    // The Game Objects that hold each menu
    public GameObject mainMenuObject;
    public GameObject pauseMenuObject;
    public GameObject optionsMenuObject;
    public GameObject levelSelectMenuObject;
    public GameObject endGameMenuObject;
    public GameObject gameOverMenuObject;
    private GameObject[] checkpoints;
    private List<GameObject> menuObjects;

    // A pointer to the currently selected menu
    private static int currentMenu = 0;

    // The Options within each Menu GameObject
    public List<List<GameObject>> listsOfOptionLists;
    private List<GameObject> _optionList;

    // The post-processing profile that's currently being used
    public PostProcessProfile postProcessProfile;

    // The tracking camera on the main menu
    public Camera _trackCam;

    // A reference to the TreeController
    public TreeController treeController;

    // Select Actions are the functions that get called
    // when you hit a menu option. You pass in a bool
    // that indicates whether you want to perform this
    // function or if you want to revert any changes that
    // were made by this function.
    private delegate void _selectAction(bool isTrue);
    private _selectAction _currentSelectAction;

    // A list of all the functions that are called once the player
    // selects a button
    private List<List<_selectAction>> _listsOfSelectActions;
    private List<_selectAction> _selectActionsList;

    // A pointer to the current button selected
    private int _buttonSelected = 0;

    // A pointer that _buttonSelected will point to once
    // calculations have been made
    private int _buttonToSelect = 0;

    // A boolean that ensures that holding the joystick up/down
    // doesn't rapidly select options
    private bool _justSelected = false;

    // Depth of field settings for when a menu is pulled up
    public float pauseDOF;
    private DepthOfField depthOfField;
    private float defaultDOF = 2.94f;

    // Unsure if still using this
    private bool justPaused = false;

    private float _currentAxis = 0f;
    private bool _pressedSelect = false;
    private bool _pressedPause = false;

    //------Branch UI--------
    public GameObject uiBranches;
    //-----------------------

    //------Height UI--------
    public HeightUIInfo heightUI;

    public Text heightUIText;

    public Slider heightUISlider;
    //-----------------------

    //------Health UI--------
    public GameObject healthUI;
    //-----------------------

    //------Misletoe UI--------
    public GameObject misletoeUI;
    private Text misletoeText;
    private int misletoeIndex = 0;
    //-----------------------

    //------Options UI-----

    // The Text labels that correspond to the control method
    // and graphics level we're using, respectively
    public Text gameModeText;
    public Text controllerText;
    public Text qualityText;

    // _qualityStrings is a string that holds the possible labels
    // for each graphic level. _qualityCursor holds the index of the
    // current label
    private int _qualityCursor;
    private string[] _qualityStrings;

    // The functionality for the controller swap is similar to that
    // of the quality options
    private int _controllerCursor;
    private string[] _controllerStrings;
    //---------------------

    //------Timer UI---------
    public GameObject timerUI;
    private Text timerUIText;
    //-----------------------

    private bool isSinglePlayer;

    private void Start() {
        if (GameModel.singlePlayer)
        {
            isSinglePlayer = true;
        }
        else
        {
            isSinglePlayer = false;
        }
        // Setting menuObjects to store all the menus in the game
        menuObjects = new List<GameObject> { mainMenuObject, pauseMenuObject, optionsMenuObject, levelSelectMenuObject, endGameMenuObject, gameOverMenuObject };

        //-----Menu Functionality List-----

        // Declaring our menu functionality list
        _listsOfSelectActions = new List<List<_selectAction>>();

        // Main Menu
        _listsOfSelectActions.Add(new List<_selectAction> { PlayGameEvent, LevelSelectEvent, OptionsEvent, ExitGameEvent });

        // Pause Menu
        _listsOfSelectActions.Add(new List<_selectAction> { PauseEvent, OptionsEvent, RestartEvent, ExitFromPauseEvent });

        // Options Menu
        _listsOfSelectActions.Add(new List<_selectAction> { GameModeEvent, ControllerEvent, QualityEvent, ExitFromOptionsEvent });

        // Level Select Menu
        _listsOfSelectActions.Add(new List<_selectAction> { SpringEvent, SummerEvent, FallEvent, WinterEvent, ExitLevelSelectEvent });

        // Level End Menu
        _listsOfSelectActions.Add(new List<_selectAction> { NextLevelEvent, ExitEndGameEvent });

        // Game Over Menu
        _listsOfSelectActions.Add(new List<_selectAction> { RestartEvent, ExitEndGameEvent });

        //-------------------------------

        // Get UI timer text
        timerUIText = timerUI.GetComponent<Text>();
        
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

        checkpoints = GameObject.FindGameObjectsWithTag("Checkpoint");
        if(checkpoints.Length != 0)
        {
            misletoeIndex = 0;
            misletoeText = misletoeUI.GetComponent<Text>();
            misletoeText.text = "Mistletoe: " + checkpoints[0].GetComponent<CheckpointBehavior>().MistletoeCount() + "/" + checkpoints[0].GetComponent<CheckpointBehavior>().mistletoeNeeded;
        }
        
        // This ensures that you don't have to go through the main menu
        // when you click restart from the pause menu
        if (GameModel.startAtMenu) {
            GameModel.paused = true;
            MenuEvent(true);
        } else {
            OpenMenu(1, false);
        }

        GameModel.squirrelHealth = 10;
        //-----Options UI----
        _qualityStrings = new string[] { "Extra Low", "Low", "Medium", "High", "Extra High", "Ultra" };
        _qualityCursor = QualitySettings.GetQualityLevel();

        _controllerStrings = new string[] { "Keyboard", "Controller", "Keyboard (No Controller Detected)" };

        UpdateQuality();

        PrepareController(GameModel.inputGamePad, (Input.GetJoystickNames().Length > 0) ? (Input.GetJoystickNames()[0] != "") :
            false);

        ChangeController(_controllerCursor);
        //-------------------
        
    }

    public override void UpdateAlways() {
        // Setting _currentAxis and _pressedSelect depending on whether the player is currently
        // a tree or a squirrel
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
        }

        //---Height UI---
        heightUIText.text = "Height: " + heightUI.currentHeightInMeters.ToString("F1") + "m";
        heightUISlider.value = heightUI.currentHeight / heightUI.treeHeight;
        //---------------
    }

    public override void UpdateTick() {
        // We can just check for the pause button without checking the state
        // of the game since this is in UpdateTick()
        if (InputHelper.Pause()) {
            PauseEvent(true);
        }

        if (GameModel.endGame) {
            EndGameEvent(true);
        }

        if (GameModel.squirrelHealth <= 0)
        {
            //quick fix for game over, will need to be changed
            
            Debug.Log("Game Over! You Died!");
            GameOverEvent(true);
            GameModel.paused = true;
            GameModel.squirrelHealth = 10; //Leads to constant gameovers if this isn't set back to default value
            GameObject Health = GameObject.Find("Health Bar");
            Health.GetComponent<HealthUI>().UpdateHealth();
        }

        timerUIText.text = "Timer: " + GameModel.displayTime;
    }

    // This ensures that the depth of field returns to its initial
    // settings once the game is restarted
    public void OnApplicationQuit() {
        postProcessProfile.TryGetSettings(out depthOfField);
        depthOfField.focusDistance.value = defaultDOF;
    }

    // The function that gets called once you select an option
    // in any menu
    void Select(int button) {
        // Make the selection cursor for the last option invisible
        for(int i = 0; i < listsOfOptionLists[currentMenu].Count; i++) {
            for (int j = 0; j < listsOfOptionLists[currentMenu][i].transform.childCount; j++) {
                if (listsOfOptionLists[currentMenu][i].transform.GetChild(j).gameObject.tag != "Static Option") {
                    listsOfOptionLists[currentMenu][i].transform.GetChild(j).gameObject.SetActive(false);
                }
            }
        }

        // Change _buttonSelected to point to the new option
        _buttonSelected = button;

        // Make the selection cursor for the new option visible
        for (int l = 0; l < listsOfOptionLists[currentMenu][_buttonSelected].transform.childCount; l++) {
            if (listsOfOptionLists[currentMenu][_buttonSelected].transform.GetChild(l).gameObject.tag != "Static Option") {
                listsOfOptionLists[currentMenu][_buttonSelected].transform.GetChild(l).gameObject.SetActive(true);
            }
        }

        // Set the function that will be called when 
        // the player hits the confirmation button
        _currentSelectAction = _listsOfSelectActions[currentMenu][_buttonSelected];
    }

    //----------MENU FUNCTIONS----------
    public void PauseEvent(bool isTrue) {
        // Pause the game
        if (!GameModel.paused) {

            GameModel.paused = true;

            List<GameObject> active = new List<GameObject> { heightUIText.gameObject, heightUISlider.gameObject, uiBranches, healthUI };
            List<GameObject> inactive = new List<GameObject> { };
            SetActiveInactive(active, inactive);

            OpenMenu(1, true);

        // Unpause the game
        } else {

            GameModel.paused = false;

            List<GameObject> active = new List<GameObject> { heightUISlider.gameObject, uiBranches, healthUI };
            List<GameObject> inactive = new List<GameObject> { heightUIText.gameObject };
            SetActiveInactive(active, inactive);

            OpenMenu(1, false);

        }
    }

    public void MenuEvent(bool isTrue) {
        GameModel.isSquirrel = true;
        OpenMenu(0, true);

        List<GameObject> active = new List<GameObject> { };
        List<GameObject> inactive = new List<GameObject> { heightUISlider.gameObject, heightUIText.gameObject, uiBranches, healthUI };
        SetActiveInactive(active, inactive);
    }

    public void PlayGameEvent(bool isTrue) {
        if(isSinglePlayer)
        {
            GameModel.singlePlayer = true;
            GameModel.splitScreen = false;
        }
        else
        {
            GameModel.singlePlayer = false;
            if (!settingsController.enforceModes)
            {
                GameModel.splitScreen = true;
            }
        }
        

        if (settingsController != null)
        {
            settingsController.SetCameras();
        }

        List<GameObject> active = new List<GameObject> { heightUISlider.gameObject, uiBranches, healthUI };
        List<GameObject> inactive = new List<GameObject> { heightUIText.gameObject };
        SetActiveInactive(active, inactive);

        OpenMenu(1, false);
        GameModel.paused = false;
        GameModel.enableTimer = true;
    }

    public void GameModeEvent(bool isTrue)
    {
        if (isSinglePlayer)
        {
            gameModeText.text = "Two Players";
            isSinglePlayer = false;
        }
        else
        {
            gameModeText.text = "One Player";
            isSinglePlayer = true;
        }
    }

    //level select event
    public void LevelSelectEvent(bool isTrue)
    {
        List<GameObject> active = new List<GameObject> { };
        List<GameObject> inactive = new List<GameObject> { heightUISlider.gameObject, heightUIText.gameObject, uiBranches, healthUI };
        SetActiveInactive(active, inactive);

        OpenMenu(3, true);

    }

    //level select menu events
    public void SpringEvent(bool isTrue)
    {
        Debug.Log("Spring Activated");
        SceneManager.LoadScene("Spring Template");
    }

    public void SummerEvent(bool isTrue)
    {
        Debug.Log("Summer Activated");
        SceneManager.LoadScene("Summer Template");
    }

    public void FallEvent(bool isTrue)
    {
        Debug.Log("Fall Activated");
        SceneManager.LoadScene("Fall Template");
    }

    public void WinterEvent(bool isTrue)
    {
        Debug.Log("Winter Activated");
        SceneManager.LoadScene("Winter Template");
    }

    public void ExitLevelSelectEvent(bool isTrue)
    {
        Debug.Log("Exit Level Select Activated");
        List<GameObject> active = new List<GameObject> { };
        List<GameObject> inactive = new List<GameObject> { heightUISlider.gameObject, heightUIText.gameObject, uiBranches, healthUI };
        SetActiveInactive(active, inactive);

        OpenMenu(0, true);
    }


    public void OptionsEvent(bool isTrue) {
        List<GameObject> active = new List<GameObject> { };
        List<GameObject> inactive = new List<GameObject> { heightUISlider.gameObject, heightUIText.gameObject, uiBranches, healthUI };
        SetActiveInactive(active, inactive);
        OpenMenu(2, true);
        if (isSinglePlayer)
        {
            gameModeText.text = "One Player";
        }
        else
        {
            gameModeText.text = "Two Players";
        }
    }

    public void RestartEvent(bool isTrue) {
        GameModel.squirrelHealth = 10;
        SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex));
        GameModel.startAtMenu = false;

        if (GameModel.singlePlayer) {
            GameModel.isSquirrel = true;
            PlayGameEvent(true);
        } else {
            PlayGameEvent(true);
        }


        OpenMenu(1, false);
        GameModel.paused = false;
        GameModel.timer = 300.0f;
        GameModel.endGame = false;
    }

    public void ControllerEvent(bool isTrue) {
        if (GameModel.inputGamePad) {
            PrepareController(false, (Input.GetJoystickNames().Length > 0) ? (Input.GetJoystickNames()[0] != "") :
                false);
        } else {
            PrepareController(true, (Input.GetJoystickNames().Length > 0) ? (Input.GetJoystickNames()[0] != "") :
                false);
        }
    }

    public void QualityEvent(bool isTrue) {
        _qualityCursor = (_qualityCursor + 1) % _qualityStrings.Length;
        UpdateQuality();
    }

    public void ExitGameEvent(bool isTrue) {
        Application.Quit();
    }

    public void ExitFromPauseEvent(bool isTrue) {
        SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex));
        GameModel.startAtMenu = true;

        List<GameObject> active = new List<GameObject> { };
        List<GameObject> inactive = new List<GameObject> { heightUISlider.gameObject, heightUIText.gameObject, uiBranches, healthUI };
        SetActiveInactive(active, inactive);

        OpenMenu(0, true);
        GameModel.paused = true;
        GameModel.timer = 300.0f;
        GameModel.squirrelHealth = 10;
    }

    public void ExitFromOptionsEvent(bool isTrue) {
        List<GameObject> active = new List<GameObject> { };
        List<GameObject> inactive = new List<GameObject> { heightUISlider.gameObject, heightUIText.gameObject, uiBranches, healthUI };
        SetActiveInactive(active, inactive);

        ChangeController(_controllerCursor);

        OpenMenu(0, true);
    }

    public void EndGameEvent(bool isTrue) {
        // Pause the game
        if (!GameModel.paused)
        {

            GameModel.paused = true;

            List<GameObject> active = new List<GameObject> { heightUIText.gameObject, heightUISlider.gameObject, uiBranches , healthUI };
            List<GameObject> inactive = new List<GameObject> { };
            SetActiveInactive(active, inactive);

            OpenMenu(4, true);

        }
    }

    public void NextLevelEvent(bool isTrue) {
        Debug.Log("Next Level");
        GameModel.paused = false;
        GameModel.timer = 300.0f;
        GameModel.endGame = false;
        Scene currentScene = SceneManager.GetActiveScene();
        GameModel.squirrelHealth = 10;

        if (currentScene.name == ("Test Scene")){
            SpringEvent(true);
        }
        else if(currentScene.name == ("Spring Template")){
            SummerEvent(true);
        }
        else if (currentScene.name == ("Summer Template")){
            FallEvent(true);
        }
        else if (currentScene.name == ("Fall Template")){
            WinterEvent(true);
        }
        else if (currentScene.name == ("Winter Template")){
            SpringEvent(true);
        }

    }

    public void ExitEndGameEvent(bool isTrue) {
        Debug.Log("Exit to Main Menu");
        GameModel.endGame = false;
        SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex));
        List<GameObject> active = new List<GameObject> { };
        List<GameObject> inactive = new List<GameObject> { heightUISlider.gameObject, heightUIText.gameObject, uiBranches, healthUI };
        SetActiveInactive(active, inactive);

        OpenMenu(0, true);
    }

    public void GameOverEvent(bool isTrue)
    {
        Debug.Log("Game Over");
        List<GameObject> active = new List<GameObject> { heightUIText.gameObject, heightUISlider.gameObject, uiBranches, healthUI };
        List<GameObject> inactive = new List<GameObject> { };
        SetActiveInactive(active, inactive);
        GameModel.timer = 300.0f;
        OpenMenu(5, true);
    }

    //----------MENU FUNCTIONS----------

    //----------PRIVATE HELPER FUNCTIONS----------
    private void OpenMenu(int menu, bool inMenu) {
        // Make the last menu invisible
        menuObjects[currentMenu].SetActive(false);

        // Set currentMenu to point to the just selected menu 
        currentMenu = menu;

        // If the current menu is the pause menu, don't set the tracking
        // camera to be the active camera (although this function isn't
        // currently working)
        if(currentMenu == 1) {
            GameModel.menuCameraEnabled = false;
        } else {
            GameModel.menuCameraEnabled = true;
        }

        // This is true if you want to access a menu
        if (inMenu) {
            // Sets the inMenu flag to true (this variable isn't currently used
            // in this script)
            GameModel.inMenu = true;

            // Set the text options in the current menu active,
            // tell the current menu what functions it can potentially call,
            // and match the current text fields to their corresponding functions
            menuObjects[currentMenu].SetActive(true);
            _selectActionsList = _listsOfSelectActions[menu];
            _optionList = listsOfOptionLists[currentMenu];

            // Make sure that all the selection cursors are invisible
            for (int i = 0; i < _optionList.Count; i++) {
                _optionList[i].transform.GetChild(0).gameObject.SetActive(false);
            }

            // Make the active opiton the topmost option in the current menu
            _buttonToSelect = 0;
            Select(_buttonToSelect);

            // Since the game is paused, we're going to make the depth of field
            // deeper
            postProcessProfile.TryGetSettings(out depthOfField);
            depthOfField.focusDistance.value = pauseDOF;

        // This is true if you want to return to activel playing the game
        } else {
            // Set the inMenu and paused variables to false,
            // since we're no longer in a menu and the game is actively
            // being played
            GameModel.inMenu = false;
            GameModel.paused = false;

            // Since the game's no longer paused, we'll
            // make the depth of field shallow
            postProcessProfile.TryGetSettings(out depthOfField);
            depthOfField.focusDistance.value = defaultDOF;
        }
    }

    private void SetActiveInactive(List<GameObject> activeObjects, List<GameObject> inactiveObjects) {
        foreach(GameObject activeObject in activeObjects) {
            if (!activeObject.activeSelf) {
                activeObject.SetActive(true);
            }
        }

        foreach (GameObject inactiveObject in inactiveObjects) {
            if (inactiveObject.activeSelf) {
                inactiveObject.SetActive(false);
            }
        }
    }

    private void UpdateQuality() {
        QualitySettings.SetQualityLevel(_qualityCursor);
        qualityText.text = _qualityStrings[_qualityCursor];
    }

    // A function that sets the controller cursor accordingly.
    // It doesn't actually change the control method (it just tells
    // ExitFromOptionsEvent what control methods we're going to switch to)
    private void PrepareController(bool useController, bool controllerConnected) {
        if (useController) {
            if (controllerConnected) {
                _controllerCursor = 1;
            } else {
                _controllerCursor = 2;
            }
        } else {
            _controllerCursor = 0;
        }

        controllerText.text = _controllerStrings[_controllerCursor];
    }

    // This function takes the controller cursor and uses it
    // to actually set your input method. It's mainly called
    // from the ExitFromOptionsEvent function
    private void ChangeController(int controllerOption) {
        switch (controllerOption) {
            case 0:
                GameModel.inputGamePad = false;
                InputHelper.SetKeyboard(InputHelper.PlayerOne);
                break;
            case 1:
                GameModel.inputGamePad = true;
                InputHelper.Initialize();
                break;
            case 2:
                GameModel.inputGamePad = false;
                InputHelper.SetKeyboard(InputHelper.PlayerOne);
                break;
            default:
                GameModel.inputGamePad = false;
                InputHelper.SetKeyboard(InputHelper.PlayerOne);
                break;
        }
    }
    //--------------------------------------------

    public void UpdateMistletoe()
    {
        CheckpointBehavior temp = checkpoints[misletoeIndex].GetComponent<CheckpointBehavior>();
        misletoeText = misletoeUI.GetComponent<Text>();
        misletoeText.text = "Mistletoe: " + temp.MistletoeCount() + "/" + temp.mistletoeNeeded;
        
    }

    public void NextCheckpoint()
    {
        CheckpointBehavior temp = checkpoints[misletoeIndex].GetComponent<CheckpointBehavior>();
        if (temp.MistletoeCount() >= temp.mistletoeNeeded && misletoeIndex < checkpoints.Length-1)
        {
            Debug.Log(checkpoints.Length);
            misletoeIndex++;
            UpdateMistletoe();
        }
        
    }
}
