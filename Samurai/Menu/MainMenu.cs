using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Numian
{
    public class MainMenu : MonoBehaviour
    {
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
