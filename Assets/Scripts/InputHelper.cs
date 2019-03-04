using System;
using System.Collections.Generic;
using UnityEngine;

namespace RiseExtensions {
	// Controller Bindings
	public enum SquirrelInput {
		MOVE_HORIZONTAL,
		MOVE_VERTICAL,
		CAMERA_HORIZONTAL,
		CAMERA_VERTICAL,
		SWAP,
		PAUSE,

		JUMP,
	};

	public enum TreeInput {
		MOVE_HORIZONTAL,
		MOVE_VERTICAL,
		CAMERA_HORIZONTAL,
		CAMERA_VERTICAL,
		SWAP,
		PAUSE,

		BRANCH_PLACE,
		BRANCH_REMOVE,
		SELECT_LEFT,
		SELECT_RIGHT,
	};

	public enum InputMode {
		SQUIRREL,
		TREE
	};

	public enum InputType {
		XBOX_ONE,
		XBOX_360,
		KEYBOARD
	};

	public static class InputHelper {
		// Public Fields
		public static ControlProfile PlayerOne { get; private set; }
		public static ControlProfile PlayerTwo { get; private set; }

		// Local Fields

		// Local Objects
		private readonly static Func<ControlProfile, TreeInput, string> BINDING_TREE = (profile, key) => { return profile.GetBinding(key); };
		private readonly static Func<ControlProfile, SquirrelInput, string> BINDING_SQUIRREL = (profile, key) => { return profile.GetBinding(key); };

		// Controller Inputs
		public readonly static string[] RT = { "RT_P1", "RT_P2", "360_RT_P1", "360_RT_P2" };
		public readonly static string[] LT = { "LT_P1", "LT_P2", "360_LT_P1", "360_LT_P2" };
		public readonly static string[] RB = { "RB_P1", "RB_P2", "360_RB_P1", "360_RB_P2" };
		public readonly static string[] LB = { "LB_P1", "LB_P2", "360_LB_P1", "360_LB_P2" };
		public readonly static string[] A = { "A_P1", "A_P2", "360_A_P1", "360_A_P2" };
		public readonly static string[] B = { "B_P1", "B_P2", "360_B_P1", "360_B_P2" };
		public readonly static string[] X = { "X_P1", "X_P2", "360_X_P1", "360_X_P2" };
		public readonly static string[] Y = { "Y_P1", "Y_P2", "360_Y_P1", "360_Y_P2" };
		public readonly static string[] DP_h = { "DP_h_P1", "DP_h_P2", "360_DP_h_P1", "360_DP_h_P2" };
		public readonly static string[] DP_v = { "DP_v_P1", "DP_v_P2", "360_DP_v_P1", "360_DP_v_P2" };
		public readonly static string[] RS_h = { "RS_h_P1", "RS_h_P2", "360_RS_h_P1", "360_RS_h_P2" };
		public readonly static string[] RS_v = { "RS_v_P1", "RS_v_P2", "360_RS_v_P1", "360_RS_v_P2" };
		public readonly static string[] LS_h = { "LS_h_P1", "LS_h_P2", "360_LS_h_P1", "360_LS_h_P2" };
		public readonly static string[] LS_v = { "LS_v_P1", "LS_v_P2", "360_LS_v_P1", "360_LS_v_P2" };
		public readonly static string[] START = { "START_P1", "START_P2", "360_START_P1", "360_START_P2" };
		public readonly static string[] BACK = { "BACK_P1", "BACK_P2", "360_BACK_P1", "360_BACK_P2" };

		// Keyboard Inputs
		public readonly static string[] KEY_MOVE_H = { "Keyboard_player_h", "Keyboard_player_h" };
		public readonly static string[] KEY_MOVE_V = { "Keyboard_player_v", "Keyboard_player_v" };
		public readonly static string[] KEY_CAMERA_H = { "Keyboard_camera_h", "Keyboard_camera_h" };
		public readonly static string[] KEY_CAMERA_V = { "Keyboard_camera_v", "Keyboard_camera_v" };
		public readonly static string[] KEY_JUMP = { "Keyboard_jump", "Keyboard_jump" };
		public readonly static string[] KEY_SELECT = { "Keyboard_next", "Keyboard_next" };
		public readonly static string[] KEY_ACTION_1 = { "Keyboard_trigger", "Keyboard_trigger" };
		public readonly static string[] KEY_ACTION_2 = { "Keyboard_break", "Keyboard_break" };
		public readonly static string[] KEY_SWAP = { "Keyboard_swap_player", "Keyboard_swap_player" };
		public readonly static string[] KEY_PAUSE = { "Keyboard_pause", "Keyboard_pause" };

