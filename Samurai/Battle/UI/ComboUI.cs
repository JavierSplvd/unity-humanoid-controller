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
        private Text comboText;
        // Start is called before the first frame update
        void Start()
        {
            combo.OnComboChange += UpdateText;
            comboText = GetComponent<Text>();
            comboText.text = "";
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
        }
    }
}