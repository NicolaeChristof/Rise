using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightUIInfo : MonoBehaviour {

    public float realSquirrelHeight;
    public float heightOffset;

    [HideInInspector]
    public float treeHeight;

    [HideInInspector]
    public float currentHeight;

    [HideInInspector]
    public float currentHeightInMeters;

    public GameObject tree;

    private CharacterController _charController;

    private float _realToVirtualRatio;

    // Start is called before the first frame update
    void Start() {
        _charController = GetComponent<CharacterController>();

        _realToVirtualRatio = realSquirrelHeight / _charController.height;

        treeHeight = tree.transform.localScale.y + tree.transform.position.y - heightOffset;
    }

    // Update is called once per frame
    void Update() {
        currentHeight = transform.position.y - heightOffset;
        currentHeightInMeters = currentHeight * _realToVirtualRatio;
    }
}
