using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MatchingButton : MonoBehaviour
{
    public enum EButtonType
    {
       NotSet,
       PairNumberBtn,  // E�le�tirme say�s� butonu
        PuzzelCatagoryBtn,// Bulmaca kategorisi butonu
    };

    [SerializeField] public EButtonType ButtonType = EButtonType.NotSet; // Butonun t�r�
    [HideInInspector] public PairsManager.EPairNumber PairNumber = PairsManager.EPairNumber.NotSet; // E�le�tirme say�s� de�eri
    [HideInInspector] public PairsManager.EPuzzleCatagories PuzzleCatagories = PairsManager.EPuzzleCatagories.NotSet; // Bulmaca kategorisi de�eri

    void Start()
    {
        
    }

    public void SetGameOption(string GameSceneName)
    {
        var comp = gameObject.GetComponent<MatchingButton>(); // Butonun MatchingButton bile�enini al


        switch (comp.ButtonType)
        {
            case MatchingButton.EButtonType.PairNumberBtn:
                PairsManager.Instance.SetPairNumber(comp.PairNumber);
                break;
            case EButtonType.PuzzelCatagoryBtn:
                PairsManager.Instance.SetPuzzelCatagories(comp.PuzzleCatagories);
                break;
        }

        if (PairsManager.Instance.AllSettingsReady())// T�m ayarlamalar tamamland�ysa
        {
            SceneManager.LoadScene(GameSceneName);// Oyun sahnesine ge�i� yap
        }
    }
   
}
