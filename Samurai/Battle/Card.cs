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
        private Text text;

        public delegate void Click(Word w);
        public event Click OnClick;

        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void UpdateInfo(Word w, string s)
        {
            word = w;
            text.text = s;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if(OnClick != null)
                OnClick(word);
            Debug.Log("Click on " + word);
        }

        public Word GetWord() => word;
    }
}
