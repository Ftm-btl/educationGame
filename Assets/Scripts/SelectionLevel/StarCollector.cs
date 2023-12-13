using UnityEngine;
using UnityEngine.UI;

public class StarCollector : MonoBehaviour
{
    public Text starCountText;
    public Text userNameText;
    private int starsCollected;
    private string lastLoginDate;

    private void Start()
    {
        if (PlayerPrefs.HasKey("Username"))
        {
            string username = PlayerPrefs.GetString("Username");
            userNameText.text = username;
        }

        // Kaydedilen y�ld�z say�s�n� ve son giri� tarihini al
        starsCollected = PlayerPrefs.GetInt("StarsCollected", 1);
        lastLoginDate = PlayerPrefs.GetString("LastLoginDate", "");

        // E�er son giri� tarihi bug�nk� tarihten farkl�ysa y�ld�z say�s�n� s�f�rla
        /*if (!IsWithin24Hours(lastLoginDate))
        {
            starsCollected = 0;
        }*/

        // Son giri� tarihini g�ncelle
        SaveLastLoginDate();

        // Y�ld�z say�s�n� g�ster
        ShowStarCount();
    }

    public void ShowPanel()
    {
        // Y�ld�z say�s�n� PlayerPrefs'ten al
        int starCount = PlayerPrefs.GetInt("StarsCollected", 0);

        // Y�ld�z say�s�n� metin ��esine atayarak g�ster
        starCountText.text = "Y�ld�z Say�s�: " + starCount.ToString();
    }

    private bool IsWithin24Hours(string date)
    {
        if (string.IsNullOrEmpty(date))
        {
            return false;
        }

        // Kaydedilen tarih ile �u anki tarih aras�ndaki saat fark�n� al
        System.TimeSpan timeSinceLastLogin = System.DateTime.Now - System.DateTime.Parse(date);

        // E�er ge�en s�re 24 saatten az ise true d�nd�r, aksi halde false d�nd�r
        return timeSinceLastLogin.TotalHours < 24;
    }

    private void SaveLastLoginDate()
    {
        // �u anki tarihi PlayerPrefs ile kaydet
        string currentLoginDate = System.DateTime.Now.ToString();
        PlayerPrefs.SetString("LastLoginDate", currentLoginDate);
    }

    private void ShowStarCount()
    {
        // Y�ld�z say�s�n� metin ��esine atayarak g�ster
        starCountText.text = "Y�ld�z Say�s�: " + starsCollected.ToString();
    }
}
