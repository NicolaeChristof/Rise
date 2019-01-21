using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    // Public References
    public GameObject playerTarget;

    public GameObject playerModel;

    public AudioClip jumpSound;

    public AudioClip walkSound;

    // Public Fields
    [Range(0.0f, 10.0f)]
    public float speed, jumpSpeed;

    [Range(0.0f, 40.0f)]
    public float gravity;

    [Range(0.0f, 10.0f)]
    public float maxPlayerDistance;

    // Private References
    private CharacterController _controller;

    private AudioSource _source;

    // Private Fields
    private Vector3 _moveDirection = Vector3.zero;

    private Vector3 _target;

    private Vector3 _heading;

    private float _distance;

    private float _volume;

    // Inputs
    private string HORIZONTAL_INPUT;

    private string VERTICAL_INPUT;

    private string JUMP;

    // Start is called before the first frame update
    void Start() {
        
        _controller = GetComponent<CharacterController>();

        _source = GetComponent<AudioSource>();

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

        if (_moveDirection.x != 0) {

            _volume = Random.Range(GameModel.volLowRange, GameModel.volHighRange);

            _source.PlayOneShot(walkSound, _volume);

        }

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

            _volume = Random.Range(GameModel.volLowRange, GameModel.volHighRange);

            _source.PlayOneShot(jumpSound, _volume);

            _moveDirection.y = jumpSpeed;

        }

        // Apply gravity
        _moveDirection.y -= gravity * Time.deltaTime;

        _heading = this.transform.position - playerTarget.transform.position;

        _distance = _heading.magnitude;

        // Debug.Log(distance);

        if (_distance > maxPlayerDistance) {

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
