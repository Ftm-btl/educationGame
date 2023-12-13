using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MatchingButton : MonoBehaviour
{
    public enum EButtonType
    {
       NotSet,
       PairNumberBtn,  // Eþleþtirme sayýsý butonu
        PuzzelCatagoryBtn,// Bulmaca kategorisi butonu
    };

    [SerializeField] public EButtonType ButtonType = EButtonType.NotSet; // Butonun türü
    [HideInInspector] public PairsManager.EPairNumber PairNumber = PairsManager.EPairNumber.NotSet; // Eþleþtirme sayýsý deðeri
    [HideInInspector] public PairsManager.EPuzzleCatagories PuzzleCatagories = PairsManager.EPuzzleCatagories.NotSet; // Bulmaca kategorisi deðeri

    void Start()
    {
        
    }

    public void SetGameOption(string GameSceneName)
    {
        var comp = gameObject.GetComponent<MatchingButton>(); // Butonun MatchingButton bileþenini al


        switch (comp.ButtonType)
        {
            case MatchingButton.EButtonType.PairNumberBtn:
                PairsManager.Instance.SetPairNumber(comp.PairNumber);
                break;
            case EButtonType.PuzzelCatagoryBtn:
                PairsManager.Instance.SetPuzzelCatagories(comp.PuzzleCatagories);
                break;
        }

        if (PairsManager.Instance.AllSettingsReady())// Tüm ayarlamalar tamamlandýysa
        {
            SceneManager.LoadScene(GameSceneName);// Oyun sahnesine geçiþ yap
        }
    }
   
}
