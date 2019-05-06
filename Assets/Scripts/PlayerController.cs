using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using RiseExtensions;
using DG.Tweening;

public class PlayerController : RiseBehavior {

    // Public References
    public GameObject playerTarget;

    public GameObject playerModel;

    public GameObject playerModelStanding;

    public Camera squirrelCamera;

    public Camera treeCamera;

    public AudioClip jumpSound;

    public AudioClip doubleJumpSound;

    public AudioClip groundWalk;

    public AudioClip branchWalk;

    public AudioClip idleSound;

    public AudioClip impactSound;

    public Slider treeSlider;

    // Public Fields
    [Range(0.0f, 10.0f)]
    public float speed, jumpSpeed;

    [Range(0.0f, 40.0f)]
    public float gravity;

    [Range(0.0f, 20.0f)]
    public float maxPlayerDistance;

    [Range(0, 2)]
    public int maxJumps;

    [Range(0.001f, 1.0f)]
    public float accelerationSpeed;

    [Range(0.001f, 1.0f)]
    public float deaccelerationSpeed;

    public bool useAcceleration;

    public bool useDeacceleration;

    // Private References
    private CharacterController _controller;

    private AudioSource _source;

    private CapsuleCollider[] _hitBoxes;

    // Private Fields
    private Vector3 _moveDirection = Vector3.zero;

    private Vector3 _externalForce = Vector3.zero;

	private Vector3 _velocity = Vector3.zero;

    private Vector3 _heading;

    private Vector3 _target;

    private float _volume;

    private bool _walkSoundPlaying = false;

    private float _numJumps = 0;

    private Vector3 _originalScale;

    private Vector3 _newScale;

    private bool _playerStunned = false;

    private float _walkSpeed = 0.0f;

    private float _maxWalkSpeed = 1.0f;

    private string _orientation = "center";

    // Start is called before the first frame update
    void Start() {
        
        _controller = GetComponent<CharacterController>();

        _source = GetComponent<AudioSource>();

        _hitBoxes = GetComponents<CapsuleCollider>();

        _originalScale = transform.localScale;

        _newScale = new Vector3(_originalScale.x, _originalScale.y + 0.1f, _originalScale.z);

        // Face player model forwards
        playerModelStanding.transform.localEulerAngles = new Vector3(0.0f, transform.rotation.y + 180.0f, 0.0f);

    }

