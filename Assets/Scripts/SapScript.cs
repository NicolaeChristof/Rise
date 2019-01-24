using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SapScript : MonoBehaviour 
{
    public float sapRefillPercentage;

    private ForestController _controller;

    private void Start() 
    {
        _controller = GameObject.Find("Forest Controller").GetComponent<ForestController>();
    }

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        RefillSap();
        Destroy(gameObject);
    }

    private void RefillSap() 
    {
        _controller.Sap += (_controller.maxSap * sapRefillPercentage);
        if (_controller.Sap > _controller.maxSap) 
        {
            _controller.Sap = _controller.maxSap;
        }
    }
}
