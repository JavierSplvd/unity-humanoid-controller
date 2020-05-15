using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Numian
{
    [RequireComponent(typeof(BattleCharacterController))]
    [RequireComponent(typeof(Animator))]
    public class SamuraiBrain : MonoBehaviour
    {
        private TurnBasedBattleController controller;
        private BattleCharacterController character;
        private Animator animator;
        private GameObject target;
        private Spring movementSpring;
        void Start()
        {
            animator = GetComponent<Animator>();
            target = GameObject
                .FindGameObjectWithTag(GameObjectTags.PlayerTeam.ToString());
            controller = GameObject
                .FindGameObjectWithTag(GameObjectTags.BattleController.ToString())
            .GetComponent<TurnBasedBattleController>();
            character = GetComponent<BattleCharacterController>();
            controller.OnEnemyUpkeep += Upkeep;
            controller.OnEnemyEarlyMove += EarlyMove;
            controller.OnEnemyAttack += Attack;
            controller.OnEnemyLateMove += LateMove;
            controller.OnEnemyCleanup += Cleanup;

            movementSpring = new Spring(50f, 1, 0);

        }

        private void Cleanup()
        {
            controller.NextState();
        }

        private void LateMove()
        {
            if (DistanceToTarget() < 5f && !character.HasMoved())
            {
                movementSpring.SetX0(-1f);
                character.CantMove();
            }
            else
            {
                movementSpring.SetX0(0f);
                controller.NextState();
            }
        }

        private void Attack()
        {
            character.AttackMountainStance();
            controller.NextState();
        }

        private void EarlyMove()
        {
            if (DistanceToTarget() > 1.5f)
            {
                movementSpring.SetX0(1f);
                character.CantMove();
            }
            else
            {
                movementSpring.SetX0(0f);
                controller.NextState();
            }

        }

        private void Upkeep()
        {
            character.ResetMove();
        }

        private float DistanceToTarget()
        {
            return Vector3.Distance(target.transform.position, transform.position);
        }

        // Update is called once per frame
        void Update()
        {
            movementSpring.Update(Time.deltaTime);
            animator.SetFloat("forward", movementSpring.GetX());

        }
    }
}
