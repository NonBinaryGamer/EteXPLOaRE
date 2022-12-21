using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class UnitManager : MonoBehaviour
{

    public GameObject SelectedUnitsLabel;
    private TMP_Text selected_units_label;

    // Start is called before the first frame update
    void Start()
    {
        selected_units_label = SelectedUnitsLabel.GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
            {
                // Move the units to the world position
                foreach (UnitController unit in Globals.SELECTED_UNITS)
                {
                    unit.MoveUnit(hit.point);
                }
            }
        }

        if (Globals.SELECTED_UNITS.Count > 0)
        {
            string unit_string = "Selected Units: \n";

            foreach (UnitController unit in Globals.SELECTED_UNITS)
            {
                unit_string += unit.ToString() + "\n";
            }
            selected_units_label.text = unit_string;
            selected_units_label.enabled = true;
        }
        else
        {
            selected_units_label.enabled = false;
        }

    }
}
