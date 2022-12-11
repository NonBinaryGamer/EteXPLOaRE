using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableController : MonoBehaviour
{
    public Collider interaction_area;
    public GameObject interaction_model;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log("I'm being interacted with");
        if (other.tag.CompareTo("Unit") == 0) {
            Debug.Log("YES");
        }
    }
}
