using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketController : MonoBehaviour
{
    private AudioSource audioSource;
    private Rigidbody _rigidbody;
    private ParticleSystem _particleSystem;

    private bool launch_initiated = false;
    private float launch_timer = 0f;

    public GameObject mainMenu;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        _rigidbody = GetComponent<Rigidbody>();
        _particleSystem = GetComponent<ParticleSystem>();
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
                }
            }
        }
    }

    private void PlayLaunchSequence()
    {
        _rigidbody.AddForce(new Vector3(0, 750, 0), ForceMode.Impulse);

    }

    public void Launch()
    {
        launch_initiated = true;
    }
}
