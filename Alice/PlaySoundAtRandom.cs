using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundAtRandom : MonoBehaviour
{
    private AudioSource audioSource;
    private bool chance;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        InvokeRepeating("Play", 5f, 5f);
    }

    // Update is called once per frame
    void Play()
    {
        chance = Random.Range(0,2) < 1;
        if(!audioSource.isPlaying && chance)
        {
            audioSource.Play();
        }
    }
}
