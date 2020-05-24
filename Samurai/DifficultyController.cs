using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Numian
{    
    public class DifficultyController : MonoBehaviour
    {
        [SerializeField]
        private int level;

        void Awake() {
            DontDestroyOnLoad(this.gameObject);
        }
        
        public void SetLevel(int level) => this.level = level;
        public int GetLevel() => level;
    }
}
