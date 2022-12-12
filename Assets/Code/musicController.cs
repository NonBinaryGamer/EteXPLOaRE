using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class musicController : MonoBehaviour
{
    public AudioClip[] music;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        audioSource.volume = Globals.MUSIC_VOLUME / 100f;

        if (!audioSource.isPlaying) {
            int next_song = Random.Range(0, music.Length);
            audioSource.clip = music[next_song];
            audioSource.Play();
        }
    }
}
