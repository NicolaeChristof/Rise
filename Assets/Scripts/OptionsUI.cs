using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsUI : MonoBehaviour {
    private int qualityCursor = 6;

    public string[] qualityOptions = new string[] { "Very Low", "Low", "Medium", "High", "Very High", "Ultra" };

    //-----JUST TO TEST-----
    private void Start() {
        ChangeQuality(false);
    }

    public void ChangeQuality(bool isIncrementing) {
        if (isIncrementing) {
            qualityCursor = (qualityCursor + 1) % qualityOptions.Length;
        }
        QualitySettings.SetQualityLevel(qualityCursor);
    }

    public string GetCurrentQuality() {
        return qualityOptions[qualityCursor];
    }
}
