using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameModel {

    public static bool inputGamePad = true;

    public static bool singlePlayer = true;

    public static bool splitScreen = true;

    public static bool paused = false;

    public static bool isSquirrel = true;

    public static float volLowRange = 0.5f;

    public static float volHighRange = 1.0f;

    // Inputs
    public static string HORIZONTAL_INPUT = "LS_h";

    public static string VERTICAL_INPUT = "LS_v";

    public static string HORIZONTAL_CAMERA_INPUT = "RS_h";

    public static string VERTICAL_CAMERA_INPUT = "RS_v";

    public static string JUMP = "A";

    public static string SWAP = "RS_B";

    public static string PAUSE = "Start";

    public static string GROW = "RT";

    public static string SELECT = "RB";

}
