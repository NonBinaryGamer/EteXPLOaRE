using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class CameraController : MonoBehaviour
{
    public Camera main_camera;
    public GameObject main_menu;
    public Slider sld_music;
    public Slider sld_sfx;

    public TextMeshProUGUI musicValueLbl;
    public TextMeshProUGUI effectsValueLbl;

    // Start is called before the first frame update
    void Start()
    {

        Globals.MUSIC_VOLUME = PlayerPrefs.GetInt("MUSIC_VOLUME", 25);
        Globals.SFX_VOLUME = PlayerPrefs.GetInt("SFX_VOLUME", 50);
        Globals.GAME_ACTIVE = true;

        Debug.Log((Globals.MUSIC_VOLUME / 100f).ToString("0%"));

        musicValueLbl.text = (Globals.MUSIC_VOLUME / 100f).ToString("0%");
        effectsValueLbl.text = (Globals.SFX_VOLUME / 100f).ToString("0%");

        sld_music.value = (Globals.MUSIC_VOLUME / 100f);
        sld_sfx.value = (Globals.SFX_VOLUME / 100f);

        sld_music.onValueChanged.AddListener((v) =>
        {
            musicValueLbl.text = v.ToString("0%");
            Globals.MUSIC_VOLUME = (int)(v * 100);
            PlayerPrefs.SetInt("MUSIC_VOLUME", Globals.MUSIC_VOLUME);
        });

        sld_sfx.onValueChanged.AddListener((v) =>
        {
            effectsValueLbl.text = v.ToString("0%");
            Globals.SFX_VOLUME = (int)(v * 100);
            PlayerPrefs.SetInt("SFX_VOLUME", Globals.SFX_VOLUME);
        });
    }

    // Update is called once per frame
    void Update()
    {

        int superSpeed = 1;
        if (Input.GetButton("SpeedUp"))
        {
            superSpeed = 5;
        }

        if (Input.GetKeyDown(KeyCode.F13))
        {
            PlayerPrefs.DeleteKey("REC_TUT");
            PlayerPrefs.DeleteKey("REC_DUST");
        }

        main_camera.transform.position = new Vector3(
            main_camera.transform.position.x + (Input.GetAxis("Horizontal") * Time.deltaTime * 10 * superSpeed),
            Mathf.Clamp(main_camera.transform.position.y + (Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * -300 * superSpeed), 2, 25),
            main_camera.transform.position.z + (Input.GetAxis("Vertical") * Time.deltaTime * 10 * superSpeed)
        );

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            main_menu.SetActive(!main_menu.activeSelf);
            Globals.GAME_ACTIVE = !main_menu.activeSelf;
        }

    }

    public void LoadTutorial()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void LoadLevel1()
    {
        SceneManager.LoadScene("DustyPlanet");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void UpdateMusicVolume()
    {
    }

    public void UpdateSFXVolume()
    {
    }
}
