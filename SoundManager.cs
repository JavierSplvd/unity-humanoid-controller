﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Numian
{
    public class SoundManager : MonoBehaviour
    {
        private AudioSource audioSource;
        // Start is called before the first frame update
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
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
    }
}