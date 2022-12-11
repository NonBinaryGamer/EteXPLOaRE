using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera main_camera;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        

        main_camera.transform.position = new Vector3(
            main_camera.transform.position.x + (Input.GetAxis("Horizontal") * Time.deltaTime * 10),
            Mathf.Clamp(main_camera.transform.position.y + (Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * -300), 2, 25),
            main_camera.transform.position.z + (Input.GetAxis("Vertical") * Time.deltaTime * 10)
        );
        

    }
}
