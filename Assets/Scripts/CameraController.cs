using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    // Public References
    public GameObject cameraTarget;

    // Public Fields
    [Range(0.0f, 10.0f)]
    public float cameraSpeed_h;

    [Range(0.0f, 1.0f)]
    public float cameraSpeed_v;

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

        if (!GameModel.paused) {

            if (GameModel.singlePlayer) {

                if (this.name == "Squirrel Camera" && GameModel.isSquirrel) {

                    // Get input directions
                    _moveDirection = new Vector3(Input.GetAxis(GameModel.HORIZONTAL_SQUIRREL_CAMERA_INPUT), Input.GetAxis(GameModel.VERTICAL_SQUIRREL_CAMERA_INPUT), 0.0f);

                } else if (this.name == "Tree Camera" && !GameModel.isSquirrel) {

                    // Get input directions
                    _moveDirection = new Vector3(Input.GetAxis(GameModel.HORIZONTAL_TREE_CAMERA_INPUT), Input.GetAxis(GameModel.VERTICAL_TREE_CAMERA_INPUT), 0.0f);
                }

            } else {

                if (this.name == "Squirrel Camera") {

                    // Get input directions
                    _moveDirection = new Vector3(Input.GetAxis(GameModel.HORIZONTAL_SQUIRREL_CAMERA_INPUT), Input.GetAxis(GameModel.VERTICAL_SQUIRREL_CAMERA_INPUT), 0.0f);

                } else if (this.name == "Tree Camera") {

                    // Get input directions
                    _moveDirection = new Vector3(Input.GetAxis(GameModel.HORIZONTAL_TREE_CAMERA_INPUT), Input.GetAxis(GameModel.VERTICAL_TREE_CAMERA_INPUT), 0.0f);

                }

            }

             // Handle vertical camera movement
            transform.Translate(0.0f, _moveDirection.y * cameraSpeed_v, 0.0f);

            Vector3 target = new Vector3(cameraTarget.transform.position.x,
                                         this.transform.position.y,
                                         cameraTarget.transform.position.z);

            // rotate the camera around the center of the target
            transform.RotateAround(target, Vector3.up, _moveDirection.x * cameraSpeed_h);

        }
    }
}
