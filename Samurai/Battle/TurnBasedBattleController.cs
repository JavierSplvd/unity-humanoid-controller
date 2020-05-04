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
            if(currentState.Equals(states[0]))
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
        // Start is called before the first frame update
        void Start()
        {
            turnStateMachine = new TurnStateMachine();
        }

        // Update is called once per frame
        void Update()
        {
            switch(turnStateMachine.GetTeam())
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

        private void PlayerTurn()
        {
            if(turnStateMachine.GetState().Equals(BattleStates.EarlyMove)){
                ActivatePlayerMovement();
            }
            else if(turnStateMachine.GetState().Equals(BattleStates.Action)){
                ActivateActionSelection();
            }
            else if(turnStateMachine.GetState().Equals(BattleStates.LateMove)){
                ActivatePlayerMovement();
            }
            else if(turnStateMachine.GetState().Equals(BattleStates.Cleanup)){
                
            }
        }

        private void ActivateActionSelection()
        {
            throw new NotImplementedException();
        }

        private void ActivatePlayerMovement()
        {
            throw new NotImplementedException();
        }

        private void EnemyTurn()
        {

        }

        public void NextState()
        {
            turnStateMachine.NextState();
        }
    }
}

