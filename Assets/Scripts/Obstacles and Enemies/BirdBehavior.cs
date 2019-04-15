using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdBehavior : RiseBehavior {

    // Public References

    // Public Fields
    [Range(-5.0f, 5.0f)]
    public float speed;

    [Range(0.0f, 7.0f)]
    public float maxDistance;

    public float pushForce;

    // Private References
    private PlayerController _playerController;

    private BoxCollider[] _trigger;

    private GameObject _player;

    private GameObject _tree;

    private GameObject _birdModel;

    // Private Fields
    private Vector3 _target;

    private Vector3 _heading;

    private float _distance;

    private Vector3 _pushDirection;

    // Start is called before the first frame update
    void Start() {

        _player = GameObject.FindGameObjectWithTag("Player");

        _tree = GameObject.FindGameObjectWithTag("Tree");

        _birdModel = gameObject.transform.GetChild(0).gameObject;

        _playerController = _player.GetComponent<PlayerController>();

        _trigger = GetComponents<BoxCollider>();

        _target = new Vector3(_tree.transform.position.x,
                              this.transform.position.y,
                              _tree.transform.position.z);

        orientObject();

    }

    // Update is called once per frame
    public override void UpdateTick() {

        // Move the bird
        _heading = transform.position - _target;

        _distance = _heading.magnitude;

        if (_distance > maxDistance) {

            transform.Translate(new Vector3(0.0f, 0.0f, 0.2f));

        } else {

            transform.RotateAround(_target, Vector3.up, speed);

        }

        // face the bird towards the target
        transform.LookAt(_target);

    }

    public override void UpdateAlways() {



    }

    void OnTriggerEnter (Collider collider) {

        if (collider.gameObject.tag.Equals("Player")) {

            _playerController.stunPlayer(0.25f);

            if (speed > 0) {

                _pushDirection = new Vector3(-pushForce, 0.0f, 0.0f);

            } else {

                _pushDirection = new Vector3(pushForce, 0.0f, 0.0f);

            }

            _playerController.addExternalForce(_pushDirection);

            GameModel.squirrelHealth--;
            GameObject Health = GameObject.Find("Health Bar");
            Health.GetComponent<HealthUI>().UpdateHealth();
            Debug.Log(GameModel.squirrelHealth);

            invertDirection();

        }

    }

    void OnTriggerStay (Collider collider) {



    }

    void OnTriggerExit (Collider collider) {



    }

    void OnCollisionEnter (Collision collision) {

        if (!(GetComponent<Collider>().gameObject.tag.Equals("Tree") || collision.gameObject.tag.Equals("Player"))) {

            invertDirection();

        }

    }

    void OnCollisionStay (Collision collision) {



    }

    void OnCollisionExit (Collision collision) {



    }

    private void invertDirection () {

        speed = -speed;

        orientObject();

    }

    private void orientObject () {

        // Orient bird model
        if (speed > 0) {

            // Face bird model to the left
            _birdModel.transform.localEulerAngles = new Vector3(0.0f, transform.rotation.y + 90.0f, 0.0f);

            _trigger[1].center = new Vector3(-0.665f, 0.0f, 0.0f);

        } else if (speed < 0) {

            // Face bird model to the right
            _birdModel.transform.localEulerAngles = new Vector3(0.0f, transform.rotation.y - 90.0f, 0.0f);

            _trigger[1].center = new Vector3(0.665f, 0.0f, 0.0f);

        }

    }
}
