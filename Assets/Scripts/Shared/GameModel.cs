﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameModel {

    // General Settings
    public static bool inputGamePad = true;

    public static bool singlePlayer = true;

    public static bool splitScreen = true;

    public static bool paused = true;

    public static bool isSquirrel = true;

	public static bool debugMode = true;

    public static bool startAtMenu = true;

    public static bool inMenu = true;

    // Sound Controls
    public static float volLowRange = 0.8f;

    public static float volHighRange = 1.0f;

    // Inputs
    public static string HORIZONTAL_SQUIRREL_INPUT = "LS_h_P1";

    public static string VERTICAL_SQUIRREL_INPUT = "LS_v_P1";

    public static string HORIZONTAL_SQUIRREL_CAMERA_INPUT = "RS_h_P1";

    public static string VERTICAL_SQUIRREL_CAMERA_INPUT = "RS_v_P1";

    public static string HORIZONTAL_TREE_INPUT = "LS_h_P1";

    public static string VERTICAL_TREE_INPUT = "LS_v_P1";

    public static string HORIZONTAL_TREE_CAMERA_INPUT = "RS_h_P1";

    public static string VERTICAL_TREE_CAMERA_INPUT = "RS_v_P1";

    public static string JUMP = "A_P1";

    public static string SWAP = "RS_B_P1";

    public static string PAUSE = "START_P1";

    public static string GROW = "RT_P1";

	public static string BREAK = "LT_P1";

    public static string SELECT = "RB_P1";

}
