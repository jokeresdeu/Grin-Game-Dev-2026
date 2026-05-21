using UnityEngine;
using TMPro; // Для TextMeshPro
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public GameObject gameOverPanel;
    public TextMeshProUGUI finalScoreText;
    public GameObject pausePanel; // Сюди перетягнеш нову панель паузи

    private int score = 0;
    private bool isGameOver = false;
    private bool isPaused = false; // Чи стоїть гра на паузі

    void Start()
    {
        Time.timeScale = 1f;
    }

    void Update()
    {
        // Якщо гравець програв, на паузу ставити не можна
        if (isGameOver) return;

        // Вмикаємо/вимикаємо паузу при натисканні на Escape або P
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            if (isPaused)
                TogglePause(false);
            else
                TogglePause(true);
        }
    }

    public void AddScore(int amount)
    {
        if (isGameOver) return;
        score += amount;
        scoreText.text = "Score: " + score;
    }

    public void GameOver()
    {
        if (isGameOver) return;
        isGameOver = true;
        gameOverPanel.SetActive(true);

        if (finalScoreText != null)
        {
            finalScoreText.text = "Final Score: " + score;
        }

        AudioSource backgroundMusic = FindObjectOfType<AudioSource>();
        if (backgroundMusic != null)
        {
            backgroundMusic.Stop();
        }

        Time.timeScale = 0f;
    }

    // Нова функція для перемикання стану паузи
    public void TogglePause(bool pauseState)
    {
        isPaused = pauseState;
        pausePanel.SetActive(pauseState); // Показуємо або ховаємо панель
        Time.timeScale = pauseState ? 0f : 1f; // Зупиняємо або відновлюємо час у грі
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}