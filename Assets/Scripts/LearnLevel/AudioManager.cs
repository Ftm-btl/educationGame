using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] public LearnData[] learnData; // ��renme verilerini i�eren dizi
    public Button youtubeLinkButton;

    public Button nextButton; // Sonraki resme ge�mek i�in buton
    public Button prevButton; // �nceki resme ge�mek i�in buton
    public Button soundButton; // Sesleri �almak i�in buton
    public Button nextElementButton; // Bir sonraki elemente ge�mek i�in buton
    public Button previousElementButton; // Bir �nceki elemente ge�mek i�in buton

    private int currentDataIndex = 0; // Ge�erli ��renme verisi indeksi
    private int currentIndex = 0; // Ge�erli resim indeksi
    private AudioSource audioSource; //ses dosyas� eklenebilirlik
    private Image imageComponent; //resim dosyas� eklenebilirlik

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
