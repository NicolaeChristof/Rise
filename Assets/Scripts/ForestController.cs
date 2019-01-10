using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ForestController : MonoBehaviour 
{
    public float maxSap;
    public float spawnLocationOffset;
    public Camera mainCam;
    public GameObject branchObj;
    public enum Action { Grow, Destroy };

    private Slider _sapUI;
    private float _sap;
    private Action _selectedAction;
    private Text _selectedActionUI;

    #region Public Properties

    public bool HasSap 
    {
        get { return _sap > 0; }
    }

    public float Sap 
    {
        get { return _sap;  }
        set 
        {
            _sap = value;
            _sapUI.value = _sap;
        }
    }

    public Action SelectedAction
    {
        get { return _selectedAction; }
        set
        {
            _selectedAction = value;

            switch (value)
            {
                case (Action.Grow):
                    _selectedActionUI.text = "<=> Grow Branch <=>";
                    break;
                case (Action.Destroy):
                    _selectedActionUI.text = "<=> Destroy Branch <=>";
                    break;
            }
        }
    }

    #endregion Public Properties

    #region Private Properties

    private bool ScrollingUp
    {
        get { return Input.GetAxis("Mouse ScrollWheel") > 0f; }
    }

    private bool ScrollingDown
    {
        get { return Input.GetAxis("Mouse ScrollWheel") < 0f; }
    }

    #endregion Private Properties

    private void Start() 
    {
        _sapUI = GameObject.Find("Sap Bar").GetComponent<Slider>();
        _sapUI.maxValue = maxSap;
        Sap = maxSap;

        _selectedActionUI = GameObject.Find("Selected Action").GetComponent<Text>();
        SelectedAction = Action.Grow;
    }

    // Update is called once per frame.
    private void Update() 
    {
        HandleLeftClicks();
        HandleScrolling();
        HandleArrowKeyPresses();
    }

    #region Private Helper Functions

    private void HandleLeftClicks() 
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            var mouseRayTarget = Physics2D.Raycast(mainCam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (SelectedAction == Action.Grow && HasSap)
            {
                if (mouseRayTarget.collider != null)
                {
                    if (mouseRayTarget.collider.tag == "Tree")
                    {
                        GrowNewBranch(mouseRayTarget);
                    }
                    else if (mouseRayTarget.collider.tag == "Branch")
                    {
                        GrowExistingBranch(mouseRayTarget);
                    }
                }
            }
            else if (SelectedAction == Action.Destroy)
            {
                if (mouseRayTarget.collider != null)
                {
                    if (mouseRayTarget.collider.tag == "Branch")
                    {
                        DestroyBranchIfAbovePlayer(mouseRayTarget.collider.gameObject);
                    }
                }
            }    
        }
    }

    private void HandleScrolling()
    {
        if (ScrollingUp)
        {
            SelectedAction = SelectedAction.Next();
        }
        if (ScrollingDown)
        {
            SelectedAction = SelectedAction.Previous(); ;
        }
    }

    private void HandleArrowKeyPresses()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            SelectedAction = SelectedAction.Next();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            SelectedAction = SelectedAction.Previous();
        }
    }

    #region Growing New Branches (Helper Functions)

    private void GrowNewBranch(RaycastHit2D mouseRayTarget)
    {
        var spawnLocation = CalculateBranchSpawnLocation(mouseRayTarget);
        SpawnBranch(spawnLocation);
    }

    private Vector2 CalculateBranchSpawnLocation(RaycastHit2D mouseRayTarget)
    {
        float horizontalSpawnPoint = 0f;
        float verticalSpawnPoint = mouseRayTarget.point.y;

        if (mouseRayTarget.point.x < 0)
        {
            horizontalSpawnPoint -= spawnLocationOffset;
        }
        else
        {
            horizontalSpawnPoint += spawnLocationOffset;
        }

        return new Vector2(horizontalSpawnPoint, verticalSpawnPoint);
    }

    private void SpawnBranch(Vector2 branchSpawnLocation)
    {
        Instantiate(branchObj, branchSpawnLocation, Quaternion.identity);
    }

    #endregion Growing New Branches

    #region Growing Existing Branches (Helper Functions)

    private void GrowExistingBranch(RaycastHit2D mouseRayTarget)
    {
        var branchScript = mouseRayTarget.collider.GetComponent<BranchScript>();
        branchScript.IsGrowing = true;
    }

    #endregion Growing Existing Branches

    #region Destroying Branches

    private void DestroyBranchIfAbovePlayer(GameObject branch)
    {
        var playerPosition = GameObject.Find("Player").GetComponent<Transform>().position;
        if (branch.transform.position.y > playerPosition.y)
        {
            var branchScript = branch.GetComponent<BranchScript>();
            branchScript.FadeAndDestroy();
        }
    }

    #endregion Destroying Branches

    #endregion Private Helper Functions
}
