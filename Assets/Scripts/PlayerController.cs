using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public GameObject playerTarget;

    public GameObject playerModel;

    [Range(0.0f, 10.0f)]
    public float speed, jumpSpeed;

    [Range(0.0f, 40.0f)]
    public float gravity;

    [Range(0.0f, 20.0f)]
    public float distanceToTree;

    private CharacterController _controller;

    private Vector3 _moveDirection = Vector3.zero;

    private Vector3 _target;

    // Start is called before the first frame update
    void Start() {
        
        _controller = GetComponent<CharacterController>();

    }

    // Update is called once per frame
    void Update() {

        if (_controller.isGrounded) {

            // Get input directions
            _moveDirection = new Vector3(Input.GetAxis("LS_h"), 0.0f, Input.GetAxis("LS_v"));

            if (Input.GetAxis("LS_h") < 0) {

                // Face player model to the left
                playerModel.transform.localEulerAngles = new Vector3(0.0f, transform.rotation.y - 90.0f, 0.0f);

            } else if (Input.GetAxis("LS_h") > 0) {

                // Face player model to the right
                playerModel.transform.localEulerAngles = new Vector3(0.0f, transform.rotation.y + 90.0f, 0.0f);

            } else {

                // Face player model outwards towards camera
                playerModel.transform.localEulerAngles = new Vector3(0.0f, transform.rotation.y + 180.0f, 0.0f);

            }

            // Maintains direction after movement stops
            _moveDirection = transform.TransformDirection(_moveDirection);

            if (_moveDirection != Vector3.zero) {

                // Faces player towards movement direction
                transform.forward = _moveDirection;

            }

            if (Input.GetButton("A")) {

                _moveDirection.y = jumpSpeed;

            }

            _moveDirection = _moveDirection * speed;

        }

        if (!_controller.isGrounded) {

            // Apply gravity
            _moveDirection.y = _moveDirection.y - (gravity * Time.deltaTime);

        }

        // Move the Controller
        _controller.Move(_moveDirection * Time.deltaTime);

        _target = new Vector3(playerTarget.transform.position.x,
                              this.transform.position.y,
                              playerTarget.transform.position.z);

        // face the player towards the target
        transform.LookAt(_target);

    }
}
