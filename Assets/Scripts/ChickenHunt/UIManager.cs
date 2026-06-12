using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ChickenHunt
{
    public class UIManager : MonoBehaviour
    {
        [Header("UI Панелі")]
        [SerializeField] private GameObject _pausePanel;
        [SerializeField] private GameObject _gameOverPanel;

        [Header("Елементи HUD")]
        [SerializeField] private Slider _hpSlider;

        private bool _isPaused = false;

        private void Start()
        {
            // На початку гри ховаємо панелі
            if (_pausePanel != null) _pausePanel.SetActive(false);
            if (_gameOverPanel != null) _gameOverPanel.SetActive(false);
            
            // Переконуємося, що час іде нормально
            Time.timeScale = 1f;

            // Якщо слайдер є, виставляємо його на максимум
            if (_hpSlider != null)
            {
                _hpSlider.maxValue = 100f;
                _hpSlider.value = 100f;
            }
        }

        private void Update()
        {
            // Відстежуємо натискання клавіші Esc (або аналога на Mac)
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (_isPaused)
                {
                    ResumeGame();
                }
                else
                {
                    PauseGame();
                }
            }
        }

        // Метод для оновлення слайдера HP з інших скриптів
        public void UpdateHP(float currentHP)
        {
            if (_hpSlider != null)
            {
                _hpSlider.value = currentHP;
            }
        }

        // Поставити гру на паузу
        public void PauseGame()
        {
            _isPaused = true;
            if (_pausePanel != null) _pausePanel.SetActive(true);
            Time.timeScale = 0f; // Зупиняємо фізику та час у грі
        }

        // Продовжити гру
        public void ResumeGame()
        {
            _isPaused = false;
            if (_pausePanel != null) _pausePanel.SetActive(false);
            Time.timeScale = 1f; // Відновлюємо хід часу
        }

        // Перезапустити поточний рівень
        public void RestartGame()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        // Повернутися в Головне меню
        public void GoToMainMenu()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("MainMenu"); // Назва нашої першої сцени
        }
    }
}