using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RiseExtensions;
using DG.Tweening;

public class HealthUI : MonoBehaviour
{
    public List<Image> Hearts;
    public Sprite full;
    public Sprite half;
    public Sprite empty;

    public Image moveableAcorn;

    private Camera cam;

    private Vector3 heartLocation;

    // These have the same utility as the corresponding variables in UIBranchManager
    private Vector3 startingSize;
    private float maxScaleUponImpact = 1.1f;

    private float cursor = 0;

    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("Squirrel Camera").GetComponent<Camera>();
        UpdateHealth();
        startingSize = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateHealth()
    {
        int health = GameModel.squirrelHealth;

        switch (health)
        {
            case 10:
                for( int i = 0; i < Hearts.Count; i++)
                {
                    Image temp = Hearts[i].GetComponent<Image>();
                    temp.sprite = full;
                }
                heartLocation = Hearts[Hearts.Count - 1 - Mathf.FloorToInt(cursor / 2)].transform.position;
                break;
            case 9:
                for (int i = 0; i < Hearts.Count - 1; i++)
                {
                    Image temp = Hearts[i].GetComponent<Image>();
                    temp.sprite = full;
                }
                Hearts[4].GetComponent<Image>().sprite = half;
                heartLocation = Hearts[Hearts.Count - 1 - Mathf.FloorToInt(cursor / 2)].transform.position;
                break;
            case 8:
                for (int i = 0; i < Hearts.Count - 1; i++)
                {
                    Image temp = Hearts[i].GetComponent<Image>();
                    temp.sprite = full;
                }
                Hearts[4].GetComponent<Image>().sprite = empty;
                heartLocation = Hearts[Hearts.Count - 1 - Mathf.FloorToInt(cursor / 2)].transform.position;
                break;
            case 7:
                for (int i = 0; i < Hearts.Count - 2; i++)
                {
                    Image temp = Hearts[i].GetComponent<Image>();
                    temp.sprite = full;
                }
                Hearts[3].GetComponent<Image>().sprite = half;
                Hearts[4].GetComponent<Image>().sprite = empty;
                heartLocation = Hearts[Hearts.Count - 2 - Mathf.FloorToInt(cursor / 2)].transform.position;
                break;
            case 6:
                for (int i = 0; i < Hearts.Count - 2; i++)
                {
                    Image temp = Hearts[i].GetComponent<Image>();
                    temp.sprite = full;
                }
                Hearts[3].GetComponent<Image>().sprite = empty;
                Hearts[4].GetComponent<Image>().sprite = empty;
                heartLocation = Hearts[Hearts.Count - 2 - Mathf.FloorToInt(cursor / 2)].transform.position;
                break;
            case 5:
                for (int i = 0; i < Hearts.Count - 3; i++)
                {
                    Image temp = Hearts[i].GetComponent<Image>();
                    temp.sprite = full;
                }
                Hearts[2].GetComponent<Image>().sprite = half;
                Hearts[3].GetComponent<Image>().sprite = empty;
                Hearts[4].GetComponent<Image>().sprite = empty;
                heartLocation = Hearts[Hearts.Count - 3 - Mathf.FloorToInt(cursor / 2)].transform.position;
                break;
            case 4:
                for (int i = 0; i < Hearts.Count - 2; i++)
                {
                    Image temp = Hearts[i].GetComponent<Image>();
                    temp.sprite = full;
                }
                Hearts[2].GetComponent<Image>().sprite = empty;
                Hearts[3].GetComponent<Image>().sprite = empty;
                Hearts[4].GetComponent<Image>().sprite = empty;
                heartLocation = Hearts[Hearts.Count - 3 - Mathf.FloorToInt(cursor / 2)].transform.position;
                break;
            case 3:
                Hearts[0].GetComponent<Image>().sprite = full;
                Hearts[1].GetComponent<Image>().sprite = half;
                Hearts[2].GetComponent<Image>().sprite = empty;
                Hearts[3].GetComponent<Image>().sprite = empty;
                Hearts[4].GetComponent<Image>().sprite = empty;
                heartLocation = Hearts[Hearts.Count - 4 - Mathf.FloorToInt(cursor / 2)].transform.position;
                break;
            case 2:
                Hearts[0].GetComponent<Image>().sprite = full;
                Hearts[1].GetComponent<Image>().sprite = empty;
                Hearts[2].GetComponent<Image>().sprite = empty;
                Hearts[3].GetComponent<Image>().sprite = empty;
                Hearts[4].GetComponent<Image>().sprite = empty;
                heartLocation = Hearts[Hearts.Count - 4 - Mathf.FloorToInt(cursor / 2)].transform.position;
                break;
            case 1:
                Hearts[0].GetComponent<Image>().sprite = half;
                Hearts[1].GetComponent<Image>().sprite = empty;
                Hearts[2].GetComponent<Image>().sprite = empty;
                Hearts[3].GetComponent<Image>().sprite = empty;
                Hearts[4].GetComponent<Image>().sprite = empty;
                heartLocation = Hearts[Hearts.Count - 5].transform.position;
                break;
            case 0:
                Hearts[0].GetComponent<Image>().sprite = empty;
                Hearts[1].GetComponent<Image>().sprite = empty;
                Hearts[2].GetComponent<Image>().sprite = empty;
                Hearts[3].GetComponent<Image>().sprite = empty;
                Hearts[4].GetComponent<Image>().sprite = empty;
                heartLocation = Hearts[Hearts.Count - 5].transform.position;
                break;
            default:
                break;

        }
        
    }

    // This is almost identical to the MoveLeaf function in
    // UIBranchManager (except for the cursor logic and
    // the need to get the current sap value, since the current
    // health value is a GameModel variable)
    public void MoveAcorn(Vector3 startPos) {
        if (GameModel.squirrelHealth < 10) {
            cursor++;
            Vector3 uiPos = cam.WorldToScreenPoint(startPos);
            Image currentAcorn = GameObject.Instantiate(moveableAcorn, uiPos, Quaternion.Euler(0f, 0f, Random.Range(0f, 359f)), transform);

            currentAcorn.transform.DOMove(heartLocation, GameModel.tweenTime, false).OnComplete(() => HealthFullyUpdated(currentAcorn));
            currentAcorn.transform.DOLocalRotate(transform.localEulerAngles, 2f);
        }
    }

    // This function is called once the movement and rotation tweens are done
    // in MoveAcorn
    private void HealthFullyUpdated(Image currentAcorn) {
        cursor--;
        currentAcorn.enabled = false;
        transform.DOScale(startingSize * maxScaleUponImpact, .15f).OnComplete(() => transform.DOScale(startingSize, .15f));
    }
}
