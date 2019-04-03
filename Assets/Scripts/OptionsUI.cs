using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RiseExtensions;

public class OptionsUI : RiseBehavior {
    private int qualityCursor = 6;

    public List<string> currentScrollable;
    public List<int> cursors;
    public string currentDisplayText;

    public List<string> qualityScrollable = new List<string> { "Very Low", "Low", "Medium", "High", "Very High", "Ultra" };

    public int placeholderCursor = 0;

    private float _currentAxis;

    //-----JUST TO TEST-----
    private void Start() {
        ChangeQuality(false);

        for (int i = 0; i < currentScrollable.Count; i++) {
            cursors[i] = 0;
        }
    }

    public override void UpdateAlways() {
        _currentAxis = InputHelper.GetAxis(SquirrelInput.MOVE_HORIZONTAL);

        if(_currentAxis < 0f) {
            // Decrement the setting
            placeholderCursor = (placeholderCursor + 1) % currentScrollable.Count;
        } else if (_currentAxis > 0f) {
            // Increment the setting
            placeholderCursor = (placeholderCursor - 1) % currentScrollable.Count;
        }
    }

    public override void UpdateTick() {
    }

    /*
    public void ChangeQuality(bool isIncrementing) {
        if (isIncrementing) {
            qualityCursor = (qualityCursor + 1) % qualityOptions.Length;
        }
        QualitySettings.SetQualityLevel(qualityCursor);
    }

    public string GetCurrentQuality() {
        return qualityOptions[qualityCursor];
    }
    */

    private void Select() {
        
    }
}
