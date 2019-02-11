using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdBehavior : MonoBehaviour {

    // Public References
    public GameObject birdTarget;

    public GameObject birdModel;

    public GameObject player;

    // Public Fields
    [Range(-5.0f, 5.0f)]
    public float speed;

    [Range(0.0f, 7.0f)]
    public float maxDistance;

    // Private References
    private PlayerController _playerController;

    // Private Fields
    private Vector3 _target;

    private Vector3 _heading;

    private float _distance;

    // Start is called before the first frame update
    void Start() {

        _playerController = player.GetComponent<PlayerController>();

        _target = new Vector3(birdTarget.transform.position.x,
                              this.transform.position.y,
                              birdTarget.transform.position.z);

    }

    // Update is called once per frame
    void Update() {

        if (!GameModel.paused) {

            // Orient bird model
            if (speed > 0) {

                // Face bird model to the left
                birdModel.transform.localEulerAngles = new Vector3(0.0f, transform.rotation.y - 90.0f, 0.0f);

            } else if (speed < 0) {

                // Face bird model to the right
                birdModel.transform.localEulerAngles = new Vector3(0.0f, transform.rotation.y + 90.0f, 0.0f);

            }

            // Move the bird
            transform.RotateAround(_target, Vector3.up, speed);

            _heading = this.transform.position - _target;

            _distance = _heading.magnitude;

            if (_distance > maxDistance) {

                transform.Translate(new Vector3(0.0f, 0.0f, 0.2f));

            }

            // face the bird towards the target
            transform.LookAt(_target);

        }

    }

    void OnCollisionEnter (Collision collision) {

        if (collision.gameObject.name != "Tree") {

            if (collision.gameObject.name == "Player") {

                Debug.Log("Player Detected! (Bird)");

                // Push the player

            }

            speed = -speed;

        }

    }
}
