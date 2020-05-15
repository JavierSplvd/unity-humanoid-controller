using System;
using UnityEngine;

namespace Numian
{
    public class BattleCharacterController : MonoBehaviour, DamageableInterface
    {
        [SerializeField]
        private Numian.CharacterPreset characterPreset;
        [SerializeField]
        private CharacterData data;
        private Animator animator;
        [SerializeField]
        private bool horizontalMovement = true;
        [SerializeField]
        private bool hasMoved, isGuarding;
        [SerializeField]
        private DoDamage weapon;
        [SerializeField]
        private Stances currentStance = Stances.MiddleStance;
        [SerializeField]
        private Vector3 steerDirection;

        public delegate void PlayerHasAttackedEvent();
        public event PlayerHasAttackedEvent OnCharacterHasAttacked;

        public delegate void CharacterHasRested();
        public event CharacterHasRested OnCharacterHasRested;

        void Start()
        {
            data = CharacterDataFactory.GetData(characterPreset);
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
            //HorizontalMove();
            Steer();
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

        private void Steer()
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.FromToRotation(Vector3.forward, steerDirection), Time.deltaTime * 0f);
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
            currentStance = Stances.HighStance;
            DoAttackStance("AirAttack");
        }
        public void AttackMountainStance()
        {
            currentStance = Stances.MiddleStance;
            DoAttackStance("MountainAttack");
        }
        public void AttackSeaStance()
        {
            currentStance = Stances.LowStance;
            DoAttackStance("SeaAttack");
        }

        private void DoAttackStance(string s)
        {
            weapon.SetDamageActive();
            data.currentStamina -= 1;
            animator.Play(s);
            if(OnCharacterHasAttacked != null)
                OnCharacterHasAttacked();
        }
        public void Rest()
        {
            data.currentStamina = data.maxStamina;
            if(OnCharacterHasRested != null)
                OnCharacterHasRested();
        }
        public CharacterData GetData() => data;
        public Stances GetStance() => currentStance;
        public bool HasMoved() => hasMoved;
        public void ResetMove() => hasMoved = false;
        public void CantMove() => hasMoved = true;

        public void ReceiveDamage(int attackValue)
        {
            if(isGuarding)
                attackValue = attackValue / 2;
                ConsumeGuardStamina();
            data.currentHealth -= attackValue;
        }

        private void ConsumeGuardStamina()
        {
            data.currentStamina -= 1;
        }

        public void ReceiveDamage(AttackData data)
        {
            int damage = data.GetAttackValue();
            if(IsWeakTowardsStance(data.GetStance()))
                damage = damage * 2;
            ReceiveDamage(damage);
        }

        private bool IsWeakTowardsStance(Stances targetStance)
        {
            if(targetStance.Equals(Stances.HighStance) && currentStance.Equals(Stances.MiddleStance))
                return true;
            if(targetStance.Equals(Stances.MiddleStance) && currentStance.Equals(Stances.LowStance))
                return true;
            if(targetStance.Equals(Stances.LowStance) && currentStance.Equals(Stances.HighStance))
                return true;
            return false;
        }
    }
}