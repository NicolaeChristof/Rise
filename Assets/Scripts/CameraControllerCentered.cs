﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllerCentered : MonoBehaviour {

    public Transform playerTransform;
    public Transform treeTransform;

    public TreeController Tree_Controller;

    private Camera _cam;
    private Vector3 _camOffset;
    private Vector3 _treeToPlayerAngle;
    private Vector3 _unalteredOffset;
    private Vector3 _unalteredOffsetNew;

    public float offsetZ;
    public float offsetY;

    //-----Subject to change-----
    private PlayerController _playerController;
    //---------------------------

    void Start() {
        _cam = GetComponent<Camera>();
        _camOffset = transform.position - playerTransform.position;
        _playerController = playerTransform.GetComponent<PlayerController>();
    }

    void LateUpdate() {
        if (this.name == "Tree Camera") {
            playerTransform = Tree_Controller.getReticleTransform();
        }
        _treeToPlayerAngle = playerTransform.position - treeTransform.position;
        _unalteredOffset = (_treeToPlayerAngle + treeTransform.position);
        _unalteredOffsetNew = transform.InverseTransformPoint(_unalteredOffset);
        _unalteredOffsetNew.z -= offsetZ;
        Vector3 alteredOffsetNew = transform.TransformPoint(_unalteredOffsetNew);
        alteredOffsetNew.y += offsetY;
        transform.position = Vector3.Slerp(transform.position, alteredOffsetNew, .35f);
        transform.LookAt(new Vector3(treeTransform.position.x, playerTransform.position.y, treeTransform.position.z));
    }
}
