using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SapController : MonoBehaviour {

    public float maxSap;

    public float sapDrainRate;

    [HideInInspector]
    public float currentSap;

    public Slider sapSlider;

    private string DRAIN;

    void Start() {

        if (GameModel.inputGamePad) {

            DRAIN = "RT";

        } else {

            DRAIN = "Keyboard_drain";

        }

        currentSap = maxSap;

        sapSlider.value = currentSap / maxSap;

    }

    private void Update() {

        if (Input.GetAxis(DRAIN) > 0f) {

            ChangeCurrentSap(sapDrainRate);

        }

    }

    private void OnTriggerEnter(Collider other) {

        if (other.tag == "Sap") {

            ChangeCurrentSap(25f);

            other.gameObject.SetActive(false);

            Destroy(other.gameObject);

        }

    }

    private void ChangeCurrentSap(float changeValue) {

        if (changeValue >= 0f) {

            if (currentSap < (maxSap - changeValue)) {

                currentSap += changeValue;

            } else {

                currentSap = maxSap;

            }

        } else {

            if (currentSap > Mathf.Abs(changeValue)) {

                currentSap += changeValue;

            } else {

                currentSap = 0f;

            }

        }

        sapSlider.value = currentSap / maxSap;

    }
}
