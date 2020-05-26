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
        private bool isGuarding;
        [SerializeField]
        private DoDamage weapon;
        [SerializeField]
        private Stances currentStance = Stances.MiddleStance;
        [SerializeField]
        private Vector3 steerEuler;
        [SerializeField]
        private Transform restPosition;
        private Spring movementSpring;
        [SerializeField]
        private int nAttacks;
        [SerializeField]
        private Combo combo;
        [SerializeField]
        private float distanceToRestPos;

        public delegate void PlayerHasAttackedEvent();
        public event PlayerHasAttackedEvent OnCharacterHasAttacked;

        public delegate void CharacterHasRested();
        public event CharacterHasRested OnCharacterHasRested;

        public delegate void CharacterIsHurted();
        public event CharacterIsHurted OnCharacterIsHurted;

        void Start()
        {
            movementSpring = new Spring(15, 1, 0);
            data = CharacterDataFactory.GetData(characterPreset);
            animator = GetComponent<Animator>();
            if (weapon == null)
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
            Steer();
            CenterForward();
            if (data.currentHealth <= 0)
                animator.SetTrigger("die");
            animator.SetFloat("forward", movementSpring.GetX());
            movementSpring.Update(Time.deltaTime);
            
        }

        public void MoveRestPosition()
        {
            if (DistanceToTarget() > 0.3f)
            {
                float x = transform.forward.x * (restPosition.position.x - transform.position.x);
                if (x > 0)
                    x = 1;
                else
                    x = -1;
                movementSpring.SetX0(x);
            }
            else
            {
                movementSpring.SetX0(0f);
            }

        }

        private float DistanceToTarget()
        {
            // Ignoring height
            Vector3 restPositionSanitized = restPosition.position;
            restPositionSanitized.y = 0;
            Vector3 thisPosition = transform.position;
            thisPosition.y = 0;
            distanceToRestPos = Vector3.Distance(restPositionSanitized, thisPosition);
            return distanceToRestPos;
        }

        private void Steer()
        {
            transform.rotation = Quaternion
                .RotateTowards(transform.rotation, Quaternion.Euler(steerEuler.x, steerEuler.y, steerEuler.z), Time.deltaTime * 0.2f);
        }

        private void CenterForward()
        {
            Vector3 curatedPos = transform.position;
            curatedPos.z = 0;
            transform.position = Vector3.Lerp(transform.position, curatedPos, Time.deltaTime * 0.3f);
        }

        public void Attack()
        {
            int n;
            if(combo != null)
                n = combo.GetCount();
            else
                n = nAttacks;
            int randomNumber = (int)UnityEngine.Random.Range(0, (float) n);
            Debug.Log(randomNumber + "/" + n);
            switch (randomNumber)
            {
                case 0: case 1:
                    DoAttackStance("A1");
                    break;
                case 2: case 3:
                    DoAttackStance("A2");
                    break;
                case 4: case 5:
                    DoAttackStance("A3");
                    break;
                case 6: case 7:
                    DoAttackStance("C2A1");
                    break;
                default:
                    DoAttackStance("C1A1");
                    break;

            }
        }

        private void DoAttackStance(string s)
        {
            weapon.SetDamageActive(true);
            data.currentStamina -= 1;
            animator.Play(s);
            if (OnCharacterHasAttacked != null)
                OnCharacterHasAttacked();

        }
        public void Rest()
        {
            data.currentStamina = data.maxStamina;
            if (OnCharacterHasRested != null)
                OnCharacterHasRested();
        }
        public CharacterData GetData() => data;
        public Stances GetStance() => currentStance;

        public void ReceiveDamage(int attackValue)
        {
            if (isGuarding)
            {
                attackValue = attackValue / 2;
                ConsumeGuardStamina();
            }
            data.currentHealth -= attackValue;
        }

        private void ConsumeGuardStamina()
        {
            data.currentStamina -= 1;
        }

        public void ReceiveDamage(AttackData data)
        {
            int damage = data.GetAttackValue();
            if (IsWeakTowardsStance(data.GetStance()))
                damage = damage * 2;
            ReceiveDamage(damage);
            if (OnCharacterIsHurted != null)
            {
                OnCharacterIsHurted();

            }
            animator.SetTrigger("hit");
        }

        private bool IsWeakTowardsStance(Stances targetStance)
        {
            if (targetStance.Equals(Stances.HighStance) && currentStance.Equals(Stances.MiddleStance))
                return true;
            if (targetStance.Equals(Stances.MiddleStance) && currentStance.Equals(Stances.LowStance))
                return true;
            if (targetStance.Equals(Stances.LowStance) && currentStance.Equals(Stances.HighStance))
                return true;
            return false;
        }

        public bool IsMoving()
        {
            return (int)(10 * animator.GetFloat("forward")) != 0f;
        }

        public void Stop()
        {
            movementSpring.SetX0(0f);
        }
    }
}