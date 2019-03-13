using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameModel {

    // General Settings
    public static bool inputGamePad = false;

    public static bool singlePlayer = true;

    public static bool splitScreen = true;

    public static bool paused = false;

    public static bool isSquirrel = true;

    public static bool debugMode = false;

    public static bool startAtMenu = true;

    public static bool inMenu = true;

    public static string gameMode = "Timed Mode"; // "Zen Mode" "Timed Mode"

    public static bool enableTimer = true;

    public static float timer = 300.0f;

    public static string displayTime = "mm:ss";

    // Sound Controls
    public static float volLowRange = 0.8f;

    public static float volHighRange = 1.0f;

}
