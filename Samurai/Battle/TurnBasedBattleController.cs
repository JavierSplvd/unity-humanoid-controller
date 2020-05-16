using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Numian
{
    [Serializable]
    public class TurnStateMachine
    {
        [SerializeField]
        private Teams currentTeam;
        [SerializeField]
        private BattleStates currentState;

        private List<Teams> teams;
        private List<BattleStates> states;


        public delegate void EnemyUpkeep();
        public event EnemyUpkeep OnEnemyUpkeep;
        public delegate void EnemyEarlyMove();
        public event EnemyEarlyMove OnEnemyEarlyMove;
        public delegate void EnemyAttack();
        public event EnemyAttack OnEnemyAction;
        public delegate void EnemyLateMove();
        public event EnemyLateMove OnEnemyLateMove;
        public delegate void EnemyCleanup();
        public event EnemyCleanup OnEnemyCleanup;

        public delegate void PlayerAction();
        public event PlayerAction OnPlayerAction;
        public delegate void PlayerLateMovement();
        public event PlayerLateMovement OnPlayerLateMovement;
        public delegate void PlayerUpkeep();
        public event PlayerUpkeep OnPlayerUpkeep;

        public TurnStateMachine()
        {
            teams = new List<Teams>();
            teams.Add(Teams.Player);
            teams.Add(Teams.Enemy);
            states = new List<BattleStates>();
            states.Add(BattleStates.Upkeep);
            states.Add(BattleStates.EarlyMove);
            states.Add(BattleStates.Action);
            states.Add(BattleStates.LateMove);
            states.Add(BattleStates.Cleanup);
            currentState = states[0];
            currentTeam = teams[0];
        }

        public void NextState()
        {
            currentState = NextOnTheList(currentState, states);
            if (currentState.Equals(states[0]))
                NextTeam();

            if (currentTeam.Equals(Teams.Player))
            {
                if (currentState.Equals(BattleStates.Upkeep) && OnPlayerUpkeep != null)
                {
                    OnPlayerUpkeep();
                }
                if (currentState.Equals(BattleStates.LateMove) && OnPlayerLateMovement != null)
                {
                    OnPlayerLateMovement();
                }
                if (currentState.Equals(BattleStates.Action) && OnPlayerAction != null)
                {
                    OnPlayerAction();
                }
            }

            if (currentTeam.Equals(Teams.Enemy))
            {
                if (currentState.Equals(BattleStates.Upkeep) && OnEnemyUpkeep != null)
                {
                    OnEnemyUpkeep();
                }
                if (currentState.Equals(BattleStates.EarlyMove) && OnEnemyEarlyMove != null)
                {
                    OnEnemyEarlyMove();
                }
                if (currentState.Equals(BattleStates.Action) && OnEnemyAction != null)
                {
                    OnEnemyAction();
                }
                if (currentState.Equals(BattleStates.LateMove) && OnEnemyLateMove != null)
                {
                    OnEnemyLateMove();
                }
                if (currentState.Equals(BattleStates.Cleanup) && OnEnemyCleanup != null)
                {
                    OnEnemyCleanup();
                }
            }
        }

        public void NextTeam()
        {
            currentTeam = NextOnTheList(currentTeam, teams);
            currentState = states[0]; // reset state
        }


        private T NextOnTheList<T>(T item, List<T> list)
        {
            int i = list.IndexOf(item);
            if (i + 1 < list.Count)
                return list[i + 1];
            else
                return list[0];
        }

        public Teams GetTeam()
        {
            return currentTeam;
        }

        public BattleStates GetState()
        {
            return currentState;
        }


    }
    public class TurnBasedBattleController : MonoBehaviour
    {
        [SerializeField]
        private TurnStateMachine turnStateMachine;
        [SerializeField]
        private BattleCharacterController playerCharController, enemyCharController;
        [SerializeField]
        private GameObject[] actionButtons;
        private Cooldown upkeepCooldown;
        private Cooldown movementCooldown;


        // Start is called before the first frame update
        void Start()
        {
            turnStateMachine = new TurnStateMachine();
            actionButtons = GameObject.FindGameObjectsWithTag("actionButton");
            CleanWorld();
            DoDuringTurns();
            movementCooldown = new SimpleCooldown(0.4f);

            turnStateMachine.OnEnemyEarlyMove += ResetMovementCooldown;
            turnStateMachine.OnEnemyLateMove += ResetMovementCooldown;
        }

        void Update()
        {
            DoDuringTurns();
            movementCooldown.Update();
        }

        void DoDuringTurns()
        {
            switch (turnStateMachine.GetTeam())
            {
                case Teams.Player:
                    PlayerTurn();
                    break;
                case Teams.Enemy:
                    EnemyTurn();
                    break;
                default:
                    Debug.LogWarning("Team not implemented.");
                    break;
            }
        }

        private void CleanWorld()
        {
            DeactivateActionSelection();
        }

        private void PlayerTurn()
        {
            if (turnStateMachine.GetState().Equals(BattleStates.Upkeep))
            {
                // On enter
                if (upkeepCooldown == null)
                {
                    upkeepCooldown = new SimpleCooldown(0.3f);
                    ResetPlayer();
                    upkeepCooldown.Heat();
                }
                // During
                upkeepCooldown.Update();
                // On exit
                if (upkeepCooldown.IsAvailable())
                {
                    upkeepCooldown = null;
                    NextState();
                }
            }
            else if (turnStateMachine.GetState().Equals(BattleStates.EarlyMove))
            {
                ActivatePlayerMovement();
                ActivateEnemyMovement();
                NextStateIfStatic();
            }
            else if (turnStateMachine.GetState().Equals(BattleStates.Action))
            {
                ActivateActionSelection();

            }
            else if (turnStateMachine.GetState().Equals(BattleStates.LateMove))
            {
                ActivatePlayerMovement();
                ActivateEnemyMovement();
                NextStateIfStatic();
            }
            else if (turnStateMachine.GetState().Equals(BattleStates.Cleanup))
            {
                NextState();
            }
        }

        private void NextStateIfStatic()
        {
            Debug.Log("Move player: "+playerCharController.IsMoving());
            Debug.Log("Move enemy: "+enemyCharController.IsMoving());
            Debug.Log("Move cooldown: "+movementCooldown.IsAvailable());
            if(!playerCharController.IsMoving() && !enemyCharController.IsMoving() && movementCooldown.IsAvailable())
                NextState();   
        }

        private void ActivateActionSelection()
        {
            if (playerCharController.GetData().currentStamina == 0)
            {
                actionButtons[actionButtons.Length - 1].SetActive(true);
            }
            else
            {
                foreach (GameObject g in actionButtons)
                {
                    g.SetActive(true);
                }
            }

        }
        private void DeactivateActionSelection()
        {
            foreach (GameObject g in actionButtons)
            {
                g.SetActive(false);
            }
        }
        private void ActivatePlayerMovement()
        {
            playerCharController.MoveRestPosition();
        }

        private void ActivateEnemyMovement()
        {
            enemyCharController.MoveRestPosition();
        }

        private void ResetPlayer()
        {
            playerCharController.ResetMove();
        }

        private void EnemyTurn()
        {
            NextState();
            if (turnStateMachine.GetState().Equals(BattleStates.Upkeep))
            {

            }
            else if (turnStateMachine.GetState().Equals(BattleStates.EarlyMove))
            {

            }
            else if (turnStateMachine.GetState().Equals(BattleStates.Action))
            {

            }
            else if (turnStateMachine.GetState().Equals(BattleStates.LateMove))
            {

            }
            else if (turnStateMachine.GetState().Equals(BattleStates.Cleanup))
            {

            }
        }
        
        public void PlayerAttacks()
        {
            playerCharController.Attack();
        }
        public void EnemyAttacks()
        {
            enemyCharController.Attack();
        }

        public void NextState()
        {
            turnStateMachine.NextState();
            CleanWorld();
            // UpdateWorld();
        }

        public BattleStates GetState()
        {
            return turnStateMachine.GetState();
        }

        public Teams GetTeam()
        {
            return turnStateMachine.GetTeam();
        }
        public TurnStateMachine GetStateMachine() => turnStateMachine;

        private void ResetMovementCooldown()
        {
            movementCooldown.Heat();
        }
    }
}

