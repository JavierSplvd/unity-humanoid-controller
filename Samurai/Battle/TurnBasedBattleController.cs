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
        private BattleCharacterController playerCharController;
        [SerializeField]
        private GameObject[] actionButtons;
        // Start is called before the first frame update
        void Start()
        {
            turnStateMachine = new TurnStateMachine();
            actionButtons = GameObject.FindGameObjectsWithTag("actionButton");
            CleanWorld();
            UpdateWorld();
        }

        void UpdateWorld()
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
            DeactivatePlayerMovement();
            DeactivateActionSelection();
        }

        private void PlayerTurn()
        {
            if (turnStateMachine.GetState().Equals(BattleStates.Upkeep))
            {
                ResetPlayer();
            }
            else if (turnStateMachine.GetState().Equals(BattleStates.EarlyMove))
            {
                ActivatePlayerMovement();                
            }
            else if (turnStateMachine.GetState().Equals(BattleStates.Action))
            {
                ActivateActionSelection();
            }
            else if (turnStateMachine.GetState().Equals(BattleStates.LateMove))
            {
                if(playerCharController.HasMoved())
                    NextState();
                else
                    ActivatePlayerMovement();
            }
            else if (turnStateMachine.GetState().Equals(BattleStates.Cleanup))
            {

            }
        }

        private void ActivateActionSelection()
        {
            foreach (GameObject g in actionButtons)
            {
                g.SetActive(true);
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
            playerCharController.SetHorizontalMovement(true);
        }

        private void DeactivatePlayerMovement()
        {
            playerCharController.SetHorizontalMovement(false);
        }

        private void ResetPlayer()
        {
            playerCharController.ResetMove();
        }

        private void EnemyTurn()
        {

        }

        public void NextState()
        {
            turnStateMachine.NextState();
            CleanWorld();
            UpdateWorld();
        }
    }
}

