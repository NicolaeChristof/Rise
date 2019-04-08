using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RiseExtensions;

public class OptionsUI : RiseBehavior {
    // Each entry in scrollables is a list of strings
    // that are labels for each preset of a configurable option
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
    public List<string> controllerScrollable = new List<string> { };

    private bool _currentAxis;

    // IncrementFunction is a delegate that takes in an int and
    // increments (or decrements) the current scrollable
    private delegate string IncrementFunction(int value);
    private IncrementFunction[] listOfIncrementFunctions;

    // incrementCurrentOption is a reference to the function that will
    // be called to increment (or decrement) the current present
    private IncrementFunction incrementCurrentOption;

    private void Start() {
        listOfIncrementFunctions = new IncrementFunction[] { ChangeController, ChangeQuality };
        scrollables = new List<List<string>> { new List<string> { "Thingy" }, qualityScrollable };
        cursors = new List<int>();

        for (int i = 0; i < listOfIncrementFunctions.Length; i++) {
            cursors.Add(0);
            incrementCurrentOption = listOfIncrementFunctions[i];
        }

        incrementCurrentOption = listOfIncrementFunctions[0];
        Debug.Log(cursors.Count);
    }

    public override void UpdateAlways() {
        _currentAxis = InputHelper.GetAxisDown(SquirrelInput.MOVE_HORIZONTAL);

        if(_currentAxis) {
            incrementCurrentOption(1);
        }/* else if (_currentAxis > 0f) {
            incrementCurrentOption(1);
            cursors[currentOptionSelected] = (cursors[currentOptionSelected] - 1) % scrollables[currentOptionSelected].Count;
        }*/
    }

    public override void UpdateTick() {

    }

    public string ChangeController(int value) {
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
        Debug.Log(currentDisplayText);
        return (currentDisplayText);
    }

    public string ChangeQuality(int value) {
        cursors[currentOptionSelected] = (cursors[currentOptionSelected] + value) % scrollables[currentOptionSelected].Count;
        QualitySettings.SetQualityLevel(cursors[currentOptionSelected]);
        currentDisplayText = scrollables[currentOptionSelected][cursors[currentOptionSelected]];
        Debug.Log(currentDisplayText);
        return (currentDisplayText);
    }

    public string SelectOption(int currentOption) {
        currentOptionSelected = currentOption;
        incrementCurrentOption = listOfIncrementFunctions[currentOptionSelected];
        currentDisplayText = scrollables[currentOptionSelected][cursors[currentOptionSelected]];
        return (currentDisplayText);
    }
}
