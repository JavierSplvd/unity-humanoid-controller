using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Numian
{
    public class Combo : MonoBehaviour
    {
        [SerializeField]
        private BattleCharacterController player, enemy;
        
        private int comboCount;

        public delegate void ComboChange();
        public event ComboChange OnComboChange;
        // Start is called before the first frame update
        void Start()
        {
            player.OnCharacterHasAttacked += AddCounter;
            enemy.OnCharacterHasAttacked += ReduceCounter;
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        void AddCounter()
        {
            comboCount += 1;
            FireEvent();
        }
        void ReduceCounter()
        {
            comboCount -= 1;
            if(comboCount <= 0)
            {
                comboCount = 0;
            }
            FireEvent();
        }

        void FireEvent()
        {
            if(OnComboChange != null)
                OnComboChange();
        }

        public int GetCount() => comboCount;
    }

}