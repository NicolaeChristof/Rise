using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdBehavior : MonoBehaviour {

    // Public References
    public GameObject birdTarget;

    public GameObject birdModel;

    public GameObject player;

    // Public Fields
    [Range(-10.0f, 10.0f)]
    public float speed;

    // Private References
    private PlayerController _playerController;

    // Private Fields
    private Vector3 _moveDirection = Vector3.zero;

    private Vector3 _target;

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

            _moveDirection = new Vector3(speed, 0.0f, 1.0f);

            // Orient bird model
            if (speed < 0) {

                // Face bird model to the left
                birdModel.transform.localEulerAngles = new Vector3(0.0f, transform.rotation.y - 90.0f, 0.0f);

            } else if (speed > 0) {

                // Face bird model to the right
                birdModel.transform.localEulerAngles = new Vector3(0.0f, transform.rotation.y + 90.0f, 0.0f);

            }

            // Move the bird
            transform.Translate(_moveDirection);
            // transform.RotateAround(_target, Vector3.up, speed);

            // face the bird towards the target
            transform.LookAt(_target);

        }

    }

    void OnCollisionEnter (Collision collision) {

        if (collision.gameObject.name == "Player") {

            // _playerController.addExternalForce(new Vector3(-(speed * 15), 10.0f, 0.0f));

        }

        speed = -speed;

    }
}
