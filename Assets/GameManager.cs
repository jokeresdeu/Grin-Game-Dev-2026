using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject gameOverPanel;
    public TextMeshProUGUI scoreText;

    public AudioSource gameMusic;
    public AudioSource gameOverMusic;

    private int score = 0;
    private bool isGameOver = false;

    private void Awake()
    {
        Instance = this;
        Time.timeScale = 1f;
    }

    private void Start()
    {
        UpdateScoreUI();

        if (gameMusic != null)
            gameMusic.Play();
    }

    public void AddScore()
    {
        if (isGameOver) return;

        score++;
        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }

    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;

        Time.timeScale = 0f;

        // 🔴 ЗУПИНЯЄМО МУЗИКУ ГРИ
        if (gameMusic != null)
            gameMusic.Stop();

        // 🔴 ВМИКАЄМО ЗВУК ПРОГРАШУ
        if (gameOverMusic != null)
            gameOverMusic.Play();

        gameOverPanel.SetActive(true);
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
}