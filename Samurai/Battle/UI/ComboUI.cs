using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Numian
{
    [RequireComponent(typeof(Text))]
    public class ComboUI : MonoBehaviour
    {
        [SerializeField]
        private Combo combo;
        [SerializeField]
        private Image[] katanaIcons;
        private Text comboText;
        // Start is called before the first frame update
        void Start()
        {
            combo.OnComboChange += UpdateText;
            comboText = GetComponent<Text>();
            comboText.text = "";
            UpdateText();
        }

        // Update is called once per frame
        void UpdateText()
        {
            int count = combo.GetCount();
            if(count == 0)
            {
                comboText.text = "";
            }
            else
            {
                comboText.text = "Combo x" + count.ToString();
            }

            Color color;
            for(int i = 0; i < katanaIcons.Length; i++)
            {
                color = katanaIcons[i].color;
                if(i<count)
                    color.a = 1;
                else
                    color.a = 0;
                katanaIcons[i].color = color;
            }
        }
    }
}