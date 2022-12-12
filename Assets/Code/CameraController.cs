using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{
    public Camera main_camera;
    public GameObject main_menu;
    public Slider sld_music;
    public Slider sld_sfx;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        int superSpeed = 1;
        if(Input.GetButton("SpeedUp")) {
            superSpeed = 5;
        }

        main_camera.transform.position = new Vector3(
            main_camera.transform.position.x + (Input.GetAxis("Horizontal") * Time.deltaTime * 10 * superSpeed),
            Mathf.Clamp(main_camera.transform.position.y + (Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * -300 * superSpeed), 2, 25),
            main_camera.transform.position.z + (Input.GetAxis("Vertical") * Time.deltaTime * 10 * superSpeed)
        );
        
        if(Input.GetKeyDown(KeyCode.Escape)) {
            Debug.Log("Escape is pressed");
            main_menu.SetActive(!main_menu.activeSelf);
        }

    }

    public void LoadTutorial() {
        SceneManager.LoadScene("SampleScene");
    }

    public void LoadLevel1() {
        SceneManager.LoadScene("DustyPlanet");
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void UpdateMusicVolume() {
        Globals.MUSIC_VOLUME = (int)(sld_music.value * 100f);
    }

    public void UpdateSFXVolume() {
        Globals.SFX_VOLUME = (int)(sld_sfx.value * 100f);
    }
}
