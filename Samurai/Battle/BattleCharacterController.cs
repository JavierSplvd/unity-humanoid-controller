using UnityEngine;

namespace Numian
{
    public class BattleCharacterController : MonoBehaviour, DamageableInterface
    {
        [SerializeField]
        private CharacterData data;
        private Animator animator;
        [SerializeField]
        private bool horizontalMovement = true;
        [SerializeField]
        private bool hasMoved;
        [SerializeField]
        private DoDamage weapon;

        void Start()
        {
            data = new CharacterDataBuilder()
                .WithBaseAttack(10)
                .WithBaseDefense(10)
                .WithMaxHealth(100)
                .WithMaxStamina(5)
                .WithTeam(Teams.Player)
            .Build();

            animator = GetComponent<Animator>();
            if(weapon == null)
            {
                throw new MissingReferenceException("Missing weapon to do damage.");
            }
            else
            {
                weapon.SetDamageValue(data.baseAttack);
            }
        }

        void Update()
        {
            HorizontalMove();
            SteerToWorldRight();
            CenterForward();
        }

        private void HorizontalMove()
        {
            // Only run this if the character is walking or idle, that way
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Walk") || animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                float h = Input.GetAxis("Horizontal");
                animator.SetFloat("forward", h);
                if (Mathf.Abs(h) > 0.1 && horizontalMovement)
                {                    
                    animator.Play("Walk");
                    hasMoved = true;
                }
                else
                {
                    animator.Play("Idle");
                }

            }
        }

        private void SteerToWorldRight()
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0,90,0), Time.deltaTime * 30f);
        }

        private void CenterForward()
        {
            Vector3 curatedPos = transform.position;
            curatedPos.z = 0;
            transform.position = Vector3.Lerp(transform.position, curatedPos, Time.deltaTime * 0.3f);
        }

        public void SetHorizontalMovement(bool b)
        {
            horizontalMovement = b;
        }

        public void AttackAirStance()
        {
            DoAttackStance("AirAttack");
        }
        public void AttackMountainStance()
        {
            DoAttackStance("MountainAttack");
        }
        public void AttackSeaStance()
        {
            DoAttackStance("SeaAttack");
        }

        private void DoAttackStance(string s)
        {
            weapon.SetDamageActive();
            data.currentStamina -= 1;
            animator.Play(s);
        }
        public void Rest()
        {
            data.currentStamina = data.maxStamina;
        }
        public CharacterData GetData() => data;
        public bool HasMoved() => hasMoved;
        public void ResetMove() => hasMoved = false;

        public void ReceiveDamage(int attackValue)
        {
            data.currentHealth -= 1;
        }
    }
}