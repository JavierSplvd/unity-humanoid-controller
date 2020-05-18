using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Numian
{
    public class WordDictionary
    {
        private List<Word> words;
        private List<string> kanjis;
        private List<string> latinWords;
        private List<string> traductions;

        private static Random rng = new Random();

        public List<T> Shuffle<T>(IList<T> list)
        {
            List<T> newList = new List<T>(list);
            int n = newList.Count;
            while (n > 1)
            {
                n--;
                int k = (int)Random.Range(0, n - 0.00001f);
                T value = newList[k];
                newList[k] = newList[n];
                newList[n] = value;
            }
            return newList;
        }

        public WordDictionary()
        {
            words = new List<Word>();
            kanjis = new List<string>();
            latinWords = new List<string>();
            traductions = new List<string>();
            AddWord(Word.One, "一", "ICHI", "one");
            AddWord(Word.Two, "二", "NI", "two");
            AddWord(Word.Three, "三", "SAN", "three");
            AddWord(Word.Four, "四", "SHI", "four");
            AddWord(Word.Five, "五", "GO", "five");
            AddWord(Word.Six, "六", "ROKU", "six");
            AddWord(Word.Seven, "七", "SHICHI", "seven");
            AddWord(Word.Eight, "八", "HACHI", "eight");
            AddWord(Word.Nine, "九", "HACHI", "nine");
            AddWord(Word.Ten, "十", "KU", "ten");
            AddWord(Word.Hundred, "百", "", "hundred");
            AddWord(Word.Thousand, "千", "", "thousand");
            AddWord(Word.TenThousand, "万", "", "ten thousand");
            AddWord(Word.Father, "父", "", "father");
            AddWord(Word.Mother, "母", "", "mother");
            AddWord(Word.Friend, "友", "", "friend");
            AddWord(Word.Woman, "女", "", "woman");
            AddWord(Word.Man, "男", "", "man");
            AddWord(Word.Person, "人", "", "person");
            AddWord(Word.Child, "子", "", "child");
        }

        private void AddWord(Word word, string kanji, string latin, string traduction)
        {
            words.Add(word);
            kanjis.Add(kanji);
            latinWords.Add(latin);
            traductions.Add(traduction);
        }

        public Word GetRandomWord()
        {
            return words[Random.Range(0, words.Count)];
        }

        public List<Word> GetThreeRandomWordsFor(Word w)
        {
            List<Word> randomWords = new List<Word>();
            List<Word> shuffleWords = Shuffle(words);
            shuffleWords.Remove(w);
            randomWords.Add(shuffleWords[0]);
            randomWords.Add(shuffleWords[1]);
            randomWords.Add(w);
            return Shuffle(randomWords.ToList());
        }
        public string GetKanji(Word w)
        {
            return kanjis[words.IndexOf(w)];
        }
        public string GetLatin(Word w)
        {
            return latinWords[words.IndexOf(w)];
        }
        public string GetTraduction(Word w)
        {
            return traductions[words.IndexOf(w)];
        }
    }
    public class CardController : MonoBehaviour
    {
        [SerializeField]
        private List<Card> questions, answers;
        public TurnBasedBattleController controller;
        private Card question;
        private Transform questionParent, answerParent;
        private WordDictionary dictionary;

        [SerializeField]
        private Vector2 originalPos, currentPos;
        [SerializeField]
        private Spring verticalPosSpring;
        private RectTransform rectTransform;

        public delegate void CorrectAnswer();
        public event CorrectAnswer OnCorrectAnswer;
        public delegate void WrongAnswer();
        public event WrongAnswer OnWrongAnswer;
        // Start is called before the first frame update
        void Start()
        {
            rectTransform = GetComponent<RectTransform>();
            originalPos = rectTransform.localPosition;
            currentPos = rectTransform.localPosition;
            verticalPosSpring = new Spring(100, 1, originalPos.y);
            dictionary = new WordDictionary();
            // Get parents
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).name.Equals("Question"))
                    questionParent = transform.GetChild(i);
                if (transform.GetChild(i).name.Equals("Answer"))
                    answerParent = transform.GetChild(i);
            }

            // Init questions
            questions = new List<Card>();
            for (int i = 0; i < questionParent.childCount; i++)
            {
                questions.Add(questionParent.GetChild(i).GetComponent<Card>());
            }
            question = questions[0];
            // Init answers
            answers = new List<Card>();
            for (int i = 0; i < answerParent.childCount; i++)
            {
                answers.Add(answerParent.GetChild(i).GetComponent<Card>());
            }
            // Subscribe to events
            controller.GetStateMachine().OnPlayerAction += ShuffleCards;
            controller.GetStateMachine().OnPlayerAction += ShowCards;
            controller.GetStateMachine().OnPlayerUpkeep += HideCards;
            controller.GetStateMachine().OnPlayerLateMovement += HideCards;
            // Subscribe to answers events;
            foreach (Card c in answers)
            {
                c.OnClick += CheckAnswer;
            }
        }

        // Update is called once per frame
        void Update()
        {
            // currentPos.x = transform.position.x;
            currentPos.y = verticalPosSpring.GetX();
            rectTransform.localPosition = currentPos;
            verticalPosSpring.Update(Time.deltaTime);
        }

        void ShuffleCards()
        {
            Word targetWord = dictionary.GetRandomWord();
            List<Word> possibleAnswers = dictionary.GetThreeRandomWordsFor(targetWord);

            question.UpdateInfo(targetWord, dictionary.GetKanji(targetWord));
            for (int i = 0; i < 3; i++)
            {
                answers[i].UpdateInfo(possibleAnswers[i], dictionary.GetTraduction(possibleAnswers[i]));
            }
        }

        void ShowCards()
        {
            verticalPosSpring.SetX0(0);
        }

        void HideCards()
        {
            verticalPosSpring.SetX0(-2000);
        }

        void CheckAnswer(Word answer)
        {
            bool response = answer.Equals(question.GetWord());
            if (response)
            {
                controller.PlayerAttacks();
                if (OnCorrectAnswer != null)
                    OnCorrectAnswer();
            }
            else
            {
                controller.EnemyAttacks();
                if (OnWrongAnswer != null)
                    OnWrongAnswer();
            }
            controller.NextState();
        }
    }
}