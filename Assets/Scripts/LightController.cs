using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour {

    public Light dayLight, nightLight;

    [Range(0.0f, 360.0f)]
    public float timeOfDay;

    [Range(0.0f, 2.0f)]
    public float dayNightSpeed, rotationSpeed;

    public bool cycle;

    private Transform sunPosition;

    private float offsetX, offsetY;

    // Use this for initialization
    void Start () {

        sunPosition = dayLight.GetComponent<Transform>();

        offsetX = sunPosition.eulerAngles.x;
        offsetY = sunPosition.eulerAngles.y;
        
    }

    // Update is called once per frame
    void Update () {

        if (cycle) {

            offsetX += dayNightSpeed;
            offsetY += rotationSpeed;

            dayLight.transform.eulerAngles = new Vector3(offsetX, offsetY, 0);
            nightLight.transform.eulerAngles = new Vector3(offsetX + 180, offsetY, 0);

        } else {

            offsetX = timeOfDay;

            dayLight.transform.eulerAngles = new Vector3(offsetX, offsetY, 0);
            nightLight.transform.eulerAngles = new Vector3(offsetX + 180, offsetY, 0);

        }
    }
}
