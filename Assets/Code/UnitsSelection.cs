using System.Collections.Generic;
using UnityEngine;

public class UnitsSelection : MonoBehaviour
{
    private bool _isDraggingMouseBox = false;
    private Vector3 _dragStartPosition;

    Ray _ray;
    RaycastHit _raycastHit;

    private void Update()
    {
        if (!Globals.GAME_ACTIVE)
        {
            _isDraggingMouseBox = false;
            _DeselectAllUnits();
        }

        if (Input.GetMouseButtonDown(0) && Globals.GAME_ACTIVE)
        {
            _isDraggingMouseBox = true;
            _dragStartPosition = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
            _isDraggingMouseBox = false;

        if (_isDraggingMouseBox && _dragStartPosition != Input.mousePosition)
            _SelectUnitsInDraggingBox();

        if (Globals.SELECTED_UNITS.Count > 0)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                _DeselectAllUnits();
            if (Input.GetMouseButtonDown(0))
            {
                _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(
                    _ray,
                    out _raycastHit,
                    1000f
                ))
                {
                    if (_raycastHit.transform.tag == "Terrain")
                        _DeselectAllUnits();
                }
            }
        }
    }

    void OnGUI()
    {
        if (_isDraggingMouseBox)
        {
            // Create a rect from both mouse positions
            var rect = Utils.GetScreenRect(_dragStartPosition, Input.mousePosition);
            Utils.DrawScreenRect(rect, new Color(0.7f, 0.8f, 0.75f, 0.2f));
            Utils.DrawScreenRectBorder(rect, 1, new Color(0.6f, 1f, 0.75f));
        }
    }

    private void _SelectUnitsInDraggingBox()
    {
        Bounds selectionBounds = Utils.GetViewportBounds(
            Camera.main,
            _dragStartPosition,
            Input.mousePosition
        );
        GameObject[] selectableUnits = GameObject.FindGameObjectsWithTag("Unit");
        bool inBounds;
        foreach (GameObject unit in selectableUnits)
        {
            inBounds = selectionBounds.Contains(
                Camera.main.WorldToViewportPoint(unit.transform.position)
            );
            if (inBounds)
                unit.GetComponent<UnitController>().Select();
            else
                unit.GetComponent<UnitController>().Deselect();
        }
    }

    private void _DeselectAllUnits()
    {
        List<UnitController> selectedUnits = new List<UnitController>(Globals.SELECTED_UNITS);
        foreach (UnitController um in selectedUnits)
            um.Deselect();
    }

}