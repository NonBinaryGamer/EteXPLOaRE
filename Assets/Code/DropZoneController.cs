using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropZoneController : MonoBehaviour
{
    private Collider interaction_area;

    public TMPro.TMP_Text label_objectives;
    public List<GameObject> interactive_units;

    // Start is called before the first frame update
    void Start()
    {
        interaction_area = GetComponent<Collider>();
    }

    private void Awake() {
        label_objectives.text = generate_objectives_label();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag.CompareTo("Unit") == 0) {
            UnitController unit = other.GetComponent<UnitController>();

            foreach(Utils.InventoryItem required_item in Globals.REQUIRED_ITEMS) {
                if (required_item.count < required_item.required) {
                    Utils.InventoryItem taken_item = unit.TakeItem(required_item.name);
                    if (taken_item != null) {
                        required_item.count += taken_item.count;
                        label_objectives.text = generate_objectives_label();
                    }
                }
            }
            interactive_units.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag.CompareTo("Unit") == 0) {
            interactive_units.Remove(other.gameObject);
        }
    }

    private string generate_objectives_label() {
        string output_string = "Required: \n";
        foreach (Utils.InventoryItem item in Globals.REQUIRED_ITEMS) {
            output_string += item.count + " / " + item.required + " " + item.name + "\n";
        }

        return output_string;
    }
}
