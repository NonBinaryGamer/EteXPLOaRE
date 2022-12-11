using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitController : MonoBehaviour
{
    public TMPro.TMP_Text label_inventory;
    public TMPro.TMP_Text label_name;
    public GameObject selectionCircle;
    public string UnitName = "Honest Explorer";

    public Utils.InventoryItem Inventory = null;
    private NavMeshAgent navMeshAgent;

    // public Utils.InventoryItem Inventory {
    //     get { return _inventory; }
    //     set { 
    //         _inventory = value; 
    //         if (Inventory != null) {
    //             label_inventory.text = "Caring: " + Inventory.count + " " + Inventory.name;
    //             label_inventory.enabled = true;
    //         }
    //         else {
    //             label_inventory.enabled = false;
    //         }
    //     }
    // }

    private void Awake() {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Start() {
        label_name.text = UnitName;
        label_inventory.enabled = false;
    }

    private void Update() {
        if (Inventory != null) {
            label_inventory.text = "Caring: " + Inventory.count + " " + Inventory.name;
            label_inventory.enabled = true;
        }
        else {
            label_inventory.enabled = false;
        }
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


    public bool GiveItem(Utils.InventoryItem item) {
        if (!Inventory.in_use) {
            Inventory = new Utils.InventoryItem(item.name, item.count);
            Debug.Log(UnitName + " received " + Inventory.count + " " + Inventory.name);
            return true;
        }

        return false;
    }

    public Utils.InventoryItem TakeItem(string looking_for) {
        if (Inventory.name.Equals(looking_for)) {
            Utils.InventoryItem to_return = new Utils.InventoryItem(Inventory.name, Inventory.count);
            Inventory.in_use = false;
            Debug.Log(UnitName + " gave " + to_return.count + " " + to_return.name);

            return to_return;
        }

        return null;
    }
}
