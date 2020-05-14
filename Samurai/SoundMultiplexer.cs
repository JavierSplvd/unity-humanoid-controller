using System;
using UnityEngine;

namespace Numian
{
    public class SoundMultiplexer : MonoBehaviour
    {
        private AudioSource[] audios;
        void Start()
        {
            audios = new AudioSource[transform.childCount];
            for (int i = 0; i < transform.childCount; i++)
            {
                audios[i] = transform.GetChild(i).GetComponent<AudioSource>();
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Play()
        {
            int r = (int)UnityEngine.Random.Range(0, transform.childCount);
            if (!audios[r].isPlaying)
            {
                audios[r].Play();
            }
        }
    }
}