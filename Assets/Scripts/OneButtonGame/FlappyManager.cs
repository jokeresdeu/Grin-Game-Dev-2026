using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro; // Додали для підтримки сучасного тексту TextMeshPro

namespace OneButtonGame
{
    public class FlappyManager : MonoBehaviour
    {
        public static FlappyManager Instance;

        [Header("UI Елементи")]
        public GameObject gameOverPanel;
        public GameObject pausePanel;
        public TextMeshProUGUI scoreText; // Змінено тип для сумісності з TextMeshPro

        private int _score = 0;
        private bool _isPaused = false;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            Time.timeScale = 1f; // Запускаємо час у грі з самого початку
            _score = 0;
            UpdateScoreText();
            
            if (gameOverPanel != null) gameOverPanel.SetActive(false);
            if (pausePanel != null) pausePanel.SetActive(false);
        }

        private void Update()
        {
            // Кнопка Esc для паузи
            if (Input.GetKeyDown(KeyCode.Escape) && !gameOverPanel.activeSelf)
            {
                if (_isPaused) ResumeGame(); else PauseGame();
            }
        }

        public void AddScore(int value)
        {
            _score += value;
            UpdateScoreText();
        }

        private void UpdateScoreText()
        {
            if (scoreText != null) scoreText.text = "Score: " + _score;
        }

        public void GameOver()
        {
            Time.timeScale = 0f; // Зупиняємо світ при програші
            if (gameOverPanel != null) gameOverPanel.SetActive(true);
        }

        public void PauseGame()
        {
            _isPaused = true;
            Time.timeScale = 0f;
            if (pausePanel != null) pausePanel.SetActive(true);
        }

        public void ResumeGame()
        {
            _isPaused = false;
            Time.timeScale = 1f;
            if (pausePanel != null) pausePanel.SetActive(false);
        }

        // НАШ ОНОВЛЕНИЙ МЕТОД ДЛЯ КНОПКИ RESTART
        public void RestartGame()
        {
            Time.timeScale = 1f; // ОБОВ'ЯЗКОВО «відморожуємо» час перед перезавантаженням!
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}