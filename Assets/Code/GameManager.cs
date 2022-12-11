using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<Utils.InventoryItem> required_items;

    // Start is called before the first frame update
    void Awake()
    {
        Globals.REQUIRED_ITEMS = required_items;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
