using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Numian
{
    [RequireComponent(typeof(Volume))]
    public class VolumeController : MonoBehaviour
    {
        private TurnBasedBattleController controller;
        [SerializeField]
        private float minVignette, maxVignette;
        private float targetVignette;
        private Volume volume;
        private Vignette vignette;
        private Spring spring;
        // Start is called before the first frame update
        void Start()
        {
            Debug.Log("volume");
            spring = new Spring(40, 1, minVignette);

            volume = GetComponent<Volume>();
            volume.profile.TryGet(out vignette);
            controller = GameObject
                .FindGameObjectWithTag(GameObjectTags.BattleController.ToString())
            .GetComponent<TurnBasedBattleController>();
            if(controller != null)
            {
                Debug.LogWarning("TurnBasedBattleController not found in scene.");
                controller.GetStateMachine().OnPlayerAction += MaxVignette;
                controller.GetStateMachine().OnPlayerLateMovement += ReduceVignette;
            
            }
            
        }

        // Update is called once per frame
        void Update()
        {
            vignette.intensity.value = spring.GetX();
        }

        void FixedUpdate()
        {
            spring.FixedUpdate(Time.fixedDeltaTime);
        }

        private void MaxVignette()
        {
            spring.SetX0(maxVignette);
        }

        private void ReduceVignette()
        {
            spring.SetX0(minVignette);
        }
    }
}
