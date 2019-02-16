﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.CompilerServices;

public class InputHelper : MonoBehaviour {
	// Public Fields
	public enum SquirrelInput {
		MOVE_HORIZONTAL,
		MOVE_VERTICAL,
		CAMERA_HORIZONTAL,
		CAMERA_VERTICAL,
		SWAP,
		PAUSE,

		JUMP,
	}

	public enum TreeInput {
		MOVE_HORIZTONAL,
		MOVE_VERTICAL,
		CAMERA_HORIZONTAL,
		CAMERA_VERTICAL,
		SWAP,
		PAUSE,

		BRANCH_PLACE,
		BRANCH_REMOVE,
		SELECT_LEFT,
		SELECT_RIGHT,
	}

	public enum InputMode {
		SQUIRREL,
		TREE
	}

	// Controller Bindings
	public readonly static string[] RT = { "RT_P1", "RT_P2" };
	public readonly static string[] LT = { "LT_P1", "LT_P2" };
	public readonly static string[] RB = { "RB_P1", "RB_P2" };
	public readonly static string[] LB = { "LB_P1", "LB_P2" };
	public readonly static string[] A = { "A_P1", "A_P2" };
	public readonly static string[] B = { "B_P1", "B_P2" };
	public readonly static string[] X = { "X_P1", "X_P2" };
	public readonly static string[] Y = { "Y_P1", "Y_P2" };
	public readonly static string[] DP_h = { "DP_h_P1", "DP_h_P2" };
	public readonly static string[] DP_v = { "DP_v_P1", "DP_v_P2" };
	public readonly static string[] RS_h = { "RS_h_P1", "RS_h_P2" };
	public readonly static string[] RS_v = { "RS_v_P1", "RS_v_P2" };
	public readonly static string[] LS_h = { "LS_h_P1", "LS_h_P2" };
	public readonly static string[] LS_v = { "LS_v_P1", "LS_v_P2" };
	public readonly static string[] START = { "START_P1", "START_P2" };
	public readonly static string[] BACK = { "BACK_P1", "BACK_P2" };

	// Local Fields
	private ControlProfile _playerOne;
	private ControlProfile _playerTwo;

	// Local Objects
	private readonly static Func<ControlProfile, TreeInput, string> BINDING_TREE = (profile, key) => { return profile.GetBinding(key); };
	private readonly static Func<ControlProfile, SquirrelInput, string> BINDING_SQUIRREL = (profile, key) => { return profile.GetBinding(key); };

	public InputHelper() {

	}

	// Start is called before the first frame update
	void Start() {
		// Initialize Control Profiles
		_playerOne = GetProfileFor("playerOne");
		_playerTwo = GetProfileFor("playerTwo");

		_playerOne.SetGamepad(1);
		_playerTwo.SetGamepad(2);

		// TODO: Set mode on setup screen, not statically
		_playerOne.mode = InputMode.SQUIRREL;
		_playerTwo.mode = InputMode.TREE;
	}

	// Update is called once per frame
	void Update() {

	}

	/// <summary>
	/// Returns the axis actuation value for the passed Squirrel input. Use like Input.GetAxis.
	/// </summary>
	/// <returns>The axis.</returns>
	/// <param name="input">The input.</param>
	public float GetAxis(SquirrelInput input) {
		return GetSquirrelInput(Input.GetAxis, input, 0.0F);
	}

	/// <summary>
	/// Returns the axis actuation value for the passed Tree input. Use like Input.GetAxis.
	/// </summary>
	/// <returns>The axis.</returns>
	/// <param name="input">The input.</param>
	public float GetAxis(TreeInput input) {
		return GetTreeInput(Input.GetAxis, input, 0.0F);
	}

	/// <summary>
	/// Returns <see langword="true"/> on the tick that the Squirrel input is actuated. Use like Input.GetButtonDown.
	/// </summary>
	/// <returns><c>true</c>, if button first down on this tick, <c>false</c> otherwise.</returns>
	/// <param name="input">Input.</param>
	public bool GetButtonDown(SquirrelInput input) {
		return GetSquirrelInput(Input.GetButtonDown, input, false);
	}

	/// <summary>
	/// Returns <see langword="true"/> on the tick that the Tree input is actuated. Use like Input.GetButtonDown.
	/// </summary>
	/// <returns><c>true</c>, if button first down on this tick, <c>false</c> otherwise.</returns>
	/// <param name="input">Input.</param>
	public bool GetButtonDown(TreeInput input) {
		return GetTreeInput(Input.GetButtonDown, input, false);
	}

	/// <summary>
	/// Returns <see langword="true"/> on the tick that the Squirrel input is released. Use like Input.GetButtonUp.
	/// </summary>
	/// <returns><c>true</c>, if button was first up on this tick, <c>false</c> otherwise.</returns>
	/// <param name="input">Input.</param>
	public bool GetButtonUp(SquirrelInput input) {
		return GetSquirrelInput(Input.GetButtonUp, input, false);
	}

	/// <summary>
	/// Returns <see langword="true"/> on the tick that the Tree input is released. Use like Input.GetButtonUp.
	/// </summary>
	/// <returns><c>true</c>, if button first up on this tick, <c>false</c> otherwise.</returns>
	/// <param name="input">Input.</param>
	public bool GetButtonUp(TreeInput input) {
		return GetTreeInput(Input.GetButtonUp, input, false);
	}

	/// <summary>
	/// Returns <see langword="true"/> every tick that the Squirrel input is actuated. Use like Input.GetButton.
	/// </summary>
	/// <returns><c>true</c>, if button down on this tick, <c>false</c> otherwise.</returns>
	/// <param name="input">Input.</param>
	public bool GetButton(SquirrelInput input) {
		return GetSquirrelInput(Input.GetButton, input, false);
	}

