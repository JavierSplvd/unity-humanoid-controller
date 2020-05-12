using System;
using UnityEngine;

namespace Numian
{
    [Serializable]
    public class CharacterData
    {
        [SerializeField]
        public Teams team;
        [SerializeField]
        public int maxHealth, currentHealth, baseAttack, baseDefense, maxStamina, currentStamina;
    }

    public class CharacterDataBuilder
    {
        private CharacterData data;
        public CharacterDataBuilder()
        {
            data = new CharacterData();
        }
        public CharacterDataBuilder WithTeam(Teams t)
        {
            data.team = t;
            return this;
        }
        public CharacterDataBuilder WithMaxHealth(int h)
        {
            data.maxHealth = h;
            data.currentHealth = h;
            return this;
        }
        public CharacterDataBuilder WithBaseDefense(int d)
        {
            data.baseDefense = d;
            return this;
        }
        public CharacterDataBuilder WithBaseAttack(int a)
        {
            data.baseAttack = a;
            return this;
        }
        public CharacterDataBuilder WithMaxStamina(int a)
        {
            data.maxStamina = a;
            data.currentStamina = a;
            return this;
        }
        public CharacterData Build()
        {
            return data;
        }
        
    }
}