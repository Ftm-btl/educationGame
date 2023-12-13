using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    [SerializeField]
    public GameObject mathcingBtn, writingBtn, quizBtn, backBtn;

    void Start()
    {
        FadeOut();
    }

    
    void FadeOut()
    {
        //oyun butonlarýnýn görünürlüðünü ve geliþ süresini ayarlama
        mathcingBtn.GetComponent<CanvasGroup>().DOFade(1, 0.8f); 
        writingBtn.GetComponent<CanvasGroup>().DOFade(1, 0.8f).SetDelay(0.5f);
        quizBtn.GetComponent<CanvasGroup>().DOFade(1, 0.8f).SetDelay(0.7f);
        backBtn.GetComponent<CanvasGroup>().DOFade(1, 0.8f);
    }
    /*public void WritingGame()
    {
        SceneManager.LoadScene(3);
    }
    public void MathcingGame()
    {
        SceneManager.LoadScene(2);
    }
    public void MenuLevel()
    {
        SceneManager.LoadScene(1);
    }*/

}
