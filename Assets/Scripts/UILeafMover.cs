using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UILeafMover : MonoBehaviour {

    public Camera cam;

    private Image leafImage;

    private Transform uiBranch;

    // Start is called before the first frame update
    void Start() {
        leafImage = GetComponent<Image>();
        uiBranch = FindObjectOfType<UIBranchManager>().transform;
        leafImage.enabled = false;
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void MoveLeaf(Vector3 startPos) {
        leafImage.enabled = true;

        Vector3 uiPos = cam.WorldToScreenPoint(startPos);
        leafImage.transform.position = uiPos;

        leafImage.transform.DOMove(uiBranch.position, 2f, false).OnComplete(SetInvisible);
    }

    private void SetInvisible()
    {
        leafImage.enabled = false;
    }
}
