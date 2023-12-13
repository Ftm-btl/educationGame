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
        // Kullanýcý daha önce giriþ yapmýþ mý kontrol ediyoruz
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

        // Kullanýcý adý ve þifreyi PlayerPrefs ile kaydediyoruz
        PlayerPrefs.SetString("Username", username);
        PlayerPrefs.SetString("Password", password);
        PlayerPrefs.SetString("IsFirstLogin", "false");
        PlayerPrefs.Save();

        ShowMessage("Profiliniz kaydedildi!");

        Invoke("LoadNextScene", 3f);
    }

    private void LoadNextScene()
    {
        // Ýlk giriþte ise diðer sahneye geçiyoruz, sonraki giriþlerde direkt diðer sahne açýlacak
        string nextScene = isFirstLogin ? "MenuLevel" : "SelectionScene";
        SceneManager.LoadScene(nextScene);
    }

    private void ShowMessage(string message)
    {
        messageText.text = message;
        messageText.gameObject.SetActive(true);
    }
}
