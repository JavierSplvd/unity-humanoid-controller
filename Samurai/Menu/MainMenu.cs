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
            SceneManager.LoadSceneAsync((int) Scenes.Battle);
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
