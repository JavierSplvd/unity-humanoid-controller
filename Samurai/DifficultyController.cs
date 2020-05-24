using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Numian
{    
    public class DifficultyController : MonoBehaviour
    {
        [SerializeField]
        private int level;
        [SerializeField]
        private int hash;
        void Awake() {

            int instances = GameObject.FindObjectsOfType<DifficultyController>().Length;
            if(instances != 1)
                Destroy(gameObject);

            if(SceneManager.GetActiveScene().buildIndex == (int) Scenes.Start)
            {
                DontDestroyOnLoad(this.gameObject);
            }


        }

        void Update()
        {
            hash = this.GetHashCode();

        }
        
        public void SetLevel(int level)
        {
            this.level = level;
        } 

        public int GetLevel() => level;
    }
}
