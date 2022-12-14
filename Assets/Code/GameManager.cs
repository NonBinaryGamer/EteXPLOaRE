using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public List<Utils.InventoryItem> required_items;
    public GameObject debug_log_container;
    public GameObject debug_log_example;

    private float offset_log_location;

    // Start is called before the first frame update
    void Awake()
    {
        Globals.REQUIRED_ITEMS = required_items;
        Globals.MANAGER = this;
        offset_log_location = debug_log_container.transform.GetChild(debug_log_container.transform.childCount - 1).GetComponent<RectTransform>().anchoredPosition.y - 22;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)) {
        }
    }

    public void AddLog(string value) {
        AddLog(value, Color.white);
    }

    public void AddLog(string value, Color color) {
        GameObject new_log_object = Instantiate(debug_log_example, debug_log_container.transform);
        new_log_object.GetComponent<RectTransform>().anchoredPosition = new Vector3(
            debug_log_container.transform.GetChild(debug_log_container.transform.childCount - 1).GetComponent<RectTransform>().anchoredPosition.x, 
            offset_log_location
        );
        offset_log_location -= 22;
        TMP_Text new_log_text_mesh = new_log_object.GetComponent<TMP_Text>();

        // RectTransform last_log_rect = debug_log_container.transform.GetChild(debug_log_container.transform.childCount - 1).GetComponent<RectTransform>();
        // new_log_rect.anchoredPosition = new Vector2(0, 1);
        // new_log_rect.anchoredPosition = new Vector2(
        //     last_log_rect.anchoredPosition.x,
        //     last_log_rect.anchoredPosition.y + last_log_rect.rect.height + 5
        // );

        new_log_text_mesh.text = value;
        new_log_text_mesh.color = color;


    }
}
