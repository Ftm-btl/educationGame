using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public InputField usernameInput;
    public InputField passwordInput;
    public Text messageText;

    private bool isFirstLogin = true;

    private void Start()
    {
       
        //PlayerPrefs.DeleteAll();
        // Kullan�c� daha �nce giri� yapm�� m� kontrol ediyoruz
        if (PlayerPrefs.HasKey("IsFirstLogin"))
        {
            isFirstLogin = false;
            LoadNextScene();
        }
    }

    public void SaveProfile()
    {
        string username = usernameInput.text;
        string password = passwordInput.text;

        // Kullan�c� ad� ve �ifreyi PlayerPrefs ile kaydediyoruz
        PlayerPrefs.SetString("Username", username);
        PlayerPrefs.SetString("Password", password);
        PlayerPrefs.SetString("IsFirstLogin", "false");
        PlayerPrefs.Save();

        ShowMessage("Profiliniz kaydedildi!");

        Invoke("LoadNextScene", 3f);
    }

    private void LoadNextScene()
    {
        // �lk giri�te ise di�er sahneye ge�iyoruz, sonraki giri�lerde direkt di�er sahne a��lacak
        string nextScene = isFirstLogin ? "MenuLevel" : "SelectionScene";
        SceneManager.LoadScene(nextScene);
    }

    private void ShowMessage(string message)
    {
        messageText.text = message;
        messageText.gameObject.SetActive(true);
    }
}
