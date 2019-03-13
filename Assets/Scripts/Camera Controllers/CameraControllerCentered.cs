using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RiseExtensions;

public class CameraControllerCentered : MonoBehaviour {

    public Transform cameraTarget;
    public Transform treeTransform;

    public TreeController Tree_Controller;

    [Range(5.0f, 15.0f)]
    public float cameraDistance;

    private Vector3 _camOffset;
    private Vector3 _treeToPlayerAngle;
    private Vector3 _unalteredOffset;
    private Vector3 _unalteredOffsetNew;
    private Vector3 _zoom;

    //-----Subject to change-----
    private PlayerController _playerController;
    //---------------------------

    void Start() {
        _camOffset = transform.position - cameraTarget.position;
        _playerController = cameraTarget.GetComponent<PlayerController>();

        if (tag == "Tree Camera") {
            cameraTarget = Tree_Controller.getReticleTransform();
        }
    }

    void LateUpdate() {

        // Get input directions
        if (tag == "Squirrel Camera") {
            _zoom = new Vector3(0.0f, 0.0f, InputHelper.GetAxis(SquirrelInput.CAMERA_VERTICAL));
        } else if (tag == "Tree Camera") {
            _zoom = new Vector3(0.0f, 0.0f, InputHelper.GetAxis(TreeInput.CAMERA_VERTICAL));
        }

        if (cameraDistance + _zoom.z < 15 &&
            cameraDistance + _zoom.z > 5) {

            cameraDistance += _zoom.z;

        }

        _treeToPlayerAngle = cameraTarget.position - treeTransform.position;

        _unalteredOffset = (_treeToPlayerAngle + treeTransform.position);

        _unalteredOffsetNew = transform.InverseTransformPoint(_unalteredOffset);

        _unalteredOffsetNew.z -= cameraDistance;

        transform.position = Vector3.Slerp(transform.position, transform.TransformPoint(_unalteredOffsetNew), .9f);

        transform.LookAt(new Vector3(treeTransform.position.x, cameraTarget.position.y, treeTransform.position.z));
    
    }
}
