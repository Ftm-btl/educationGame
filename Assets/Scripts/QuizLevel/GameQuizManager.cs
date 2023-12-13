using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameQuizManager : MonoBehaviour
{
#pragma warning disable 649
    // QuizGameUI scriptine referans
    [SerializeField] private QuizGameUI quizGameUI;
    // ScriptableObject dosyasına referans
    [SerializeField] private List<GameQuizDataScriptable> quizDataList;
    [SerializeField] private float timeInSeconds;
#pragma warning restore 649

    private string currentCategory = "";
    private int correctAnswerCount = 0;
    // soruların verileri
    private List<Question> questions;
    // seçilen sorunun verileri
    private Question selectedQuetion = new Question();
    private int gameScore;
    private int lifesRemaining;
    private float currentTime;
    private GameQuizDataScriptable dataScriptable;

    private QuizGameStatus gameStatus = QuizGameStatus.NEXT;

    public QuizGameStatus GameStatus { get { return gameStatus; } }

    public List<GameQuizDataScriptable> QuizData { get => quizDataList; }

    public void StartGame(int categoryIndex, string category)
    {
        currentCategory = category;
        correctAnswerCount = 0;
        gameScore = 0;
        lifesRemaining = 3;
        currentTime = timeInSeconds;
        // soru verilerini ayarla
        questions = new List<Question>();
        dataScriptable = quizDataList[categoryIndex];
        questions.AddRange(dataScriptable.questions);
        // soruyu seç
        SelectQuestion();
        gameStatus = QuizGameStatus.PLAYING;
    }

    /// <summary>
    /// Soru verilerinden rastgele bir soru seçmek için kullanılan metod
    /// </summary>
    private void SelectQuestion()
    {
        // rastgele bir sayı al
        int val = UnityEngine.Random.Range(0, questions.Count);
        // seçilen soruyu belirle
        selectedQuetion = questions[val];
        // soruyu quizGameUI'ya gönder
        quizGameUI.SetQuestion(selectedQuetion);

        questions.RemoveAt(val);
    }

    private void Update()
    {
        if (gameStatus == QuizGameStatus.PLAYING)
        {
            currentTime -= Time.deltaTime;
            SetTime(currentTime);
        }
    }

    void SetTime(float value)
    {
        TimeSpan time = TimeSpan.FromSeconds(currentTime); // süre değerini ayarla
        quizGameUI.TimerText.text = time.ToString("mm':'ss");   // süreyi Zaman formatına çevir

        if (currentTime <= 0)
        {
            //Game Over
            GameEnd();
        }
    }

    /// <summary>
    /// Cevabın doğru olup olmadığını kontrol etmek için kullanılan metod
    /// </summary>
    /// <param name="selectedOption">answer string</param>
    /// <returns></returns>
    public bool Answer(string selectedOption)
    {
        // varsayılan olarak false olarak ayarla
        bool correct = false;
        // seçilen cevap doğru cevaba benziyorsa
        if (selectedQuetion.correctAns == selectedOption)
        {
            // Evet, Cevap doğru
            correctAnswerCount++;
            correct = true;
            gameScore += 50;
            quizGameUI.ScoreText.text = "Score:" + gameScore;
        }
        else
        {
            // Hayır, Cevap yanlış
            // Canı azalt
            lifesRemaining--;
            quizGameUI.ReduceLife(lifesRemaining);

            if (lifesRemaining == 0)
            {
                GameEnd();
            }
        }

        if (gameStatus == QuizGameStatus.PLAYING)
        {
            if (questions.Count > 0)
            {
                // 1 saniye sonra SelectQuestion metodunu tekrar çağır
                Invoke("SelectQuestion", 0.4f);
            }
            else
            {
                GameEnd();
            }
        }
        // doğru bool değerini döndür
        return correct;
    }

    private void GameEnd()
    {
        gameStatus = QuizGameStatus.NEXT;
        quizGameUI.GameOverPanel.SetActive(true);

        
        PlayerPrefs.SetInt(currentCategory, correctAnswerCount); // bu kategori için skoru kaydet
    }
}

// Soruların verilerini depolamak için kullanılan veri yapısı
[System.Serializable]
public class Question
{
    public string questionInfo;         //soru metni
    public QuestionType questionType;   //tip
    public Sprite questionImage;        //resim için resim
    public AudioClip audioClip;         //ses için ses klibi
    public UnityEngine.Video.VideoClip videoClip;   //video için video klibi
    public List<string> options;        //seçenekler
    public string correctAns;           //doğru seçenek
}

[System.Serializable]
public enum QuestionType
{
    TEXT,
    IMAGE,
    AUDIO,
    VIDEO
}

[SerializeField]
public enum QuizGameStatus
{
    PLAYING,
    NEXT
}