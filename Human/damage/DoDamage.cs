using System;
using UnityEngine;

namespace Numian
{
    public class DoDamage : MonoBehaviour
    {
        private bool damageActive;
        private int damageValue;
        private Cooldown cooldown;
        [SerializeField]
        private BattleCharacterController controller;
        private SwordVelocity swordVelocity;

        void Start()
        {
            cooldown = new SimpleCooldown(0.1f);
            swordVelocity.OnSwordIsQuick += SetDamageActiveTrue;
            swordVelocity.OnSwordIsSlow += SetDamageActiveFalse;
        }

        private void SetDamageActiveTrue()
        {
            SetDamageActive(true);
        }

        private void SetDamageActiveFalse()
        {
            SetDamageActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            cooldown.Update();
        }

        void OnTriggerEnter(Collider other)
        {

            if (other.tag.Equals("Damageable") && cooldown.IsAvailable() && damageActive)
            {
                other.gameObject.SendMessage("ReceiveDamage", new AttackData(damageValue, controller.GetStance()));
                cooldown.Heat();
            }
        }

        public void SetDamageValue(int v)
        {
            damageValue = v;
        }
        public void SetDamageActive(bool value)
        {
            Debug.Log("damage active: " + value);
            damageActive = value;
        }

    }

}