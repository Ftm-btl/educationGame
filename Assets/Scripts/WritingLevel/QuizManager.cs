using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class QuizManager : MonoBehaviour
{
    public static QuizManager instance; //Instance to make is available in other scripts without reference

    [SerializeField] private GameObject gameComplete;
    [SerializeField] private Text progressText;                 // Text element to show the progress
    [SerializeField] private QuizDataScriptable questionDataScriptable;
    [SerializeField] private Image questionImage;
    [SerializeField] private WordData[] answerWordList;
    [SerializeField] private WordData[] optionsWordList;

    private GameStatus gameStatus = GameStatus.Playing;
    private char[] wordsArray = new char[12];

    private List<int> selectedWordsIndex;
    private int currentAnswerIndex = 0, currentQuestionIndex = 0;
    private bool correctAnswer = true;//Doğru cevap kontrolü için correctAnswer ve cevap kelimesini tutmak için answerWord değişkenleri tanımlanıyor.
    private string answerWord;

    private int currentQuestionNumber = 1;//O anki soru numarasını ve toplam soru sayısını takip etmek için değişkenleri tanımlanıyor.
    private int totalQuestionCount;

    private void Awake()
    {
        // instance değişkenini kontrol ederek sadece bir örneğin kalmasını sağlıyor. Eğer başka bir örnek varsa, bu betiği yok ediyor.
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        //Toplam soru sayısını, questionDataScriptable içindeki soru listesinin eleman sayısı olarak belirliyoruz.
        totalQuestionCount = questionDataScriptable.questions.Count;

        selectedWordsIndex = new List<int>();
        SetQuestion();
    }

    void SetQuestion()
    {
        gameStatus = GameStatus.Playing;
        //kullanarak şu anki sorunun cevabını ve soru resmini alıyoruz.
        answerWord = questionDataScriptable.questions[currentQuestionIndex].answer;
        questionImage.sprite = questionDataScriptable.questions[currentQuestionIndex].questionImage;

        ResetQuestion();

        selectedWordsIndex.Clear();
        Array.Clear(wordsArray, 0, wordsArray.Length);

        for (int i = 0; i < answerWord.Length; i++)
        {
            wordsArray[i] = char.ToUpper(answerWord[i]);
        }

        for (int j = answerWord.Length; j < wordsArray.Length; j++)
        {
            wordsArray[j] = (char)UnityEngine.Random.Range(65, 90);
        }

        wordsArray = ShuffleList.ShuffleListItems<char>(wordsArray.ToList()).ToArray();

        for (int k = 0; k < optionsWordList.Length; k++)
        {
            optionsWordList[k].SetWord(wordsArray[k]);
        }

        progressText.text = currentQuestionNumber + "/" + totalQuestionCount + " sorudasınız";
    }

    public void ResetQuestion()
    {
        for (int i = 0; i < answerWordList.Length; i++)
        {
            answerWordList[i].gameObject.SetActive(true);
            answerWordList[i].SetWord('_');
        }

        for (int i = answerWord.Length; i < answerWordList.Length; i++)
        {
            answerWordList[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < optionsWordList.Length; i++)
        {
            optionsWordList[i].gameObject.SetActive(true);
        }

        currentAnswerIndex = 0;
    }

    public void SelectedOption(WordData value)
    {
        if (gameStatus == GameStatus.Next || currentAnswerIndex >= answerWord.Length) return;

        selectedWordsIndex.Add(value.transform.GetSiblingIndex());
        value.gameObject.SetActive(false);
        answerWordList[currentAnswerIndex].SetWord(value.wordValue);

        currentAnswerIndex++;

        if (currentAnswerIndex == answerWord.Length)
        {
            correctAnswer = true;
            for (int i = 0; i < answerWord.Length; i++)
            {
                if (char.ToUpper(answerWord[i]) != char.ToUpper(answerWordList[i].wordValue))
                {
                    correctAnswer = false;
                    break;
                }
            }

            if (correctAnswer)
            {
                Debug.Log("Correct Answer");
                gameStatus = GameStatus.Next;
                currentQuestionIndex++;

                if (currentQuestionIndex < questionDataScriptable.questions.Count)
                {
                    currentQuestionNumber++;
                    Invoke("SetQuestion", 0.5f);
                }
                else
                {
                    Debug.Log("Game Complete");
                    gameComplete.SetActive(true);
                }
            }
        }
    }

    public void ResetLastWord()
    {
        //son seçilen kelimenin sıfırlanmasını sağlar.
        if (selectedWordsIndex.Count > 0)
        {
            int index = selectedWordsIndex[selectedWordsIndex.Count - 1];
            optionsWordList[index].gameObject.SetActive(true);
            selectedWordsIndex.RemoveAt(selectedWordsIndex.Count - 1);

            currentAnswerIndex--;
            answerWordList[currentAnswerIndex].SetWord('_');
        }
    }
}


[System.Serializable]
public class QuestionData
{
    public Sprite questionImage;
    public string answer;
}

public enum GameStatus
{
   Next,
   Playing
}
