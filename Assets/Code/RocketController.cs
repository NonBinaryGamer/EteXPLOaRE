using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class RocketController : MonoBehaviour
{
    private AudioSource audioSource;
    private Rigidbody _rigidbody;
    private ParticleSystem _particleSystem;

    private bool launch_initiated = false;
    private float launch_timer = 0f;
    private System.DateTime _startTime;
    private bool stopTimer = false;

    public GameObject mainMenu;
    public TextMeshProUGUI timeLabel;
    public string saveKey;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        _rigidbody = GetComponent<Rigidbody>();
        _particleSystem = GetComponent<ParticleSystem>();

        _startTime = System.DateTime.Now;
    }

    // Update is called once per frame
    void Update()
    {
        if (launch_initiated)
        {
            launch_timer += Time.deltaTime;

            if (launch_timer >= 2.0f)
            {
                audioSource.Play();
                _particleSystem.Play();
            }

            if (launch_timer >= 3f)
            {
                PlayLaunchSequence();
                launch_initiated = false;

                if (mainMenu != null)
                {
                    mainMenu.SetActive(true);
                    Globals.GAME_ACTIVE = false;
                }
            }
        }
        if (!stopTimer)
        {
            System.DateTime currentTime = System.DateTime.Now;
            TimeSpan playTime = currentTime - _startTime;
            timeLabel.text = Math.Floor(playTime.TotalHours).ToString("0") + ":" + Mathf.Floor(playTime.Minutes).ToString("00") + ":" + Mathf.Floor(playTime.Seconds).ToString("00");
        }
    }

    private void PlayLaunchSequence()
    {
        _rigidbody.AddForce(new Vector3(0, 750, 0), ForceMode.Impulse);

    }

    public void Launch()
    {
        launch_initiated = true;
        System.DateTime endTime = System.DateTime.Now;
        TimeSpan playTime = endTime - _startTime;
        float currentRecord = PlayerPrefs.GetFloat(saveKey, float.MaxValue);
        Debug.Log("New record: " + playTime.TotalSeconds + " Current Record: " + currentRecord);
        stopTimer = true;
        if (playTime.TotalSeconds < currentRecord)
        {
            PlayerPrefs.SetFloat(saveKey, (float)playTime.TotalSeconds);
        }

        // PB:
        // Tutorial: 0:49
        // Dusty: 4:13
    }
}
