using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    // Public References
    public GameObject playerTarget;

    public GameObject playerModel;

    public Camera squirrelCamera;

    public Camera treeCamera;

    public AudioClip jumpSound;

    public AudioClip walkSound;

    public Slider treeSlider;

    // Public Fields
    [Range(0.0f, 10.0f)]
    public float speed, jumpSpeed;

    [Range(0.0f, 40.0f)]
    public float gravity;

    [Range(0.0f, 20.0f)]
    public float maxPlayerDistance;

    // The height of a real-life squirrel
    public float realSquirrelHeight;

    public Text heightText;

    // Private References
    private CharacterController _controller;

    private AudioSource _source;

    // Private Fields
    private Vector3 _moveDirection = Vector3.zero;

    private Vector3 _externalForce = Vector3.zero;

	private Vector3 _velocity = Vector3.zero;

    private Vector3 _heading;

    private Vector3 _target;

    private float _volume;

    private bool _moving = false;

    private float _realToVirtualRatio;

    private float _heightOffset = 2.25f;

    private float _currentHeight;

    private float _currentHeightActual;

    private float _treeHeight;

    // Start is called before the first frame update
    void Start() {
        
        _controller = GetComponent<CharacterController>();

        _source = GetComponent<AudioSource>();

        _realToVirtualRatio = realSquirrelHeight / _controller.height;

        _treeHeight = playerTarget.transform.localScale.y + playerTarget.transform.position.y - _heightOffset;

    }

    // Update is called once per frame
    void Update() {

        if (!GameModel.paused) {

            if (GameModel.isSquirrel) {

                // Get input directions
                _moveDirection = new Vector3(Input.GetAxis(GameModel.HORIZONTAL_SQUIRREL_INPUT), _moveDirection.y, Input.GetAxis(GameModel.VERTICAL_SQUIRREL_INPUT));

                // Disable z axis movement
                _moveDirection.z = 1.0f;

                // Walking sound
                if (_moveDirection.x != 0 && !_moving) {

                    _moving = true;

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

                    // _source.Stop(); // temporarily disabled. trying to figure out how to stop only one audioclip at a time without having multiple audio sources.

                    _moving = false;

                }

                _moveDirection.x *= speed;

                _moveDirection.z *= speed;

                if (Input.GetButton(GameModel.JUMP) && _controller.isGrounded) {

                    _volume = Random.Range(GameModel.volLowRange, GameModel.volHighRange);

                    _source.PlayOneShot(jumpSound, _volume);

                    _moveDirection.y = jumpSpeed;

                }

            } else {

                // Drop player if they swapped to tree mode
                _moveDirection = new Vector3(0.0f, _moveDirection.y, 0.0f);

            }

            // Apply gravity
            _moveDirection.y -= gravity * Time.deltaTime;

            // Apply external force
            _moveDirection += _externalForce;

            _externalForce = new Vector3(0.0f, 0.0f, 0.0f);

            // Maintains direction after movement stops
            _moveDirection = transform.TransformDirection(_moveDirection);

            if (_moveDirection != Vector3.zero) {

                // Faces player towards movement direction
                transform.forward = _moveDirection;

            }

			// Move the Controller
			ApplyVelocity();
			ApplyMotion(_moveDirection);

            _target = new Vector3(playerTarget.transform.position.x,
                                  this.transform.position.y,
                                  playerTarget.transform.position.z);

            // face the player towards the target
            transform.LookAt(_target);

        }

        _currentHeight = transform.position.y - _heightOffset;
        _currentHeightActual =  _currentHeight * _realToVirtualRatio;

        // https://answers.unity.com/questions/50391/how-to-round-a-float-to-2-dp.html
        heightText.text = "Height: " + _currentHeightActual.ToString("F1") + "m";

        treeSlider.value = _currentHeight / _treeHeight;

    }

    public void addExternalForce (Vector3 force) {

        _externalForce += force;

        Debug.Log(_externalForce);

    }

	public Vector3 GetMoveDirection() {
		return _moveDirection;
	}

	public void SetVelocity(Vector3 passedVelocityVector) {
		_velocity = passedVelocityVector;
	}

	private void ApplyVelocity() {
		// Apply velocity to move direction vector
		_moveDirection = _moveDirection + (_velocity * Time.deltaTime);

		// Apply velocity falloff TODO: Fix me. This is currently tied to gravity.
		float falloff = (gravity * Time.deltaTime);
		_velocity.x = Mathf.Clamp(_velocity.x - (falloff * Mathf.Sign(_velocity.x)), 0, float.MaxValue);
		_velocity.y = Mathf.Clamp(_velocity.y - (falloff * Mathf.Sign(_velocity.y)), 0, float.MaxValue);
		_velocity.z = Mathf.Clamp(_velocity.z - (falloff * Mathf.Sign(_velocity.z)), 0, float.MaxValue);
	}

	private void ApplyMotion(Vector3 passedMotionVector) {
		_controller.Move(passedMotionVector * Time.deltaTime);
	}

}
