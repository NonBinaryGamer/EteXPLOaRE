using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class UnitController : MonoBehaviour
{
    public GameObject NameLabel;
    private TMP_Text label_name;
    public GameObject selectionCircle;
    public string UnitName = "Honest Explorer";

    public Utils.InventoryItem Inventory = null;
    private NavMeshAgent navMeshAgent;

    private UnitState _unitState;
    public UnitState unitState {get; set;}
    private string target_object;

    public AudioClip[] walkingSounds;
    public AudioClip teleportSound;
    private AudioSource audioSource;

    private bool game_won = false;

    public enum UnitState
    {
        IDLE,
        WALKING,
        OPERATING,
    }

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
        audioSource = GetComponent<AudioSource>();
    }

    private void Start() {
        label_name = NameLabel.GetComponent<TMP_Text>();

        label_name.text = UnitName;
        unitState = UnitState.IDLE;
    }

    private void Update() {

        if (audioSource == null) {
            audioSource = GetComponent<AudioSource>();
        }
        audioSource.volume = Globals.SFX_VOLUME / 100f / 6f;

        if (navMeshAgent == null) {
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        if (unitState != UnitState.OPERATING) {
            if (navMeshAgent.velocity.magnitude < 0.1f) {
                unitState = UnitState.IDLE;
            }
            else {
                unitState = UnitState.WALKING;

                if (!audioSource.isPlaying) {
                    int random_audio = UnityEngine.Random.Range(0, walkingSounds.Length);
                    audioSource.clip = walkingSounds[random_audio];
                    audioSource.Play();
                }
            }
        }

        if (game_won) {
            transform.localScale = new Vector3(transform.localScale.x - Time.deltaTime, transform.localScale.y - Time.deltaTime, transform.localScale.z - Time.deltaTime);
        }

        // Debug.Log(unitState.ToString());
    }

    override public string ToString() {
        string output_string = "";
        
        if (unitState == UnitState.OPERATING) {
            output_string = UnitName + " is operating " + target_object;
        }
        else if (unitState == UnitState.WALKING) {
            
            if (Inventory != null && Inventory.in_use) {
                output_string = UnitName + " is walking with " + Inventory.count + " " + Inventory.name;
            }
            else {
                output_string = UnitName + " is exploring";
            }
        }
        else if (unitState == UnitState.IDLE) {
            if (Inventory != null && Inventory.in_use) {
                output_string = UnitName + " is holding " + Inventory.count + " " + Inventory.name;
            }
            else {
                output_string = UnitName + " is resting";
            }
        }

        return output_string;
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

        unitState = UnitState.WALKING;
    }

    public void DoWork(string workplace) {
        unitState = UnitState.OPERATING;
        target_object = workplace;
    }

    public void StopWork() {
        target_object = "";
        unitState = UnitState.IDLE;
    }


    public bool GiveItem(Utils.InventoryItem item) {
        if (!Inventory.in_use) {
            Inventory = new Utils.InventoryItem(item.name, item.count);
            // Debug.Log(UnitName + " received " + Inventory.count + " " + Inventory.name);
            return true;
        }

        return false;
    }

    public Utils.InventoryItem TakeItem(string looking_for) {
        if (Inventory.name.Equals(looking_for)) {
            Utils.InventoryItem to_return = new Utils.InventoryItem(Inventory.name, Inventory.count);
            Inventory.in_use = false;
            Inventory.count = 0;
            Inventory.name = "";
            // Debug.Log(UnitName + " gave " + to_return.count + " " + to_return.name);

            return to_return;
        }

        return null;
    }

    public void GetReadyForLaunch() {
        Debug.Log(UnitName + " thinks the game is over");
        game_won = true;
        Deselect();
        audioSource = GetComponent<AudioSource>();
        audioSource.Stop();
        audioSource.clip = teleportSound;
        audioSource.Play();
    }
}