    // Update is called once per frame
    public override void UpdateTick () {

        if (GameModel.isSquirrel) {

            // Get input directions
            if (!_playerStunned  && !GameModel.endGame) {

                _moveDirection = new Vector3(InputHelper.GetAxis(SquirrelInput.MOVE_HORIZONTAL), _moveDirection.y, InputHelper.GetAxis(SquirrelInput.MOVE_VERTICAL));

                // Accelerate up to full speed
                if (_moveDirection.x > 0.0f) {

                    if (_orientation == "left") {

                        _walkSpeed = 0.0f;

                    }

                    if (_walkSpeed < _maxWalkSpeed) {

                        _walkSpeed += accelerationSpeed;

                    }

                    _orientation = "right";

                } else if (_moveDirection.x < 0.0f) {

                    if (_orientation == "right") {

                        _walkSpeed = 0.0f;

                    }

                    if (_walkSpeed > -_maxWalkSpeed) {

                        _walkSpeed -= accelerationSpeed;

                    }

                    _orientation = "left";

                } else {

                    if (useDeacceleration) {

                        if (_walkSpeed > 0.15f) {

                            _walkSpeed -= deaccelerationSpeed;

                        } else if (_walkSpeed < -0.15f) {

                            _walkSpeed += deaccelerationSpeed;

                        } else {

                            _walkSpeed = 0.0f;

                            _orientation = "center";

                        }

                    } else {

                        _walkSpeed = 0.0f;

                        _orientation = "center";

                    }

                }

                if (useAcceleration) {

                    _moveDirection.x = _walkSpeed;

                }

            } else {

                _moveDirection = new Vector3(0.0f, _moveDirection.y, 0.0f);

            }

            if (!GameModel.endGame) {

                // Disable z axis movement
                _moveDirection.z = 1.0f;

            }

            // Walking sound
            if (_moveDirection.x != 0 && !_walkSoundPlaying && _controller.isGrounded) {

                _walkSoundPlaying = true;

                if (transform.position.y < 3.0f) {

                    _source.clip = groundWalk;

                } else {

                    _source.clip = branchWalk;

                }

                _source.Play();

            }

            if (_walkSoundPlaying && !_controller.isGrounded) {

                _source.Stop();

                _walkSoundPlaying = false;

            }

            // Orient player model
            if (_moveDirection.x < 0) {

                playerModelStanding.SetActive(false);

                playerModel.SetActive(true);

                _hitBoxes[0].enabled = true;

                _hitBoxes[1].enabled = false;

                // Face player model to the left
                playerModel.transform.localEulerAngles = new Vector3(0.0f, transform.rotation.y - 90.0f, 0.0f);

            } else if (_moveDirection.x > 0) {

                playerModelStanding.SetActive(false);

                playerModel.SetActive(true);

                _hitBoxes[0].enabled = true;

                _hitBoxes[1].enabled = false;

                // Face player model to the right
                playerModel.transform.localEulerAngles = new Vector3(0.0f, transform.rotation.y + 90.0f, 0.0f);

            } else {

                playerModel.SetActive(false);

                playerModelStanding.SetActive(true);

                _hitBoxes[0].enabled = false;

                _hitBoxes[1].enabled = true;

                _source.Stop();

                _walkSoundPlaying = false;

            }

            _moveDirection.x *= speed;

            _moveDirection.z *= speed;

            if (InputHelper.GetButtonDown(SquirrelInput.JUMP) && _numJumps < maxJumps && !GameModel.endGame) {

                _numJumps++;

                _volume = Random.Range(GameModel.volLowRange, GameModel.volHighRange);

                _source.PlayOneShot(jumpSound, _volume);

                _moveDirection.y = jumpSpeed;

                transform.DOScale(_newScale, 2.0f)
                    .SetEase(Ease.OutElastic);

                transform.DOScale(_originalScale, 2.0f)
                    .SetEase(Ease.OutElastic);

            }

            if (_controller.isGrounded) {

                _numJumps = 0;

            }

        } else {

            _source.Stop();

            _walkSoundPlaying = false;

            // Drop player if they swapped to tree mode
            _moveDirection = new Vector3(0.0f, _moveDirection.y, 0.0f);

        }

        // Apply gravity
        if (!_controller.isGrounded) {

            _moveDirection.y -= gravity * Time.deltaTime;

        }

        // Apply external force
        if (_externalForce != Vector3.zero) {

            _moveDirection = _externalForce;

            _externalForce = Vector3.zero;

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

        /*
        _currentHeight = transform.position.y - _heightOffset;
        _currentHeightActual =  _currentHeight * _realToVirtualRatio;

        // https://answers.unity.com/questions/50391/how-to-round-a-float-to-2-dp.html
        heightText.text = "Height: " + _currentHeightActual.ToString("F1") + "m";

        treeSlider.value = _currentHeight / _treeHeight;*/

    }

    public override void UpdateAlways () {



    }

    public void addExternalForce (Vector3 force) {

        _externalForce += force;

        // Debug.Log(_externalForce);

    }

    public void stunPlayer (float stunTime) {

        StartCoroutine(stun(stunTime));

    }

    private IEnumerator stun (float stunTime) {

        _playerStunned = true;

        _numJumps = maxJumps;

        _volume = Random.Range(GameModel.volLowRange, GameModel.volHighRange);

        _source.PlayOneShot(impactSound, _volume);

        yield return new WaitForSeconds(stunTime);

        _playerStunned = false;

    }

}
