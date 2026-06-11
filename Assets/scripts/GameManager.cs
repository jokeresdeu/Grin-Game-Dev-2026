using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI Text")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text bestScoreText;
    [SerializeField] private TMP_Text gameOverScoreText;

    [Header("Panels")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject gameOverPanel;

    private float score;
    private int bestScore;

    public bool IsGameOver { get; private set; }
    public bool IsPaused { get; private set; }

    private void Awake()
    {
        Instance = this;
        Time.timeScale = 1f;

        bestScore = PlayerPrefs.GetInt("BestScore", 0);
    }

    private void Start()
    {
        if (pausePanel != null)
            pausePanel.SetActive(false);

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        UpdateUI();
    }

    private void Update()
    {
        if (IsGameOver || IsPaused) return;

        score += Time.deltaTime * 10f;
        UpdateUI();
    }

    public void AddCrystalScore()
    {
        if (IsGameOver) return;

        score += 25f;
        UpdateUI();
    }

    public void GameOver()
    {
        if (IsGameOver) return;

        IsGameOver = true;

        int finalScore = Mathf.FloorToInt(score);

        if (finalScore > bestScore)
        {
            bestScore = finalScore;
            PlayerPrefs.SetInt("BestScore", bestScore);
            PlayerPrefs.Save();
        }

        if (gameOverScoreText != null)
        {
            gameOverScoreText.text =
                "Score: " + finalScore +
                "\nBest: " + bestScore;
        }

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        Time.timeScale = 0f;
    }

    public void PauseGame()
    {
        if (IsGameOver) return;

        IsPaused = true;

        if (pausePanel != null)
            pausePanel.SetActive(true);

        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        IsPaused = false;

        if (pausePanel != null)
            pausePanel.SetActive(false);

        Time.timeScale = 1f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void UpdateUI()
    {
        int currentScore = Mathf.FloorToInt(score);

        if (scoreText != null)
            scoreText.text = "Score: " + currentScore;

        if (bestScoreText != null)
            bestScoreText.text = "Best: " + bestScore;
    }
}