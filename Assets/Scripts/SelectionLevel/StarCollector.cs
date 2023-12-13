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

        // Kaydedilen yýldýz sayýsýný ve son giriþ tarihini al
        starsCollected = PlayerPrefs.GetInt("StarsCollected", 1);
        lastLoginDate = PlayerPrefs.GetString("LastLoginDate", "");

        // Eðer son giriþ tarihi bugünkü tarihten farklýysa yýldýz sayýsýný sýfýrla
        /*if (!IsWithin24Hours(lastLoginDate))
        {
            starsCollected = 0;
        }*/

        // Son giriþ tarihini güncelle
        SaveLastLoginDate();

        // Yýldýz sayýsýný göster
        ShowStarCount();
    }

    public void ShowPanel()
    {
        // Yýldýz sayýsýný PlayerPrefs'ten al
        int starCount = PlayerPrefs.GetInt("StarsCollected", 0);

        // Yýldýz sayýsýný metin öðesine atayarak göster
        starCountText.text = "Yýldýz Sayýsý: " + starCount.ToString();
    }

    private bool IsWithin24Hours(string date)
    {
        if (string.IsNullOrEmpty(date))
        {
            return false;
        }

        // Kaydedilen tarih ile þu anki tarih arasýndaki saat farkýný al
        System.TimeSpan timeSinceLastLogin = System.DateTime.Now - System.DateTime.Parse(date);

        // Eðer geçen süre 24 saatten az ise true döndür, aksi halde false döndür
        return timeSinceLastLogin.TotalHours < 24;
    }

    private void SaveLastLoginDate()
    {
        // Þu anki tarihi PlayerPrefs ile kaydet
        string currentLoginDate = System.DateTime.Now.ToString();
        PlayerPrefs.SetString("LastLoginDate", currentLoginDate);
    }

    private void ShowStarCount()
    {
        // Yýldýz sayýsýný metin öðesine atayarak göster
        starCountText.text = "Yýldýz Sayýsý: " + starsCollected.ToString();
    }
}
