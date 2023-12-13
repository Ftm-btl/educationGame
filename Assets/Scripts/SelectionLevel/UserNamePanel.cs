using UnityEngine;
using UnityEngine.UI;

public class UserNamePanel : MonoBehaviour
{
    public Button userButton;
    public GameObject userPanel;

    private void Start()
    {
        // Kullanýcý butonuna týklama olayýný ekleyin
        userButton.onClick.AddListener(ShowUserPanel);
    }

    private void ShowUserPanel()
    {
        // Paneli etkinleþtirin
        userPanel.SetActive(true);

        // Paneldeki UserPanel scriptini alýn
        StarCollector userPanelScript = userPanel.GetComponent<StarCollector>();

        // Yýldýz sayýsýný göster
        //userPanelScript.ShowPanel();
    }
}
