using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI")]
    [SerializeField] private GameObject deathPanel;
    [SerializeField] private TextMeshProUGUI killText;

    private int _kills;

    private void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (deathPanel != null)
            deathPanel.SetActive(false);
    }

    public void AddKill()
    {
        _kills++;

        UpdateKillUI();
    }

    private void UpdateKillUI()
    {
        if (killText != null)
            killText.text = "Kills: " + _kills;
    }

    public void PlayerDied()
    {
        Time.timeScale = 0f;

        if (deathPanel != null)
            deathPanel.SetActive(true);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        _kills = 0;

        SceneManager.LoadScene(0);
    }
}