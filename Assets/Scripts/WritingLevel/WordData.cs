using UnityEngine;
using UnityEngine.UI;

public class WordData : MonoBehaviour
{
    [SerializeField] private Text wordText;

    [HideInInspector]
    public char wordValue;

    private Button buttonComponent;

    private void Awake()
    {
        buttonComponent = GetComponent<Button>();//// Buton bileşenini al
        if (buttonComponent)
        {
            buttonComponent.onClick.AddListener(() => WordSelected());// Buton tıklanma olayına WordSelected yöntemini ekle
        }
    }

    public void SetWord(char value)
    {
        wordText.text = value + "";// Metin nesnesine değeri ata
        wordValue = value;// Kelime değerini güncelle
    }

    private void WordSelected()
    {
        QuizManager.instance.SelectedOption(this);// QuizManager üzerinden SelectedOption yöntemini çağır ve bu bileşeni parametre olarak gönder
    }
}
    

