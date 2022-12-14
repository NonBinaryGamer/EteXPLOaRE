using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IntractableController : MonoBehaviour
{
    private Collider interaction_area;
    private ObjectState _State;

    public List<GameObject> interactive_units;
    public List<GameObject> UI_Components;

    public ObjectState State {
        set {
            _State = value;
            state_label.text = _State.ToString();
        }
        get {return _State;}
    }

    public string objectName;

    private string _objectName;

    private List<GameObject> units_to_remove = new List<GameObject>();

    public string ObjectName {
        set {
            _objectName = value;
            type_label.text = _objectName.ToString();
        }
        get {return _objectName;}
    }

    public GameObject interaction_model;
    public TMP_Text type_label;
    public TMP_Text state_label;

    public enum ObjectState
    {
        IDLE,
        INTERACTION_START,
        INTERACTION_END,
        BEING_MOVED,
        WORKING,
        BROKEN,
        OUTPUT_FULL,
        INSUFFICIENT_RESOURCES,
        INSUFFICIENT_STAFF,
    }

    // Start is called before the first frame update
    void Start()
    {
        interaction_area = GetComponent<Collider>();
        State = ObjectState.IDLE;
        
        foreach (GameObject go in UI_Components) {
            go.SetActive(false);
        }
    }

    private void Awake() {
        ObjectName = objectName;
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject go in units_to_remove) {
            interactive_units.Remove(go);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag.CompareTo("Unit") == 0) {
            if(!interactive_units.Contains(other.gameObject)) {
                interactive_units.Add(other.gameObject);
            }
            // Globals.MANAGER.AddLog("Unit has triggered " + objectName);
            State = ObjectState.INTERACTION_START;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag.CompareTo("Unit") == 0) {
            State = ObjectState.INTERACTION_END;
            units_to_remove.Add(other.gameObject);
            UnitController unit = other.gameObject.GetComponent<UnitController>();
            unit.StopWork();
        }
    }

    private void OnTriggerStay(Collider other) {
        if (other.tag.CompareTo("Unit") == 0) {
            UnitController unit = other.gameObject.GetComponent<UnitController>();
            unit.DoWork(ObjectName);
        }
    }

    private void OnMouseOver() {
        foreach (GameObject go in UI_Components) {
            go.SetActive(true);
        }
    }

    private void OnMouseExit() {
        foreach (GameObject go in UI_Components) {
            go.SetActive(false);
        }
    }
}
