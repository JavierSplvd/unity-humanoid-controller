﻿using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Numian
{
    [Serializable]
    public class WordDictionary
    {
        private List<Word> words;
        private List<string> kanjis;
        private List<string> latinWords;
        private List<string> traductions;
        private List<int> failures;
        private List<int> success;
        public int level;

        public List<T> Shuffle<T>(IList<T> list)
        {
            List<T> newList = new List<T>(list);
            int n = newList.Count;
            while (n > 1)
            {
                n--;
                int k = (int)UnityEngine.Random.Range(0, n - 0.00001f);
                T value = newList[k];
                newList[k] = newList[n];
                newList[n] = value;
            }
            return newList;
        }
        public WordDictionary()
        {
            Init(1);
        }
        // https://en.wikibooks.org/wiki/JLPT_Guide/JLPT_N5_Kanji
        private void Init(int level)
        {
            this.level = level;

            words = new List<Word>();
            kanjis = new List<string>();
            latinWords = new List<string>();
            traductions = new List<string>();
            // 1
            AddWord(Word.One, "一", "ichi", "one");
            AddWord(Word.Two, "二", "ni", "two");
            AddWord(Word.Three, "三", "san", "three");
            AddWord(Word.Four, "四", "shi", "four");
            AddWord(Word.Five, "五", "go", "five");
            AddWord(Word.Six, "六", "roku", "six");
            AddWord(Word.Seven, "七", "shichi", "seven");
            AddWord(Word.Eight, "八", "hachi", "eight");
            AddWord(Word.Nine, "九", "kyuu", "nine");
            AddWord(Word.Ten, "十", "juu", "ten");
            // 2
            AddWord(Word.Hundred, "百", "hyaku", "hundred");
            AddWord(Word.Thousand, "千", "sen", "thousand");
            AddWord(Word.TenThousand, "万", "man", "ten thousand");
            AddWord(Word.Father, "父", "fu, bu", "father");
            AddWord(Word.Mother, "母", "bo, bou", "mother");
            AddWord(Word.Friend, "友", "yuu", "friend");
            AddWord(Word.Woman, "女", "jo, nyo", "woman");
            AddWord(Word.Man, "男", "dan, nan", "man");
            AddWord(Word.Person, "人", "jin", "person");
            AddWord(Word.Child, "子", "shi, su", "child");
            // 3
            AddWord(Word.Sun, "日", "nichi, jitsu", "sun");
            AddWord(Word.Moon, "月", "getsu, gatsu", "moon");
            AddWord(Word.Fire, "火", "ka", "fire");
            AddWord(Word.Water, "水", "sui, mizu", "water");
            AddWord(Word.Tree, "木", "boku, moku", "tree");
            AddWord(Word.Gold, "金", "kin, kon", "gold");
            AddWord(Word.Earth, "土", "do, to", "earth");
            AddWord(Word.Book, "本", "hon", "book");
            AddWord(Word.Rest, "休", "kyuu, yasu", "rest");
            AddWord(Word.Talk,"話","hanashi","talk");
            // 4
            AddWord(Word.Year,"年","toshi","year");
            AddWord(Word.Noon,"午","hiru","noon");
            AddWord(Word.Before,"前","zen","before; in front of; previous");
            AddWord(Word.After,"後","ato","behind; after");
            AddWord(Word.Time,"時","toki","time; hour counter");
            AddWord(Word.Interval,"間","aida","interval");
            AddWord(Word.Every,"毎","mai","every");
            AddWord(Word.Previous,"先","saki","previous");
            AddWord(Word.Now,"今","ima","now");
            AddWord(Word.What,"何","nani","what");
            // 5
            AddWord(Word.Top,"上","ue","top");
            AddWord(Word.Bottom,"下","shita","bottom");
            AddWord(Word.Left,"左","hidari","left");
            AddWord(Word.Right,"右","migi","right");
            AddWord(Word.North,"北","kita","north");
            AddWord(Word.South,"南","minami","south");
            AddWord(Word.East,"東","azuma","east");
            AddWord(Word.West,"西","nishi","west");
            AddWord(Word.Outside,"外","soto","outside");
            AddWord(Word.Name,"名","na","name");
            // 6
            AddWord(Word.High,"高","kou","high");
            AddWord(Word.Small,"小","shou","small");
            AddWord(Word.Middle,"中","naka","middle");
            AddWord(Word.Great,"大","oo","great");
            AddWord(Word.Long,"長","take","long");
            AddWord(Word.Half,"半","han","half");
            AddWord(Word.Part,"分","fun","part");
            AddWord(Word.Learning,"学","gaku","learning");
            AddWord(Word.School,"校","kou","school");
            AddWord(Word.Birth,"生","ubu","birth");
            // 7
            AddWord(Word.Mountain,"山","yama","mountain");
            AddWord(Word.River,"川","kawa","river");
            AddWord(Word.White,"白","shiro","white");
            AddWord(Word.Heaven,"天","ten","heaven");
            AddWord(Word.Rain,"雨","ame","rain");
            AddWord(Word.Electricity,"電","inazuma","electricity");
            AddWord(Word.Spirit,"気","ki","spirit");
            AddWord(Word.Wheel,"車","kuruma","wheel; vehicle; car");
            AddWord(Word.Country,"国","kuni","country");
            AddWord(Word.Circle,"円","en","circle; yen (money)");
            // 8
            AddWord(Word.Hear,"聞","","to hear");
            AddWord(Word.Eat,"食","shoku","eating");
            AddWord(Word.Read,"読","aidoku","to read");
            AddWord(Word.Come,"来","rai","to come");
            AddWord(Word.Book,"書","sho","document, book");
            AddWord(Word.Show,"見","mi","to show");
            AddWord(Word.Go,"行","kou","to go");
            AddWord(Word.GoOut,"出","de","to go out");
            AddWord(Word.Enter,"入","iri","enter");
            AddWord(Word.Meet,"会","kai","to meet");

            failures = new List<int>(new int[words.Count]);
            success = new List<int>(new int[words.Count]);
        }

        private void AddWord(Word word, string kanji, string latin, string traduction)
        {
            words.Add(word);
            kanjis.Add(kanji);
            latinWords.Add(latin);
            traductions.Add(traduction);
        }

        private List<Word> WordsByDifficulty()
        {
            // limit the words depending on the level
            return words.Take(level * 10).ToList();
        }

        public Word GetRandomWord()
        {
            return words[UnityEngine.Random.Range(0, WordsByDifficulty().Count)];
        }

        public List<Word> GetThreeRandomWordsFor(Word w)
        {
            // limit the words depending on the level
            List<Word> poolOfWords = WordsByDifficulty();

            List<Word> shuffleWords = Shuffle(poolOfWords);
            Debug.Log("Level:" +level);
            Debug.Log(String.Join("///", words));
            shuffleWords.Remove(w);

            // randomized selection of three words
            List<Word> randomWords = new List<Word>();
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
        public void AddFailure(Word w)
        {
            int index = words.IndexOf(w);
            failures[index] += 1;
        }
        public void AddSuccess(Word w)
        {
            int index = words.IndexOf(w);
            success[index] += 1;
        }

        public List<int> GetFailures() => failures;
        public List<Word> GetWords() => words;
        public List<KeyValuePair<string, int>> GetFailuresAndKanjis()
        {
            List<KeyValuePair<string, int>> l = new List<KeyValuePair<string, int>>();
            foreach(Word w in words)
            {
                int index = words.IndexOf(w);
                l.Add(new KeyValuePair<string, int>(kanjis[index], failures[index]));
            }
            return l;
        }

        public void SetLevel(int level) => this.level = level;
    }
    public class WordStatAggregator
    {
        private WordDictionary wordDictionary;
        private CardController cardController;
        private List<KeyValuePair<Word, int>> topFailuresWords;
        private List<int> topFailuresCount;
        public WordStatAggregator(WordDictionary wordDictionary, CardController cardController)
        {
            this.wordDictionary = wordDictionary;
            this.cardController = cardController;
        }

        public List<KeyValuePair<string, int>> GetTopFiveFailures()
        {
            List<KeyValuePair<string, int>> failuresAndKanjis = wordDictionary.GetFailuresAndKanjis();
            failuresAndKanjis = failuresAndKanjis.OrderByDescending(x => x.Value).Take(5).ToList();
            Debug.Log(String.Join("///", failuresAndKanjis));
            return failuresAndKanjis;
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
        private WordStatAggregator wordStats;

        [SerializeField]
        private Vector2 originalPos, currentPos;
        [SerializeField]
        private Spring verticalPosSpring;
        private RectTransform rectTransform;

        // Stats
        [SerializeField]
        private Transform row1, row2, row3, row4, row5;

        public delegate void CorrectAnswer();
        public event CorrectAnswer OnCorrectAnswer;
        public delegate void WrongAnswer();
        public event WrongAnswer OnWrongAnswer;
        public delegate void UpdateFailures(List<KeyValuePair<string, int>> l);
        public event UpdateFailures OnUpdateFailures;
        // Start is called before the first frame update
        void Start()
        {
            rectTransform = GetComponent<RectTransform>();
            originalPos = rectTransform.localPosition;
            currentPos = rectTransform.localPosition;
            verticalPosSpring = new Spring(100, 1, originalPos.y);
            // dictionary related stuff
            dictionary = Savegame.LoadDictionary();
            DifficultyController difficulty = GameObject
                .FindGameObjectWithTag(GameObjectTags.DifficultyController.ToString())
            .GetComponent<DifficultyController>();
            dictionary.SetLevel((int) difficulty.GetLevel());
            wordStats = new WordStatAggregator(dictionary, this);
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
            controller.GetStateMachine().OnPlayerCleanup += Save;
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
        }

        void FixedUpdate()
        {
            verticalPosSpring.FixedUpdate(Time.fixedDeltaTime);
        }

        void ShuffleCards()
        {
            Word targetWord = dictionary.GetRandomWord();
            List<Word> possibleAnswers = dictionary.GetThreeRandomWordsFor(targetWord);

            question.UpdateInfo(targetWord, dictionary.GetKanji(targetWord), dictionary.GetLatin(targetWord));
            for (int i = 0; i < 3; i++)
            {
                answers[i].UpdateAnswerInfo(possibleAnswers[i], dictionary.GetTraduction(possibleAnswers[i]));
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
                {
                    OnCorrectAnswer();
                    dictionary.AddSuccess(answer);
                }
            }
            else
            {
                controller.EnemyAttacks();
                if (OnWrongAnswer != null)
                {
                    OnWrongAnswer();
                    dictionary.AddFailure(answer);
                }
            }
            controller.NextState();
        }

        public void GetFailures()
        {
            if(OnUpdateFailures != null)
                OnUpdateFailures(wordStats.GetTopFiveFailures());
        }

        private void Save()
        {
            Savegame.SaveDictionary(dictionary);
        }
    }
}