using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHelper : MonoBehaviour {
	// Public Fields
	public enum SquirrelInput {
		MOVE_HORIZONTAL,
		MOVE_VERTICAL,
		SWAP,
		PAUSE,

		JUMP,
		PEEK_LEFT,
		PEEK_RIGHT,
	}

	public enum TreeInput {
		MOVE_HORIZTONAL,
		MOVE_VERTICAL,
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

		SetDefaults(ref _playerOne); // TODO: Remove when configuration file loading is implemented
		SetDefaults(ref _playerTwo); // TODO: Remove when configuration file loading is implemented
	}

	// Update is called once per frame
	void Update() {

	}

	public float GetAxis(SquirrelInput input) {
		return GetSquirrelInput(Input.GetAxis, input, 0.0F);
	}

	public float GetAxis(TreeInput input) {
		return GetTreeInput(Input.GetAxis, input, 0.0F);
	}

	public bool GetButtonDown(SquirrelInput input) {
		return GetSquirrelInput(Input.GetButtonDown, input, false);
	}

	public bool GetButtonDown(TreeInput input) {
		return GetTreeInput(Input.GetButtonDown, input, false);
	}

	public bool GetButtonUp(SquirrelInput input) {
		return GetSquirrelInput(Input.GetButtonUp, input, false);
	}

	public bool GetButtonUp(TreeInput input) {
		return GetTreeInput(Input.GetButtonUp, input, false);
	}

	public void SetDefaults(ref ControlProfile profile) {

	}

	/* Internal Methods */

	private ControlProfile GetProfileFor(string profileName) {
		// TODO: Fetch profiles from configuration file by name.
		return new ControlProfile(profileName);
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
			_treeBindings.Add(input, binding);
		}

		public void RegisterBinding(SquirrelInput input, string[] binding) {
			_squirrelBindings.Add(input, binding);
		}

		public string GetBinding(SquirrelInput input) {
			return _squirrelBindings[input][_gamepadIndex];
		}

		public string GetBinding(TreeInput input) {
			return _treeBindings[input][_gamepadIndex];
		}

		public string GetName() {
			return _profileName;
		}
	}
}
