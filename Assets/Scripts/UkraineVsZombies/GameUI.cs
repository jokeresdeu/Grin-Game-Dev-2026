using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    public static GameUI Instance;

    [Header("Stats")]
    public int score = 0;
    public int baseHP = 5;

    [Header("UI Text")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI enemiesText;

    [Header("Panels")]
    [SerializeField] private GameObject losePanel;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateScoreUI();
        UpdateHPUI();
        UpdateWaveUI(1);
        UpdateEnemiesUI(0);

        if (losePanel != null)
            losePanel.SetActive(false);

        Time.timeScale = 1f;
    }

    public void AddScore(int value)
    {
        score += value;
        UpdateScoreUI();
    }

    public void LoseHP(int value)
    {
        baseHP -= value;

        if (baseHP < 0)
            baseHP = 0;

        UpdateHPUI();

        if (baseHP <= 0)
        {
            ShowGameOver();
        }
    }

    public void UpdateWaveUI(int wave)
    {
        if (waveText != null)
            waveText.text = "Wave: " + wave;
    }

    public void UpdateEnemiesUI(int count)
    {
        if (enemiesText != null)
            enemiesText.text = "Enemies: " + count;
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }

    private void UpdateHPUI()
    {
        if (hpText != null)
            hpText.text = "HP: " + baseHP;
    }

    private void ShowGameOver()
    {
        if (losePanel != null)
            losePanel.SetActive(true);

        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}