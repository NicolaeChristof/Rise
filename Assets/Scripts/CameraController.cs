using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    // Public References
    public GameObject player;

    public GameObject cameraTarget;

    // Public Fields
    [Range(0.0f, 10.0f)]
    public float cameraSpeed_h;

    [Range(0.0f, 1.0f)]
    public float cameraSpeed_v;

    [Range(0.0f, 10.0f)]
    public float groundHeight;

    [Range(1.0f, 10.0f)]
    public float playerDistance;

    // Private Fields
    private Vector3 _moveDirection = Vector3.zero;

    // Start is called before the first frame update
    void Start() {

        Vector3 target = new Vector3(cameraTarget.transform.position.x,
                                     this.transform.position.y,
                                     cameraTarget.transform.position.z);

        // face the camera towards the center of the target
        transform.LookAt(target);

    }

    // Update is called once per frame
    void Update() {

        // Get input directions
        if (GameModel.inputGamePad) {

            _moveDirection = new Vector3(Input.GetAxis("RS_h"), Input.GetAxis("RS_v"), 0.0f);

        } else {

            _moveDirection = new Vector3(Input.GetAxis("Keyboard_camera_h"), Input.GetAxis("Keyboard_camera_v"), 0.0f);

        }

        if (this.name == "Squirrel Camera") {

            // Keep camera close to player and out of the ground
            if ((transform.position.y + _moveDirection.y > groundHeight) &&
                (transform.position.y + _moveDirection.y > player.transform.position.y - playerDistance) &&
                (transform.position.y + _moveDirection.y < player.transform.position.y + playerDistance)) {

                // Handle vertical camera movement
                transform.Translate(0.0f, _moveDirection.y * cameraSpeed_v, 0.0f);

            } else if ((transform.position.y > player.transform.position.y + playerDistance) &&
                       (_moveDirection.y < 0)) {

                // Handle vertical camera movement
                transform.Translate(0.0f, _moveDirection.y * cameraSpeed_v, 0.0f);

            } else if ((transform.position.y < player.transform.position.y - playerDistance) &&
                       (_moveDirection.y > 0)) {

                // Handle vertical camera movement
                transform.Translate(0.0f, _moveDirection.y * cameraSpeed_v, 0.0f);

            }

        } else {

            // Handle vertical camera movement
            transform.Translate(0.0f, _moveDirection.y * cameraSpeed_v, 0.0f);

        }

        Vector3 target = new Vector3(cameraTarget.transform.position.x,
                                     this.transform.position.y,
                                     cameraTarget.transform.position.z);

        // rotate the camera around the center of the target
        transform.RotateAround(target, Vector3.up, _moveDirection.x * cameraSpeed_h);

    }
}
