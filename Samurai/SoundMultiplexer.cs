using System;
using UnityEngine;

namespace Numian
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundMultiplexer : MonoBehaviour
    {
        [SerializeField]
        private AudioClip[] audios;
        private AudioSource audioSource;
        void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Play()
        {
            int r = (int)UnityEngine.Random.Range(0, audios.Length);
            if (!audioSource.isPlaying)
            {
                audioSource.clip = audios[r];
                audioSource.Play();
            }
        }
    }
}