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

	public InputHelper() {

	}

	// Start is called before the first frame update
	void Start() {

	}

	// Update is called once per frame
	void Update() {

	}
}