		public static void Initialize() {
			// Initialize Control Profiles
			PlayerOne = GetProfileFor("playerOne");
			PlayerTwo = GetProfileFor("playerTwo");

			PlayerOne.SetGamepad(0);
			PlayerTwo.SetGamepad(1);

			// TODO: Set mode on setup screen, not statically
			PlayerOne.mode = InputMode.SQUIRREL;
			PlayerTwo.mode = InputMode.TREE;
		}

		public static void Check() {
            /*
			if (Pause()) {
				GameModel.paused = !GameModel.paused;
			}*/

			// Handle Swap
			if (Swap()) {
				// TODO: Hold for duration, then swap if confirmed?
				PlayerOne.SwapModes();
				PlayerTwo.SwapModes();
			}

			if (GameModel.debugMode) {
				PrintDebug();
			}
		}

		/// <summary>
		/// Returns whether the InputHelper detected a game pause input this tick.
		/// </summary>
		/// <returns>Whether input for pause was detected.</returns>
		public static bool Pause() {
			return (GetButton(SquirrelInput.PAUSE) || GetButton(TreeInput.PAUSE));
		}

		/// <summary>
		/// Returns whether the InputHelper detected a mode swap input this tick.
		/// </summary>
		/// <returns>Whether input for swap was detected.</returns>
		public static bool Swap() {
			return GameModel.singlePlayer ? GetButtonDown(SquirrelInput.SWAP) || GetButtonDown(TreeInput.SWAP) : GetButton(SquirrelInput.SWAP) && GetButton(TreeInput.SWAP);
		}

		/// <summary>
		/// Returns the axis actuation value for the passed Squirrel input. Use like Input.GetAxis.
		/// </summary>
		/// <returns>The axis.</returns>
		/// <param name="input">The input.</param>
		public static float GetAxis(SquirrelInput input) {
			return GetSquirrelInput(Input.GetAxis, input, 0.0F);
		}

		/// <summary>
		/// Returns the axis actuation value for the passed Tree input. Use like Input.GetAxis.
		/// </summary>
		/// <returns>The axis.</returns>
		/// <param name="input">The input.</param>
		public static float GetAxis(TreeInput input) {
			return GetTreeInput(Input.GetAxis, input, 0.0F);
		}

		/// <summary>
		/// Returns <see langword="true"/> on the tick that the Squirrel input is actuated. Use like Input.GetButtonDown.
		/// </summary>
		/// <returns><c>true</c>, if button first down on this tick, <c>false</c> otherwise.</returns>
		/// <param name="input">Input.</param>
		public static bool GetButtonDown(SquirrelInput input) {
			return GetSquirrelInput(Input.GetButtonDown, input, false);
		}

		/// <summary>
		/// Returns <see langword="true"/> on the tick that the Tree input is actuated. Use like Input.GetButtonDown.
		/// </summary>
		/// <returns><c>true</c>, if button first down on this tick, <c>false</c> otherwise.</returns>
		/// <param name="input">Input.</param>
		public static bool GetButtonDown(TreeInput input) {
			return GetTreeInput(Input.GetButtonDown, input, false);
		}

		/// <summary>
		/// Returns <see langword="true"/> on the tick that the Squirrel input is released. Use like Input.GetButtonUp.
		/// </summary>
		/// <returns><c>true</c>, if button was first up on this tick, <c>false</c> otherwise.</returns>
		/// <param name="input">Input.</param>
		public static bool GetButtonUp(SquirrelInput input) {
			return GetSquirrelInput(Input.GetButtonUp, input, false);
		}

