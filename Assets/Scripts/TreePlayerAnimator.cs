using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreePlayerAnimator : MonoBehaviour {

    // Public References

    // Public Fields

    // Private References
    private Transform _tree;

    // Private Fields

    // Start is called before the first frame update
    void Start() {

        _tree = transform.GetChild(0);

    }

    // Update is called once per frame
    void Update() {

        _tree.Rotate(0.0f, 0.0f, 5.0f);
        
    }
}
