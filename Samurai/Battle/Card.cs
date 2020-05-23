using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Numian
{

    public class Card : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField]
        private Word word;
        [SerializeField]
        private Text kanji, romaji, traduction;

        public delegate void Click(Word w);
        public event Click OnClick;

        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void UpdateInfo(Word w, string kanji, string romaji)
        {
            word = w;
            this.kanji.text = kanji;
            this.romaji.text = romaji;
        }

        public void UpdateAnswerInfo(Word word, string traduction)
        {
            this.word = word;
            this.traduction.text = traduction;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if(OnClick != null)
                OnClick(word);
        }

        public Word GetWord() => word;
    }
}