		/// <summary>
		/// Returns <see langword="true"/> on the tick that the Tree input is released. Use like Input.GetButtonUp.
		/// </summary>
		/// <returns><c>true</c>, if button first up on this tick, <c>false</c> otherwise.</returns>
		/// <param name="input">Input.</param>
		public static bool GetButtonUp(TreeInput input) {
			return GetTreeInput(Input.GetButtonUp, input, false);
		}

		/// <summary>
		/// Returns <see langword="true"/> every tick that the Squirrel input is actuated. Use like Input.GetButton.
		/// </summary>
		/// <returns><c>true</c>, if button down on this tick, <c>false</c> otherwise.</returns>
		/// <param name="input">Input.</param>
		public static bool GetButton(SquirrelInput input) {
			return GetSquirrelInput(Input.GetButton, input, false);
		}

		/// <summary>
		/// Returns <see langword="true"/> every tick that the Tree input is actuated. Use like Input.GetButton.
		/// </summary>
		/// <returns><c>true</c>, if button down on this tick, <c>false</c> otherwise.</returns>
		/// <param name="input">Input.</param>
		public static bool GetButton(TreeInput input) {
			return GetTreeInput(Input.GetButton, input, false);
		}

		/// <summary>
		/// Sets the bindings of the passed ControlProfile to their default values.
		/// </summary>
		/// <param name="profile">The ControlProfile to set.</param>
		public static void SetDefaults(ControlProfile profile) {
			// Squirrel Controls
			profile.RegisterBinding(SquirrelInput.MOVE_HORIZONTAL, LS_h);
			profile.RegisterBinding(SquirrelInput.MOVE_VERTICAL, LS_v);
			profile.RegisterBinding(SquirrelInput.CAMERA_HORIZONTAL, RS_h);
			profile.RegisterBinding(SquirrelInput.CAMERA_VERTICAL, RS_v);
			profile.RegisterBinding(SquirrelInput.PAUSE, START);
			profile.RegisterBinding(SquirrelInput.SWAP, Y);

			profile.RegisterBinding(SquirrelInput.JUMP, A);

			// Tree Controls
			profile.RegisterBinding(TreeInput.MOVE_HORIZONTAL, LS_h);
			profile.RegisterBinding(TreeInput.MOVE_VERTICAL, LS_v);
			profile.RegisterBinding(TreeInput.CAMERA_HORIZONTAL, RS_h);
			profile.RegisterBinding(TreeInput.CAMERA_VERTICAL, RS_v);
			profile.RegisterBinding(TreeInput.PAUSE, BACK);
			profile.RegisterBinding(TreeInput.SWAP, Y);

			profile.RegisterBinding(TreeInput.BRANCH_PLACE, A);
			profile.RegisterBinding(TreeInput.BRANCH_REMOVE, B);
			// profile.RegisterBinding(TreeInput.SELECT_LEFT, LB); // temporarily trying out face button only interface
			profile.RegisterBinding(TreeInput.SELECT_RIGHT, X);
		}

		public static void SetKeyboard(ControlProfile profile) {
			// Set mode flag
			profile.type = InputType.KEYBOARD;

			// Squirrel Controls
			profile.RegisterBinding(SquirrelInput.MOVE_HORIZONTAL, KEY_MOVE_H);
			profile.RegisterBinding(SquirrelInput.MOVE_VERTICAL, KEY_MOVE_V);
			profile.RegisterBinding(SquirrelInput.CAMERA_HORIZONTAL, KEY_CAMERA_H);
			profile.RegisterBinding(SquirrelInput.CAMERA_VERTICAL, KEY_CAMERA_V);
			profile.RegisterBinding(SquirrelInput.PAUSE, KEY_PAUSE);
			profile.RegisterBinding(SquirrelInput.SWAP, KEY_SWAP);
			profile.RegisterBinding(SquirrelInput.JUMP, KEY_JUMP);

			// Tree Controls
			profile.RegisterBinding(TreeInput.MOVE_HORIZONTAL, KEY_MOVE_H);
			profile.RegisterBinding(TreeInput.MOVE_VERTICAL, KEY_MOVE_V);
			profile.RegisterBinding(TreeInput.CAMERA_HORIZONTAL, KEY_CAMERA_H);
			profile.RegisterBinding(TreeInput.CAMERA_VERTICAL, KEY_CAMERA_V);
			profile.RegisterBinding(TreeInput.PAUSE, KEY_PAUSE);
			profile.RegisterBinding(TreeInput.SWAP, KEY_SWAP);

			profile.RegisterBinding(TreeInput.BRANCH_PLACE, KEY_ACTION_1);
			profile.RegisterBinding(TreeInput.BRANCH_REMOVE, KEY_ACTION_2);
			// profile.RegisterBinding(TreeInput.SELECT_LEFT, LB); // temporarily trying out face button only interface
			profile.RegisterBinding(TreeInput.SELECT_RIGHT, KEY_SELECT);
		}

