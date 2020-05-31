using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Numian
{
    public class SceneController : MonoBehaviour
    {
        private TurnBasedBattleController controller;
        private Cooldown waitOnVictory = new SimpleCooldown(2.2f);
        // Start is called before the first frame update
        void Start()
        {
            controller = GameObject
                .FindGameObjectWithTag(GameObjectTags.BattleController.ToString())
            .GetComponent<TurnBasedBattleController>();
            controller.OnWin += LoadStartScreen;
            controller.OnDefeat += LoadStartScreen;
            waitOnVictory.Heat();

        }

        // Update is called once per frame
        void Update()
        {
            if(waitOnVictory.IsAvailable())
                SceneManager.LoadSceneAsync((int) Scenes.Start, mode:LoadSceneMode.Single);
        }

        private void LoadStartScreen()
        {
            waitOnVictory.Update();
        }
    }
}