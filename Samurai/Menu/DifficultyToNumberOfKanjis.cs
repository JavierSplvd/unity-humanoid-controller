using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Numian
{
    public class DifficultyToNumberOfKanjis : MonoBehaviour
    {
        public Text text;

        void Start()
        {
            text = GetComponent<Text>();
            UpdateValue(1);
        }
        public void UpdateValue(float value)
        {
            text.text = (value * 10).ToString() + " kanjis";
        }
    }
}