		/* Internal Methods */

		private static ControlProfile GetProfileFor(string profileName) {
			// TODO: Fetch profile corresponding to "profileName" from file. If that fails, load default.
			ControlProfile profile = new ControlProfile("default", InputType.XBOX_ONE);
			SetDefaults(profile);
			return profile;
		}

		private static T GetTreeInput<T>(Func<string, T> inputFunction, TreeInput key, T defaultValue) {
			return GetInputUsing(BINDING_TREE, inputFunction, key, InputMode.TREE, defaultValue);
		}

		private static T GetSquirrelInput<T>(Func<string, T> inputFunction, SquirrelInput key, T defaultValue) {
			return GetInputUsing(BINDING_SQUIRREL, inputFunction, key, InputMode.SQUIRREL, defaultValue);
		}

		private static U GetInputUsing<T, U>(Func<ControlProfile, T, string> bindingFunction, Func<string, U> inputFunction, T key, InputMode mode, U defaultvalue) {
			if (PlayerOne.mode == mode) {
				return inputFunction(bindingFunction(PlayerOne, key));
			}
			if (!GameModel.singlePlayer) {
				if (PlayerTwo.mode == mode) {
					return inputFunction(bindingFunction(PlayerTwo, key));
				}
			}
			return defaultvalue;
		}

		private static void PrintDebug() {
			foreach (SquirrelInput input in (SquirrelInput[])Enum.GetValues(typeof(SquirrelInput))) {
				if (GetButton(input) || Math.Abs(GetAxis(input)) > 0.1F) {
					Debug.Log("SQUIRREL: " + input);
				}
			}
			foreach (TreeInput input in (TreeInput[])Enum.GetValues(typeof(SquirrelInput))) {
				if (GetButton(input) || Math.Abs(GetAxis(input)) > 0.1F) {
					Debug.Log("TREE: " + input);
				}
			}
		}

		/* ControlProfile Implementation */

		public class ControlProfile {
			// Public Fields
			public InputMode mode;
			public InputType type;

			// Local Fields
			private readonly string _profileName;
			private uint _gamepadIndex;

			// Local Objects
			private readonly Dictionary<SquirrelInput, string[]> _squirrelBindings;
			private readonly Dictionary<TreeInput, string[]> _treeBindings;

			public ControlProfile(string passedName, InputType inputType) {
				_squirrelBindings = new Dictionary<SquirrelInput, string[]>();
				_treeBindings = new Dictionary<TreeInput, string[]>();
				_profileName = passedName;
				type = inputType;
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
				return CheckAndGetBinding(_squirrelBindings, input, GetIndex());
			}

			public string GetBinding(TreeInput input) {
				return CheckAndGetBinding(_treeBindings, input, GetIndex());
			}

			public void SwapModes() {
				if (mode == InputMode.SQUIRREL) {
					mode = InputMode.TREE;
				}
				else if (mode == InputMode.TREE) {
					mode = InputMode.SQUIRREL;
				}
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

			private uint GetIndex() {
				switch (type) {
					case InputType.XBOX_360:
						return _gamepadIndex + 2;
					default:
						return _gamepadIndex;
				}
			}

			private static void CheckAndPutBinding<T>(Dictionary<T, string[]> dictionary, T key, string[] binding) {
				if (binding == null) {
					Debug.LogError("Null binding passed to control profile!");
				}
				else if (binding.Length < 2) {
					Debug.LogError("Invalid binding passed to control profile!");
				}
				else {
					dictionary.Remove(key);
					dictionary.Add(key, binding);
				}
			}
		}
	}

}