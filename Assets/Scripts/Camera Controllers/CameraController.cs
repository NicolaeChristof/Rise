using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using RiseExtensions;

public class CameraController : RiseBehavior {

    // Public References
    public GameObject cameraTarget;

    // Public Fields
    [Range(0.0f, 10.0f)]
    public float cameraSpeed_h;

    [Range(0.0f, 1.0f)]
    public float cameraSpeed_v;

    // Private Reference

    // Private Fields
    private Vector3 _moveDirection = Vector3.zero;

    private PostProcessProfile _postProcessProfile;
    private DepthOfField depthOfField;

    // Start is called before the first frame update
    void Start() {

        Vector3 target = new Vector3(cameraTarget.transform.position.x,
                                     this.transform.position.y,
                                     cameraTarget.transform.position.z);

        // face the camera towards the center of the target
        transform.LookAt(target);

        _postProcessProfile = GetComponent<PostProcessVolume>().profile;
        _postProcessProfile.TryGetSettings(out depthOfField);

    }

    // Update is called once per frame
    public override void UpdateTick() {

        if (GameModel.singlePlayer) {

            if (this.tag == "Squirrel Camera" && GameModel.isSquirrel) {

                // Get input directions
                _moveDirection = new Vector3(InputHelper.GetAxis(SquirrelInput.CAMERA_HORIZONTAL), 0.0f, 0.0f);

            } else if (this.tag == "Tree Camera" && !GameModel.isSquirrel) {

                // Get input directions
                _moveDirection = new Vector3(InputHelper.GetAxis(TreeInput.CAMERA_HORIZONTAL), 0.0f, 0.0f);
            
            }

        } else {

            if (this.tag == "Squirrel Camera") {

                // Get input directions
                _moveDirection = new Vector3(InputHelper.GetAxis(SquirrelInput.CAMERA_HORIZONTAL), 0.0f, 0.0f);


            } else if (this.tag == "Tree Camera") {

                // Get input directions
                _moveDirection = new Vector3(InputHelper.GetAxis(TreeInput.CAMERA_HORIZONTAL), 0.0f, 0.0f);

            }

        }

        // Handle vertical camera movement
        transform.Translate(0.0f, _moveDirection.y * cameraSpeed_v, 0.0f);

        Vector3 target = new Vector3(cameraTarget.transform.position.x,
                                     this.transform.position.y,
                                     cameraTarget.transform.position.z);

        // rotate the camera around the center of the target
        transform.RotateAround(target, Vector3.up, _moveDirection.x * cameraSpeed_h);

        _postProcessProfile.TryGetSettings(out depthOfField);
        depthOfField.focusDistance.value = Mathf.Abs(transform.InverseTransformDirection(transform.position - target).z);

    }

    public override void UpdateAlways() {



    }
}
