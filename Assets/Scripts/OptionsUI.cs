using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RiseExtensions;

public class OptionsUI : RiseBehavior {
    // currentScrollable is the list of presets for each configurable
    // option in the options menu
    public List<List<string>> scrollables;

    // cursors is a list of integers that marks which preset is selected
    // for each configurable option in the options menu. currentOptionSelected
    // is a pointer to the current cursor
    public List<int> cursors;
    public int currentOptionSelected = 0;

    // currentDisplayText shows the text label for the active preset of the
    // current configurable option
    public string currentDisplayText;

    // This is the scrollable list of labels for quality presets
    public List<string> qualityScrollable = new List<string> { "Very Low", "Low", "Medium", "High", "Very High", "Ultra" };

    private float _currentAxis;

    // IncrementFunction is a delegate that takes in an int and
    // increments (or decrements) the current scrollable
    private delegate void IncrementFunction(int value);
    private IncrementFunction[] listOfIncrementFunctions;

    // incrementCurrentOption is a reference to the function that will
    // be called to increment (or decrement) the current present
    private IncrementFunction incrementCurrentOption;

    private void Start() {
        listOfIncrementFunctions = new IncrementFunction[] { ChangeController, ChangeQuality };
        scrollables = new List<List<string>> { new List<string> { "Thingy" }, qualityScrollable };

        for (int i = 0; i < listOfIncrementFunctions.Length; i++) {
            cursors[i] = 0;
            incrementCurrentOption = listOfIncrementFunctions[i];
            incrementCurrentOption(0);
        }

        incrementCurrentOption = listOfIncrementFunctions[0];
    }

    public override void UpdateAlways() {
        _currentAxis = InputHelper.GetAxis(SquirrelInput.MOVE_HORIZONTAL);

        if(_currentAxis < 0f) {
            incrementCurrentOption(-1);
            cursors[currentOptionSelected] = (cursors[currentOptionSelected] + 1) % scrollables[currentOptionSelected].Count;
        } else if (_currentAxis > 0f) {
            incrementCurrentOption(1);
            cursors[currentOptionSelected] = (cursors[currentOptionSelected] - 1) % scrollables[currentOptionSelected].Count;
        }
    }

    public override void UpdateTick() {

    }

    public void ChangeController(int value) {
        bool controllerConnected = (Input.GetJoystickNames().Length > 0);

        if (GameModel.inputGamePad) {
            GameModel.inputGamePad = false;
            currentDisplayText = "Keyboard";
            InputHelper.SetKeyboard(InputHelper.PlayerOne);
        } else {
            if (controllerConnected) {
                GameModel.inputGamePad = true;
                currentDisplayText = "Controller";
                InputHelper.Initialize();
            } else {
                currentDisplayText = "Keyboard (No Controller)";
            }
        }
    }

    public void ChangeQuality(int value) {
        cursors[currentOptionSelected] = (cursors[currentOptionSelected] + 1) % scrollables[currentOptionSelected].Count;
        QualitySettings.SetQualityLevel(cursors[currentOptionSelected]);
        currentDisplayText = scrollables[currentOptionSelected][cursors[currentOptionSelected]];
    }

    public void SelectOption(int currentOption) {
        currentOptionSelected = currentOption;
        incrementCurrentOption = listOfIncrementFunctions[currentOptionSelected];
        currentDisplayText = scrollables[currentOptionSelected][cursors[currentOptionSelected]];
    }
}
