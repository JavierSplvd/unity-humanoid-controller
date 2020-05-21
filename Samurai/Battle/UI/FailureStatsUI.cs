using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Numian
{
    public class FailureStatsUI : MonoBehaviour
    {
        // Stats
        [SerializeField]
        private Transform row1, row2, row3, row4, row5;

        private Text kanji1, kanji2, kanji3, kanji4, kanji5;
        private Text count1, count2, count3, count4, count5;
        private CardController cardController;
        // Start is called before the first frame update
        void Start()
        {
            cardController = GameObject
                .FindGameObjectWithTag(GameObjectTags.CardController.ToString())
            .GetComponent<CardController>();
            kanji1 = row1.GetChild(0).gameObject.GetComponent<Text>();
            kanji2 = row2.GetChild(0).gameObject.GetComponent<Text>();
            kanji3 = row3.GetChild(0).gameObject.GetComponent<Text>();
            kanji4 = row4.GetChild(0).gameObject.GetComponent<Text>();
            kanji5 = row5.GetChild(0).gameObject.GetComponent<Text>();
            
            count1 = row1.GetChild(1).gameObject.GetComponent<Text>();
            count2 = row2.GetChild(1).gameObject.GetComponent<Text>();
            count3 = row3.GetChild(1).gameObject.GetComponent<Text>();
            count4 = row4.GetChild(1).gameObject.GetComponent<Text>();
            count5 = row5.GetChild(1).gameObject.GetComponent<Text>();

            cardController.OnUpdateFailures += UpdateTexts;
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void UpdateTexts(List<KeyValuePair<string, int>> l)
        {
            kanji1.text = l[0].Key;
            count1.text = l[0].Value.ToString();
            
            kanji2.text = l[1].Key;
            count2.text = l[1].Value.ToString();

            kanji3.text = l[2].Key;
            count3.text = l[2].Value.ToString();

            kanji4.text = l[3].Key;
            count4.text = l[3].Value.ToString();

            kanji5.text = l[4].Key;
            count5.text = l[4].Value.ToString();
        }
    }
}