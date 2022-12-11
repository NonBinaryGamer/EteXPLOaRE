using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitController : MonoBehaviour
{
    public GameObject selectionCircle;
    
    private NavMeshAgent navMeshAgent;

    private void Awake() {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }


    private void OnMouseDown()
    {
        Select(true);
    }

    public void Select() { Select(false); }
    public void Select(bool clearSelection)
    {
        if (Globals.SELECTED_UNITS.Contains(this)) return;
        if (clearSelection)
        {
            List<UnitController> selectedUnits = new List<UnitController>(Globals.SELECTED_UNITS);
            foreach (UnitController um in selectedUnits)
                um.Deselect();
        }
        Globals.SELECTED_UNITS.Add(this);
        if (selectionCircle != null){
            selectionCircle.SetActive(true);
        }
    }

    public void Deselect()
    {
        if (!Globals.SELECTED_UNITS.Contains(this)) return;
        Globals.SELECTED_UNITS.Remove(this);
        if (selectionCircle != null){
            selectionCircle.SetActive(false);
        }
    }

    // This function moves the unit to the specified position
    public void MoveUnit(Vector3 position)
    {
        // Tell the agent where to go
        navMeshAgent.destination = position;
    }
}
