using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Numian
{
    public class DifficultySliderToDifficulty : MonoBehaviour
    {
        private DifficultyController difficulty;
        private Slider kanjiSlider;
        void Start()
        {
            difficulty = GameObject.FindGameObjectWithTag(GameObjectTags.DifficultyController.ToString())
                .GetComponent<DifficultyController>();
            kanjiSlider = GetComponent<Slider>();
            kanjiSlider.onValueChanged.AddListener(delegate {ValueChanged();});
        }

        private void ValueChanged()
        {
            difficulty.SetLevel((int) kanjiSlider.value);
        }
    }
}