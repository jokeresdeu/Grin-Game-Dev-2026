using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Player")]
    [SerializeField] private Transform player;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI bestScoreText;
    [SerializeField] private TextMeshProUGUI finalScoreText;

    [Header("Panels")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject gameOverPanel;

    private float startX;
    private int score;
    private int bonusScore;
    private bool isGameRunning = true;

    public bool IsGameRunning => isGameRunning;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Time.timeScale = 1f;

        if (player != null)
        {
            startX = player.position.x;
        }

        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        UpdateUI();
    }

    private void Update()
    {
        if (!isGameRunning || player == null)
        {
            return;
        }

        int distanceScore = Mathf.Max(0, Mathf.FloorToInt((player.position.x - startX) * 10f));
        score = distanceScore + bonusScore;

        UpdateUI();
    }

    public void AddScore(int value)
    {
        bonusScore += value;
        UpdateUI();
    }

    public void GameOver()
    {
        if (!isGameRunning)
        {
            return;
        }

        isGameRunning = false;

        int bestScore = PlayerPrefs.GetInt("BestScore", 0);

        if (score > bestScore)
        {
            PlayerPrefs.SetInt("BestScore", score);
        }

        if (finalScoreText != null)
        {
            finalScoreText.text = "SCORE: " + score;
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }

    public void PauseGame()
    {
        if (!isGameRunning)
        {
            return;
        }

        Time.timeScale = 0f;

        if (pausePanel != null)
        {
            pausePanel.SetActive(true);
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;

        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Game");
    }

    public void QuitToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    private void UpdateUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "SCORE: " + score;
        }

        if (bestScoreText != null)
        {
            bestScoreText.text = "BEST: " + PlayerPrefs.GetInt("BestScore", 0);
        }
    }
}