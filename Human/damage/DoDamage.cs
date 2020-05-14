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

        void Start()
        {
            cooldown = new SimpleCooldown(0.4f);
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
                damageActive = false;
            }
        }

        public void SetDamageValue(int v)
        {
            damageValue = v;
        }
        public void SetDamageActive()
        {
            damageActive = true;
        }

    }

}