using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Numian
{
    [RequireComponent(typeof(SoundMultiplexer))]
    public class CharacterHurtTrigger : MonoBehaviour
    {
        [SerializeField]
        private BattleCharacterController controller;
        private SoundMultiplexer soundMultiplexer;
        void Start()
        {
            soundMultiplexer = GetComponent<SoundMultiplexer>();
            controller.OnCharacterIsHurted += soundMultiplexer.Play;
        }

    }
}