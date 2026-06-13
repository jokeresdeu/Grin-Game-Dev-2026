using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuController : MonoBehaviour
{
    public GameObject DifficultyPanel;
    public GameObject MainMenuContent;
    public GameObject SettingsPanel;
    private void Start()
    {
        if (DifficultyPanel != null)
            DifficultyPanel.SetActive(false);
        if (SettingsPanel != null)
            SettingsPanel.SetActive(false);
        if (MainMenuContent != null)
        {
            MainMenuContent.SetActive(true);
            Transform settingsBtn = MainMenuContent.transform.Find("SettingsButton");
            if (settingsBtn != null)
            {
                UnityEngine.UI.Button btn = settingsBtn.GetComponent<UnityEngine.UI.Button>();
                if (btn != null)
                {
                    btn.onClick.RemoveListener(OnSettingsClicked);
                    btn.onClick.AddListener(OnSettingsClicked);
                }
            }
        }
        if (DifficultyPanel != null)
        {
            Transform closeBtn = DifficultyPanel.transform.Find("CloseBtn");
            if (closeBtn != null)
            {
                UnityEngine.UI.Button btn = closeBtn.GetComponent<UnityEngine.UI.Button>();
                if (btn != null)
                {
                    btn.onClick.RemoveListener(CloseDifficultyPanel);
                    btn.onClick.AddListener(CloseDifficultyPanel);
                }
            }
        }
        if (SettingsPanel != null)
        {
            Transform samToggle = SettingsPanel.transform.Find("SAMToggle");
            if (samToggle != null)
            {
                UnityEngine.UI.Toggle t = samToggle.GetComponent<UnityEngine.UI.Toggle>();
                if (t != null)
                {
                    t.SetIsOnWithoutNotify(GameSettings.SAMDebugMode);
                    t.onValueChanged.RemoveListener(OnSAMToggle);
                    t.onValueChanged.AddListener(OnSAMToggle);
                }
            }
            Transform closeBtn = SettingsPanel.transform.Find("CloseBtn");
            if (closeBtn != null)
            {
                UnityEngine.UI.Button btn = closeBtn.GetComponent<UnityEngine.UI.Button>();
                if (btn != null)
                {
                    btn.onClick.RemoveListener(CloseSettingsPanel);
                    btn.onClick.AddListener(CloseSettingsPanel);
                }
            }
        }
    }
    public void OnPlayClicked()
    {
        if (DifficultyPanel != null) DifficultyPanel.SetActive(true);
        if (MainMenuContent != null) MainMenuContent.SetActive(false);
    }
    public void OnDifficultySelected(int difficulty)
    {
        GameSettings.Difficulty = difficulty;
        SceneManager.LoadScene("CupGame");
    }
    public void OnSettingsClicked()
    {
        if (SettingsPanel != null) SettingsPanel.SetActive(true);
        if (MainMenuContent != null) MainMenuContent.SetActive(false);
    }
    public void CloseSettingsPanel()
    {
        if (SettingsPanel != null) SettingsPanel.SetActive(false);
        if (MainMenuContent != null) MainMenuContent.SetActive(true);
    }
    public void OnSAMToggle(bool isOn)
    {
        GameSettings.SAMDebugMode = isOn;
    }
    public void OnQuitClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    public void CloseDifficultyPanel()
    {
        if (DifficultyPanel != null) DifficultyPanel.SetActive(false);
        if (MainMenuContent != null) MainMenuContent.SetActive(true);
    }
}
