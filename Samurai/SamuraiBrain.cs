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
        private int attackCounter = 0;
        void Start()
        {
            animator = GetComponent<Animator>();
            target = GameObject
                .FindGameObjectWithTag(GameObjectTags.PlayerTeam.ToString());
            controller = GameObject
                .FindGameObjectWithTag(GameObjectTags.BattleController.ToString())
            .GetComponent<TurnBasedBattleController>();
            character = GetComponent<BattleCharacterController>();
            controller.GetStateMachine().OnEnemyUpkeep += Upkeep;
            controller.GetStateMachine().OnEnemyEarlyMove += EarlyMove;
            controller.GetStateMachine().OnEnemyAction += Attack;
            controller.GetStateMachine().OnEnemyLateMove += LateMove;
            controller.GetStateMachine().OnEnemyCleanup += Cleanup;

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
            if (attackCounter < 3)
                character.AttackMountainStance();
            else if (attackCounter < 6)
                character.AttackAirStance();
            else if (attackCounter < 9)
                character.AttackSeaStance();
            if(attackCounter >= 9)
                attackCounter = 0;
            else 
                attackCounter++;
            controller.NextState();
        }

        private void EarlyMove()
        {
            if (DistanceToTarget() > 1.8f)
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
            controller.NextState();
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
