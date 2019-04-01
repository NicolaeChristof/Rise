using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsUI : MonoBehaviour {
    public int qualityCursor = 0;

    string[] qualityOptions = new string[] { "Very Low", "Low", "Medium", "High", "Very High", "Ultra" };

    //-----JUST TO TEST-----
    private void Update() {
        if (Input.GetKeyDown(KeyCode.L)) {
            ChangeQuality(true);
        }
        Debug.Log(qualityCursor);
    }

    public void ChangeQuality(bool isIncrementing) {
        qualityCursor = (qualityCursor + 1) % qualityOptions.Length;
    }
}
