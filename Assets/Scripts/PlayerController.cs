using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public GameObject cameraTarget;

    [Range(0.0f, 10.0f)]
    public float speed, jumpSpeed;

    [Range(0.0f, 40.0f)]
    public float gravity;

    [Range(0.0f, 20.0f)]
    public float distanceToTree;

    private CharacterController _controller;

    private Vector3 _moveDirection = Vector3.zero;

    private Transform targetPosition;

    // Start is called before the first frame update
    void Start() {
        
        _controller = GetComponent<CharacterController>();

        targetPosition = cameraTarget.transform;

        Debug.Log(targetPosition.position);

    }

    // Update is called once per frame
    void Update() {

        if (_controller.isGrounded) {

            // Get input directions
             _moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));

            // Maintains direction after movement stops
            _moveDirection = transform.TransformDirection(_moveDirection);

            if (_moveDirection != Vector3.zero) {

                // Faces player towards movement direction
                transform.forward = _moveDirection;

            }

            if (Input.GetButton("Jump")) {

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

        // face the player towards the center of the target
        transform.LookAt(cameraTarget.transform);

    }
}
