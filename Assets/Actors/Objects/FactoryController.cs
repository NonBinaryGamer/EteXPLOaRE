using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FactoryController : MonoBehaviour
{
    public Utils.InventoryItem[] inputs;


    public List<GameObject> factory_units;
    private List<GameObject> units_to_remove = new List<GameObject>();
    public Utils.InventoryItem output;
    private int _output_count;

    public int OutputCount
    {
        get { return _output_count; }
        set
        {
            _output_count = value;
            output.count = _output_count;
            label_Outputs.text = "Ready: " + OutputCount.ToString() + " " + output.name;
        }
    }

    private int _current_staff;

    public int CurrentStaff
    {
        get { return _current_staff; }
        set
        {
            _current_staff = value;
            label_staff.text = "Staff is " + CurrentStaff.ToString() + "/" + required_staff.ToString();
        }
    }


    public int required_staff;
    public float timer_max;
    private float timer_current = 0f;

    private IntractableController intractableController;


    public TMP_Text label_staff;
    public TMP_Text label_Inputs;
    public TMP_Text label_Outputs;


    private void Awake()
    {
        intractableController = GetComponent<IntractableController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        OutputCount = 0;
        CurrentStaff = 0;

        label_Inputs.enabled = false;
        label_Inputs.enabled = !CheckInputs();
        name = intractableController.ObjectName;
    }

    // Update is called once per frame
    void Update()
    {
        HandleObjectState(intractableController.State);
    }

    public void HandleObjectState(IntractableController.ObjectState state)
    {
        // CurrentStaff = factory_units.Count;

        switch (state)
        {
            case IntractableController.ObjectState.IDLE:
                intractableController.tooltip.SetContent("Waiting for staff.");
                break;
            case IntractableController.ObjectState.INTERACTION_START:
                intractableController.tooltip.SetContent("Fire up in progress.");
                CurrentStaff += 1;
                GetResourcesFromUnit();
                if (CheckInputs())
                {
                    label_Inputs.enabled = false;
                    intractableController.State = IntractableController.ObjectState.WORKING;
                }
                else
                {
                    label_Inputs.enabled = true;
                    intractableController.State = IntractableController.ObjectState.INSUFFICIENT_RESOURCES;
                }
                break;
            case IntractableController.ObjectState.INTERACTION_END:
                intractableController.tooltip.SetContent("Shutting down.");
                CurrentStaff -= 1;
                GiveResourcesToUnit();
                intractableController.State = IntractableController.ObjectState.IDLE;
                break;
            case IntractableController.ObjectState.BEING_MOVED:
                break;
            case IntractableController.ObjectState.WORKING:
                intractableController.tooltip.SetContent("Operating.");
                if (OutputCount >= output.max)
                {
                    intractableController.State = IntractableController.ObjectState.OUTPUT_FULL;
                }

                if (_current_staff < required_staff)
                {
                    intractableController.State = IntractableController.ObjectState.INSUFFICIENT_STAFF;
                }

                foreach (Utils.InventoryItem item in inputs)
                {
                    if (item.count < item.required)
                    {
                        intractableController.State = IntractableController.ObjectState.INSUFFICIENT_RESOURCES;
                    }
                }

                if (Globals.GAME_ACTIVE)
                {
                    timer_current += Time.deltaTime;
                }

                if (timer_current >= timer_max)
                {
                    timer_current = 0f;
                    OutputCount += 1;
                    foreach (Utils.InventoryItem item in inputs)
                    {
                        item.count -= item.required;
                    }
                }
                break;
            case IntractableController.ObjectState.BROKEN:
                break;
            case IntractableController.ObjectState.OUTPUT_FULL:
                intractableController.tooltip.SetContent("The output is full.");
                break;
            case IntractableController.ObjectState.INSUFFICIENT_RESOURCES:
                intractableController.tooltip.SetContent("Missing required resources.");
                label_Inputs.enabled = !CheckInputs();
                break;
            case IntractableController.ObjectState.INSUFFICIENT_STAFF:
                intractableController.tooltip.SetContent("Not enough staff.");
                break;
        }
        intractableController.tooltip.AddContent("");

        if (required_staff > 0)
        {
            intractableController.tooltip.AddContent("Staff: (Requires " + required_staff + ")");
            foreach (GameObject go in factory_units)
            {
                UnitController unit = go.GetComponent<UnitController>();
                intractableController.tooltip.AddContent(unit.UnitName);
            }
            if (factory_units.Count == 0)
            {
                intractableController.tooltip.AddContent("None");
            }

            intractableController.tooltip.AddContent("");
        }

        if (inputs.Length > 0)
        {
            intractableController.tooltip.AddContent("Consumes: ");
            foreach (Utils.InventoryItem item in inputs)
            {
                intractableController.tooltip.AddContent(item.required + " " + item.name + " (has " + item.count + ")");
            }
            intractableController.tooltip.AddContent("");
        }

        if (output != null)
        {
            intractableController.tooltip.AddContent("Produces: ");
            intractableController.tooltip.AddContent(output.count + "/" + output.max + " " + output.name);
            intractableController.tooltip.AddContent("");
        }

        foreach (GameObject go in units_to_remove)
        {
            factory_units.Remove(go);
        }
        units_to_remove.Clear();
    }

    private void GetResourcesFromUnit()
    {
        foreach (GameObject go in factory_units)
        {
            Debug.Log(go.name + " has triggered entry with " + intractableController.ObjectName);
            UnitController unit = go.GetComponent<UnitController>();

            foreach (Utils.InventoryItem item in inputs)
            {
                if (unit.Inventory.name.Equals(item.name))
                {
                    if (item.count < item.max)
                    {
                        Debug.Log("Taking " + item.name + " from " + unit.UnitName);
                        item.count += unit.TakeItem(item.name).count;
                    }
                }
            }
        }
    }

    private void GiveResourcesToUnit()
    {
        foreach (GameObject go in factory_units)
        {
            UnitController unit = go.GetComponent<UnitController>();
            if (output.count > 0)
            {
                Debug.Log("Giving " + output.count + " " + output.name + " to " + unit.UnitName);
                if (unit.GiveItem(output))
                {
                    OutputCount = 0;
                }
            }
        }

    }

    private bool CheckInputs()
    {
        bool inputs_valid = true;

        string output_string = "Missing ";

        foreach (Utils.InventoryItem factoryInput in inputs)
        {
            if (factoryInput.count < factoryInput.required)
            {
                output_string += (factoryInput.required - factoryInput.count).ToString() + " " + factoryInput.name + " ";
                inputs_valid = false;
            }
        }

        label_Inputs.text = output_string;
        return inputs_valid;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.CompareTo("Unit") == 0)
        {
            if (!this.factory_units.Contains(other.gameObject))
            {
                this.factory_units.Add(other.gameObject);
                Debug.Log("Adding " + other.gameObject + " to the list: " + this.factory_units.Count);
            }
            // Globals.MANAGER.AddLog("Unit has triggered a factory");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.CompareTo("Unit") == 0)
        {
            units_to_remove.Add(other.gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
    }

}
