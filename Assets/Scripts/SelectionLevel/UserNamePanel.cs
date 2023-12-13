using UnityEngine;
using UnityEngine.UI;

public class UserNamePanel : MonoBehaviour
{
    public Button userButton;
    public GameObject userPanel;

    private void Start()
    {
        // Kullan�c� butonuna t�klama olay�n� ekleyin
        userButton.onClick.AddListener(ShowUserPanel);
    }

    private void ShowUserPanel()
    {
        // Paneli etkinle�tirin
        userPanel.SetActive(true);

        // Paneldeki UserPanel scriptini al�n
        StarCollector userPanelScript = userPanel.GetComponent<StarCollector>();

        // Y�ld�z say�s�n� g�ster
        //userPanelScript.ShowPanel();
    }
}
