using System;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    public Texture2D normalCursor;
    public Texture2D handCursor;
    public Texture2D deleteCursor;
    public Texture2D invalidCursor;

    private ForestController _forest;
    private RaycastHit2D _mouseLocation;
    private enum CursorOption { Default, Hand, Delete, Invalid, Undefined };
    private CursorOption _cursor;

    #region Private Properties

    private bool HoveringTree
    {
        get
        {
            if (_mouseLocation.collider == null)
            {
                return false;
            }

            return _mouseLocation.collider.tag == "Tree";
        }
    }

    private bool HoveringBranch
    {
        get
        {
            if (_mouseLocation.collider == null)
            {
                return false;
            }

            return _mouseLocation.collider.tag == "Branch";
        }
    }

    #endregion Private Properties

    private void Start()
    {
        _forest = GameObject.Find("Forest Controller").GetComponent<ForestController>();
        _cursor = CursorOption.Undefined;
	}
	
	private void Update()
    {
        _mouseLocation = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        UpdateCursorTexture();
    }

    #region Private Helper Functions

    private void UpdateCursorTexture()
    {
        if (AttemptingInvalidAction())
        {
            SetCursorToInvalid();
        }
        else
        {
            if ((HoveringTree || HoveringBranch) && _forest.SelectedAction == ForestController.Action.Grow)
            {
                SetCursorToHand();
            }
            else if (HoveringBranch && _forest.SelectedAction == ForestController.Action.Destroy)
            {
                SetCursorToDelete();
            }
            else
            {
                SetCursorToDefault();
            }
        }
    }


    #region UpdateCursorTexture helper functions

    private bool AttemptingInvalidAction()
    {
        if (HoveringBranch)
        {
            var branch = _mouseLocation.collider.gameObject;
            if (_forest.SelectedAction == ForestController.Action.Grow)
            {
                var branchScript = branch.GetComponent<BranchScript>();
                if (branchScript.IsMaxLength)
                {
                    return true;
                }
                if (!_forest.HasSap)
                {
                    return true;
                }
                return false;
            }
            else
            {
                // if we are here, the player is hovering a branch in Destroy mode
                return BranchIsBelowPlayer(branch);
            }
        }
        else if (HoveringTree)
        { 
            if (!_forest.HasSap)
            {
                return true;
            }
            if (_forest.SelectedAction == ForestController.Action.Destroy)
            {
                return true;
            }
            return false;
        }
        else
        {
            return false;
        }
    }

    private bool BranchIsBelowPlayer(GameObject branch)
    {
        var playerHeight = GameObject.Find("Player").transform.position.y;

        return branch.transform.position.y < playerHeight;
    }

    private void SetCursorToInvalid()
    {
        if (_cursor != CursorOption.Invalid)
        {
            var width = invalidCursor.width;
            var height = invalidCursor.height;
            var hotspot = new Vector2(width / 2, height / 2);
            Cursor.SetCursor(invalidCursor, hotspot, CursorMode.Auto);
            _cursor = CursorOption.Invalid;
        }
    }

    private void SetCursorToHand()
    {
        if (_cursor != CursorOption.Hand)
        {
            var width = handCursor.width;
            var hotspotX = (width / 2) - (width * 0.05f);
            var hotspot = new Vector2(hotspotX, 0);
            Cursor.SetCursor(handCursor, hotspot, CursorMode.Auto);
            _cursor = CursorOption.Hand;
        }
    }

    private void SetCursorToDelete()
    {
        if (_cursor != CursorOption.Delete)
        {
            var width = deleteCursor.width;
            var height = deleteCursor.height;
            var hotspot = new Vector2(width / 2, height / 2);
            Cursor.SetCursor(deleteCursor, hotspot, CursorMode.Auto);
            _cursor = CursorOption.Delete;
        }
    }

    private void SetCursorToDefault()
    {
        if (_cursor != CursorOption.Default)
        {
            var width = normalCursor.width;
            var hotspotX = (width / 2) - (width * 0.1f);
            var hotspot = new Vector2(hotspotX, 0);
            Cursor.SetCursor(normalCursor, hotspot, CursorMode.Auto);
            _cursor = CursorOption.Default;
        }
    }

    #endregion UpdateCursorTexture helper functions

    #endregion Private Helper Functions
}
