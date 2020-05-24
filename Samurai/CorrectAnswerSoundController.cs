using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Numian
{
    [RequireComponent(typeof(AudioSource))]
    public class CorrectAnswerSoundController : MonoBehaviour
    {
        private CardController cardController;
        private AudioSource source;
        // Start is called before the first frame update
        void Start()
        {
            source = GetComponent<AudioSource>();
            cardController = GameObject.FindGameObjectWithTag(GameObjectTags.CardController.ToString()).GetComponent<CardController>();
            cardController.OnCorrectAnswer += Play;
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void Play()
        {
            if(!source.isPlaying)
                source.Play();
        }
    }
}
