using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Numian
{
    public class MainMenu : MonoBehaviour
    {
        private DifficultyController difficulty;
        private Spring horizontalMenuDisplacement;
        public RectTransform menuParent;

        void Start()
        {
            horizontalMenuDisplacement = new Spring(30, 1, 0);
            difficulty = GameObject.FindGameObjectWithTag(GameObjectTags.DifficultyController.ToString())
                .GetComponent<DifficultyController>();
        }

        void FixedUpdate() 
        {
            Vector3 currentPos = menuParent.localPosition;
            currentPos.x = horizontalMenuDisplacement.GetX();
            menuParent.localPosition = currentPos;
            horizontalMenuDisplacement.FixedUpdate(Time.fixedDeltaTime);    
        }

        public void MoveMenuToStart()
        {
            horizontalMenuDisplacement.SetX0(0);
        }

        public void MoveMenuToSettings()
        {
            horizontalMenuDisplacement.SetX0(-1920);
        }

        public void PlayGame()
        {
            switch(difficulty.GetLevel())
            {
                case Difficulties.D1: case Difficulties.D2: case Difficulties.D3: case Difficulties.D4:
                    SceneManager.LoadSceneAsync((int) Scenes.Town);
                    break;
                case Difficulties.D5: case Difficulties.D6:
                    SceneManager.LoadSceneAsync((int) Scenes.Forest);
                    break;
                case Difficulties.D7: case Difficulties.D8:
                    SceneManager.LoadSceneAsync((int) Scenes.Battlefield);
                    break;
                default:
                    SceneManager.LoadSceneAsync((int) Scenes.Town);
                    break;
            }
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
