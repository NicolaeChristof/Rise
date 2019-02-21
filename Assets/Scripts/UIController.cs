using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public Button pauseButton;

    private void Start() {
        pauseButton.onClick.AddListener(pauseEvent);
    }

    void pauseEvent() {
        SceneManager.LoadScene( SceneManager.GetActiveScene().buildIndex );
    }
}
