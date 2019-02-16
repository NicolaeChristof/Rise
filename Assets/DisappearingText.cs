using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisappearingText : MonoBehaviour
{
    private Text displayedText;

    // Start is called before the first frame update
    void Start()
    {
        displayedText = this.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void MakeTextDisappear() {

    }
}
