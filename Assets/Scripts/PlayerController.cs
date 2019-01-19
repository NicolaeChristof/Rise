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

    [Range(0.0f, 10.0f)]
    public float maxPlayerDistance;

    private CharacterController _controller;

    private Vector3 _moveDirection = Vector3.zero;

    private Vector3 _target;

    private Vector3 heading;

    private float distance;

    private string HORIZONTAL_INPUT;

    private string VERTICAL_INPUT;

    private string JUMP;

    // Start is called before the first frame update
    void Start() {
        
        _controller = GetComponent<CharacterController>();

        if (GameModel.inputGamePad) {

            HORIZONTAL_INPUT = "LS_h";

            VERTICAL_INPUT = "LS_v";

            JUMP = "A";

        } else {

            HORIZONTAL_INPUT = "Keyboard_player_h";

            VERTICAL_INPUT = "Keyboard_player_v";

            JUMP = "Keyboard_jump";

        }

    }

    // Update is called once per frame
    void Update() {

        // Get input directions
        _moveDirection = new Vector3(Input.GetAxis(HORIZONTAL_INPUT), _moveDirection.y, Input.GetAxis(VERTICAL_INPUT));

        // Orient player model
        if (_moveDirection.x < 0) {

            // Face player model to the left
            playerModel.transform.localEulerAngles = new Vector3(0.0f, transform.rotation.y - 90.0f, 0.0f);

        } else if (_moveDirection.x > 0) {

            // Face player model to the right
            playerModel.transform.localEulerAngles = new Vector3(0.0f, transform.rotation.y + 90.0f, 0.0f);

        } else {

            // Face player model outwards towards camera
            playerModel.transform.localEulerAngles = new Vector3(0.0f, transform.rotation.y + 180.0f, 0.0f);

        }

        _moveDirection.x *= speed;

        _moveDirection.z *= speed;

        if (Input.GetButton(JUMP) && _controller.isGrounded) {

            _moveDirection.y = jumpSpeed;

        }

        // Apply gravity
        _moveDirection.y -= gravity * Time.deltaTime;

        heading = this.transform.position - playerTarget.transform.position;

        distance = heading.magnitude;

        // Debug.Log(distance);

        if (distance > maxPlayerDistance) {

            _moveDirection.z = 6;

        }

        // Maintains direction after movement stops
        _moveDirection = transform.TransformDirection(_moveDirection);

        if (_moveDirection != Vector3.zero) {

            // Faces player towards movement direction
            transform.forward = _moveDirection;

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