	/// <summary>
	/// Returns <see langword="true"/> every tick that the Tree input is actuated. Use like Input.GetButton.
	/// </summary>
	/// <returns><c>true</c>, if button down on this tick, <c>false</c> otherwise.</returns>
	/// <param name="input">Input.</param>
	public bool GetButton(TreeInput input) {
		return GetTreeInput(Input.GetButton, input, false);
	}

	/// <summary>
	/// Sets the bindings of the passed ControlProfile to their default values.
	/// </summary>
	/// <param name="profile">The ControlProfile to set.</param>
	public void SetDefaults(ControlProfile profile) {
		// Squirrel Controls
		profile.RegisterBinding(SquirrelInput.MOVE_HORIZONTAL, LS_h);
		profile.RegisterBinding(SquirrelInput.MOVE_VERTICAL, LS_v);
		profile.RegisterBinding(SquirrelInput.CAMERA_HORIZONTAL, RS_h);
		profile.RegisterBinding(SquirrelInput.CAMERA_VERTICAL, RS_v);
		profile.RegisterBinding(SquirrelInput.PAUSE, BACK);
		profile.RegisterBinding(SquirrelInput.SWAP, Y);

		profile.RegisterBinding(SquirrelInput.JUMP, A);

		// Tree Controls
		profile.RegisterBinding(TreeInput.MOVE_HORIZTONAL, LS_h);
		profile.RegisterBinding(TreeInput.MOVE_VERTICAL, LS_v);
		profile.RegisterBinding(TreeInput.CAMERA_HORIZONTAL, RS_h);
		profile.RegisterBinding(TreeInput.CAMERA_VERTICAL, RS_v);
		profile.RegisterBinding(TreeInput.PAUSE, BACK);
		profile.RegisterBinding(TreeInput.SWAP, Y);

		profile.RegisterBinding(TreeInput.BRANCH_PLACE, RT);
		profile.RegisterBinding(TreeInput.BRANCH_REMOVE, LT);
		profile.RegisterBinding(TreeInput.SELECT_LEFT, LB);
		profile.RegisterBinding(TreeInput.SELECT_RIGHT, RB);
	}

	/* Internal Methods */

	private ControlProfile GetProfileFor(string profileName) {
		// TODO: Fetch profile corresponding to "profileName" from file. If that fails, load default.
		ControlProfile profile = new ControlProfile("default");
		SetDefaults(profile);
		return profile;
	}

	private T GetTreeInput<T>(Func<string, T> inputFunction, TreeInput key, T defaultValue) {
		return GetInputUsing(BINDING_TREE, inputFunction, key, InputMode.TREE, defaultValue);
	}
	
	private T GetSquirrelInput<T>(Func<string, T> inputFunction, SquirrelInput key, T defaultValue) {
		return GetInputUsing(BINDING_SQUIRREL, inputFunction, key, InputMode.SQUIRREL, defaultValue);
	}

	private U GetInputUsing<T,U>(Func<ControlProfile, T, string> bindingFunction, Func<string, U> inputFunction, T key, InputMode mode, U defaultvalue) {
		if (_playerOne.mode == mode) {
			return inputFunction(bindingFunction(_playerOne, key));
		}
		if (!GameModel.singlePlayer) {
			if (_playerTwo.mode == mode) {
				return inputFunction(bindingFunction(_playerTwo, key));
			}
		}
		return defaultvalue;
	}

	/* ControlProfile Implementation */

	public class ControlProfile {
		// Public Fields
		public InputMode mode;

		// Local Fields
		private readonly string _profileName;
		private uint _gamepadIndex;

		// Local Objects
		private readonly Dictionary<SquirrelInput, string[]> _squirrelBindings;
		private readonly Dictionary<TreeInput, string[]> _treeBindings;

		public ControlProfile(string passedName) {
			_squirrelBindings = new Dictionary<SquirrelInput, string[]>();
			_treeBindings = new Dictionary<TreeInput, string[]>();
			_profileName = passedName;
		}

		public void SetGamepad(uint index) {
			_gamepadIndex = index;
		}

		public void RegisterBinding(TreeInput input, string[] binding) {
			CheckAndPutBinding(_treeBindings, input, binding);
		}

		public void RegisterBinding(SquirrelInput input, string[] binding) {
			CheckAndPutBinding(_squirrelBindings, input, binding);
		}

		public string GetBinding(SquirrelInput input) {
			return CheckAndGetBinding(_squirrelBindings, input, _gamepadIndex);
		}

		public string GetBinding(TreeInput input) {
			return CheckAndGetBinding(_treeBindings, input, _gamepadIndex);
		}

		public string GetName() {
			return _profileName;
		}

		/* Internal Methods */
		private static string CheckAndGetBinding<T>(Dictionary<T, string[]> dictionary, T key, uint index) {
			if (!dictionary.ContainsKey(key)) {
				Debug.LogError(string.Format("No binding provided for {0} control: {1}", key.GetType(), key));
				return "!!!NULL!!!";
			}
			return dictionary[key][index];
		}

		private static void CheckAndPutBinding<T>(Dictionary<T, string[]> dictionary, T key, string[] binding) {
			if (binding == null) {
				Debug.LogError("Null binding passed to control profile!");
			}
			else if (binding.Length < 2) {
				Debug.LogError("Invalid binding passed to control profile!");
			}
			else {
				dictionary.Add(key, binding);
			}
		}
	}
}
