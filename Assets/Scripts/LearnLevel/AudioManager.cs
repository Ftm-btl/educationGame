using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] public LearnData[] learnData; // Öðrenme verilerini içeren dizi
    public Button youtubeLinkButton;

    public Button nextButton; // Sonraki resme geçmek için buton
    public Button prevButton; // Önceki resme geçmek için buton
    public Button soundButton; // Sesleri çalmak için buton
    public Button nextElementButton; // Bir sonraki elemente geçmek için buton
    public Button previousElementButton; // Bir önceki elemente geçmek için buton

    private int currentDataIndex = 0; // Geçerli öðrenme verisi indeksi
    private int currentIndex = 0; // Geçerli resim indeksi
    private AudioSource audioSource; //ses dosyasý eklenebilirlik
    private Image imageComponent; //resim dosyasý eklenebilirlik

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        imageComponent = GetComponentInChildren<Image>();

        nextButton.onClick.AddListener(NextImage);
        prevButton.onClick.AddListener(PreviousImage);
        soundButton.onClick.AddListener(PlaySound);
        nextElementButton.onClick.AddListener(NextElement);
        previousElementButton.onClick.AddListener(PreviousElement);

        ShowImage(currentDataIndex, currentIndex);
        youtubeLinkButton.onClick.AddListener(OpenYouTubeLink);

    }

    private void ShowImage(int dataIndex, int imageIndex)
    {
        if (dataIndex >= 0 && dataIndex < learnData.Length)
        {
            LearnData data = learnData[dataIndex];
            if (imageIndex >= 0 && imageIndex < data.image.Length)
            {
                imageComponent.sprite = data.image[imageIndex];
               
            }
        }
    }

    private void PlaySound()
    {
        if (currentDataIndex >= 0 && currentDataIndex < learnData.Length)
        {
            LearnData data = learnData[currentDataIndex];
            if (currentIndex >= 0 && currentIndex < data.sounds.Length)
            {
                audioSource.clip = data.sounds[currentIndex];
                audioSource.Play();
            }
        }
    }

    private void NextImage()
    {
        if (currentDataIndex >= 0 && currentDataIndex < learnData.Length)
        {
            LearnData data = learnData[currentDataIndex];
            currentIndex++;
            if (currentIndex >= data.image.Length)
            {
                currentIndex = 0;
            }
            ShowImage(currentDataIndex, currentIndex);
        }
    }

    private void PreviousImage()
    {
        if (currentDataIndex >= 0 && currentDataIndex < learnData.Length)
        {
            LearnData data = learnData[currentDataIndex];
            currentIndex--;
            if (currentIndex < 0)
            {
                currentIndex = data.image.Length - 1;
            }
            ShowImage(currentDataIndex, currentIndex);
        }
    }

    private void NextElement()
    {
        currentDataIndex++;
        if (currentDataIndex >= learnData.Length)
        {
            currentDataIndex = 0;
        }
        currentIndex = 0;
        ShowImage(currentDataIndex, currentIndex);
    }

    private void PreviousElement()
    {
        currentDataIndex--;
        if (currentDataIndex < 0)
        {
            currentDataIndex = learnData.Length - 1;
        }
        currentIndex = 0;
        ShowImage(currentDataIndex, currentIndex);
    }
    private void OpenYouTubeLink()
    {
        string youtubeLink = learnData[currentDataIndex].youtubeLinks[currentIndex];
        Application.OpenURL(youtubeLink);
    }


}

[System.Serializable]
public class LearnData
{
    public Sprite[] image;
    public AudioClip[] sounds;
    public string[] youtubeLinks;
}
