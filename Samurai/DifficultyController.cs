using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Numian
{    
    public class DifficultyController : MonoBehaviour
    {
        [SerializeField]
        private Difficulties level;
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
            Debug.Log("SetLevel" + level);
            List<Difficulties> l = Enum.GetValues(typeof(Difficulties)).Cast<Difficulties>().ToList();
            foreach(Difficulties d in l)
            {
                if(l.IndexOf(d) == level)
                    this.level = l[level];
            }
        } 

        public Difficulties GetLevel() => level;
    }
}
