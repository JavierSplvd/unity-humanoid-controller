using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Numian
{
    [RequireComponent(typeof(SoundMultiplexer))]
    public class SwordSoundTrigger : MonoBehaviour
    {
        public SwordVelocity swordVelocity;
        private SoundMultiplexer soundMultiplexer;
        void Start()
        {
            soundMultiplexer = GetComponent<SoundMultiplexer>();
            swordVelocity.OnSwordIsQuick += soundMultiplexer.Play;

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}