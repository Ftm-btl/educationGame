using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class QuizGameUI : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField] private GameQuizManager quizManager;               // QuizManager scriptine referans
    [SerializeField] private QuizCategoryBtnScript categoryBtnPrefab;
    [SerializeField] private GameObject scrollHolder;
    [SerializeField] private Text scoreText, timerText;
    [SerializeField] private List<Image> lifeImageList;
    [SerializeField] private GameObject gameOverPanel, mainMenu, gamePanel;
    [SerializeField] private Color correctCol, wrongCol, normalCol; //buton renkleri
    [SerializeField] private Image questionImg;                     //görsel soruları göstermek için Image bileşeni
    [SerializeField] private UnityEngine.Video.VideoPlayer questionVideo;   // video soruları göstermek için
    [SerializeField] private AudioSource questionAudio;             //sesli sorular için ses kaynağı
    [SerializeField] private Text questionInfoText;                 //soru metnini göstermek için metin bileşeni
    [SerializeField] private List<Button> options;                  //seçenek butonlarının referansları
#pragma warning restore 649

    private float audioLength;          //sesin uzunluğunu tutmak için değişken
    private Question question;          //mevcut soru verisini tutmak için değişken
    private bool answered = false;      //cevaplandı mı yoksa cevap bekleniyor mu

    public Text TimerText { get => timerText; }                     //get fonksiyonu
    public Text ScoreText { get => scoreText; }                     //getter
    public GameObject GameOverPanel { get => gameOverPanel; }                     //getter

    private void Start()
    {
        // tüm butonlara dinleyici ekleme
        for (int i = 0; i < options.Count; i++)
        {
            Button localBtn = options[i];
            localBtn.onClick.AddListener(() => OnClick(localBtn));
        }

        CreateCategoryButtons();

    }
    /// <summary>
    /// Soruyu ekrana yerleştiren metod
    /// </summary>
    /// <param name="question"></param>
    public void SetQuestion(Question question)
    {
        // soruyu ayarla
        this.question = question;
        // soru tipine bak
        switch (question.questionType)
        {
            case QuestionType.TEXT:
                questionImg.transform.parent.gameObject.SetActive(false);   // görsel alanını deaktive et
                break;
            case QuestionType.IMAGE:
                questionImg.transform.parent.gameObject.SetActive(true);   // görsel alanını aktive et 
                questionVideo.transform.gameObject.SetActive(false);        // videoyu deaktive et
                questionImg.transform.gameObject.SetActive(true);           // görseli aktive et
                questionAudio.transform.gameObject.SetActive(false);       // sesi deaktive et

                questionImg.sprite = question.questionImage;                // görsel resmi ayarla
                break;
            case QuestionType.AUDIO:
                questionVideo.transform.parent.gameObject.SetActive(true);  // görsel alanını aktive et
                questionVideo.transform.gameObject.SetActive(false);        // videoyu deaktive et
                questionImg.transform.gameObject.SetActive(false);          // videoyu deaktive et
                questionAudio.transform.gameObject.SetActive(true);         // sesi aktive et

                audioLength = question.audioClip.length;                   // ses klibini ayarla
                StartCoroutine(PlayAudio());                                // Coroutine'i başlat
                break;
            case QuestionType.VIDEO:
                questionVideo.transform.parent.gameObject.SetActive(true);  // görsel alanını aktive et
                questionVideo.transform.gameObject.SetActive(true);        // videoyu aktive et
                questionImg.transform.gameObject.SetActive(false);          // görseli deaktive et
                questionAudio.transform.gameObject.SetActive(false);        // sesi deaktive et

                questionVideo.clip = question.videoClip;                    // video klibini ayarla
                questionVideo.Play();                                       // videoyu oynat
                break;
        }

        questionInfoText.text = question.questionInfo;                      // soru metnini ayarla

        // seçenekleri karıştır
        List<string> ansOptions = QuizShuffleList.ShuffleListItems<string>(question.options);

        /// seçenek butonlarına ilgili seçenekleri ata
        for (int i = 0; i < options.Count; i++)
        {
            //set the child text
            options[i].GetComponentInChildren<Text>().text = ansOptions[i];
            options[i].name = ansOptions[i];   // butonun adını ayarla
            options[i].image.color = normalCol; // buton rengini normal yap
        }

        answered = false;                       

    }

    public void ReduceLife(int remainingLife)
    {
        lifeImageList[remainingLife].color = Color.red;
    }

    /// <summary>
    /// Belirli bir süre sonra sesi tekrar oynatan IEnumerator
    /// </summary>
    /// <returns></returns>
    IEnumerator PlayAudio()
    {
        // eğer soru tipi ses ise
        if (question.questionType == QuestionType.AUDIO)
        {
            // PlayOneShot ile sesi oynat
            questionAudio.PlayOneShot(question.audioClip);
            // birkaç saniye bekle
            yield return new WaitForSeconds(audioLength + 0.5f);
            // tekrar oynat
            StartCoroutine(PlayAudio());
        }
        else // soru tipi ses değilse
        {
            // Coroutine'i durdur
            StopCoroutine(PlayAudio());
            // null döndür
            yield return null;
        }
    }

    /// <summary>
    /// Butonlara atanan metod
    /// </summary>
    /// <param name="btn">Butonlara atanan metod</param>
    void OnClick(Button btn)
    {
        if (quizManager.GameStatus == QuizGameStatus.PLAYING)
        {
            // answered false ise
            if (!answered)
            {
                // answered true yap
                answered = true;
                // bool değerini al
                bool val = quizManager.Answer(btn.name);

                // doğruys
                if (val)
                {
                    // rengi doğru renk yap
                    //btn.image.color = correctCol;
                    StartCoroutine(BlinkImg(btn.image));
                }
                else
                {
                    // değilse rengi yanlış renk yap
                    btn.image.color = wrongCol;
                }
            }
        }
    }

    /// <summary>
    /// Dinamik olarak Kategori Butonları oluşturan metod
    /// </summary>
    void CreateCategoryButtons()
    {
        // QuizManager'daki tüm mevcut kategorileri döngü ile gez
        for (int i = 0; i < quizManager.QuizData.Count;   i++)
        {
            // Yeni bir CategoryBtn oluştur
            QuizCategoryBtnScript categoryBtn = Instantiate(categoryBtnPrefab, scrollHolder.transform);
            // Butonun varsayılan değerlerini ayarla
            categoryBtn.SetButton(quizManager.QuizData[i].categoryName, quizManager.QuizData[i].questions.Count);
            int index = i;
            // Butona dinleyici ekle ve CategoryBtn metodunu çağır
            categoryBtn.Btn.onClick.AddListener(() => CategoryBtn(index, quizManager.QuizData[index].categoryName));
        }
    }

    // Category Butonu tarafından çağrılan metod
    private void CategoryBtn(int index, string category)
    {
        quizManager.StartGame(index, category); //oyunu başlat
        mainMenu.SetActive(false);              //ana menüyü devredışı bırak
        gamePanel.SetActive(true);              //oyun panelini aktifleştir
    }

    // yanıp sönme efekti [isterseniz kullanabilirsiniz veya kullanmayabilirsiniz]
    IEnumerator BlinkImg(Image img)
    {
        for (int i = 0; i < 2; i++)
        {
            img.color = Color.white;
            yield return new WaitForSeconds(0.1f);
            img.color = correctCol;
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void RestryButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
