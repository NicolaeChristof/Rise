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

	public InputHelper() {

	}

	// Start is called before the first frame update
	void Start() {

	}

	// Update is called once per frame
	void Update() {

	}

	/* Internal Methods */

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
