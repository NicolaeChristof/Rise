using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeController : MonoBehaviour
{
    // Local References
    private GameObject _tree;
    private GameObject _reticle;

    // Local Constants
    private const string MOVE_VERTICAL = "DPAD_v";
    private const string MOVE_LATERAL = "DPAD_h";
    private const string GROW = "LB";

    private const float SPEED = 2.15F;
    private const float EPSILON = 0.01F;

    void Start()
    {
        // Establish local references
        _tree = GameObject.Find("Tree");
        _reticle = GameObject.Find("Tree Reticle");
    }

    void Update()
    {

    }
}
