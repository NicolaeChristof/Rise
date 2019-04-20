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
    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("Squirrel Camera").GetComponent<Camera>();
        UpdateHealth();
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
                break;
            case 9:
                for (int i = 0; i < Hearts.Count - 1; i++)
                {
                    Image temp = Hearts[i].GetComponent<Image>();
                    temp.sprite = full;
                }
                Hearts[4].GetComponent<Image>().sprite = half;
                break;
            case 8:
                for (int i = 0; i < Hearts.Count - 1; i++)
                {
                    Image temp = Hearts[i].GetComponent<Image>();
                    temp.sprite = full;
                }
                Hearts[4].GetComponent<Image>().sprite = empty;
                break;
            case 7:
                for (int i = 0; i < Hearts.Count - 2; i++)
                {
                    Image temp = Hearts[i].GetComponent<Image>();
                    temp.sprite = full;
                }
                Hearts[3].GetComponent<Image>().sprite = half;
                Hearts[4].GetComponent<Image>().sprite = empty;
                break;
            case 6:
                for (int i = 0; i < Hearts.Count - 2; i++)
                {
                    Image temp = Hearts[i].GetComponent<Image>();
                    temp.sprite = full;
                }
                Hearts[3].GetComponent<Image>().sprite = empty;
                Hearts[4].GetComponent<Image>().sprite = empty;
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
                break;
            case 3:
                Hearts[0].GetComponent<Image>().sprite = full;
                Hearts[1].GetComponent<Image>().sprite = half;
                Hearts[2].GetComponent<Image>().sprite = empty;
                Hearts[3].GetComponent<Image>().sprite = empty;
                Hearts[4].GetComponent<Image>().sprite = empty;
                break;
            case 2:
                Hearts[0].GetComponent<Image>().sprite = full;
                Hearts[1].GetComponent<Image>().sprite = empty;
                Hearts[2].GetComponent<Image>().sprite = empty;
                Hearts[3].GetComponent<Image>().sprite = empty;
                Hearts[4].GetComponent<Image>().sprite = empty;
                break;
            case 1:
                Hearts[0].GetComponent<Image>().sprite = half;
                Hearts[1].GetComponent<Image>().sprite = empty;
                Hearts[2].GetComponent<Image>().sprite = empty;
                Hearts[3].GetComponent<Image>().sprite = empty;
                Hearts[4].GetComponent<Image>().sprite = empty;
                break;
            case 0:
                Hearts[0].GetComponent<Image>().sprite = empty;
                Hearts[1].GetComponent<Image>().sprite = empty;
                Hearts[2].GetComponent<Image>().sprite = empty;
                Hearts[3].GetComponent<Image>().sprite = empty;
                Hearts[4].GetComponent<Image>().sprite = empty;
                break;
            default:
                break;

        }
        
    }

    public void MoveAcorn(Vector3 startPos) {
        Vector3 uiPos = cam.WorldToScreenPoint(startPos);
        Image currentAcorn = GameObject.Instantiate(moveableAcorn, uiPos, Random.rotation, transform);

        currentAcorn.transform.DOMove(transform.position, 2f, false).OnComplete(() => currentAcorn.enabled = false);
        currentAcorn.transform.DOLocalRotate(transform.localEulerAngles, 2f);
    }
}
