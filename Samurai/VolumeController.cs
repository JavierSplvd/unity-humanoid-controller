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
            controller = GameObject
                .FindGameObjectWithTag(GameObjectTags.BattleController.ToString())
            .GetComponent<TurnBasedBattleController>();
            volume = GetComponent<Volume>();
            volume.profile.TryGet(out vignette);
            controller.GetStateMachine().OnPlayerAction += MaxVignette;
            controller.GetStateMachine().OnPlayerLateMovement += ReduceVignette;
            spring = new Spring(40, 1, minVignette);
        }

        // Update is called once per frame
        void Update()
        {
            vignette.intensity.value = spring.GetX();
            spring.Update(Time.deltaTime);
        }

        private void MaxVignette()
        {
            Debug.Log("max");
            spring.SetX0(maxVignette);
        }

        private void ReduceVignette()
        {
            Debug.Log("min");
            spring.SetX0(minVignette);
        }
    }
}
