using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

namespace ClassicPlatformer
{
    public class GameUI : MonoBehaviour
    {
        [Header("UI Elements")]
        public TextMeshProUGUI healthText;
        public GameObject pauseMenu;
        public GameObject deathMenu;
        public GameObject winnerMenu;

        private bool _isPaused = false;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (_isPaused)
                    ResumeGame();
                else
                    PauseGame();
            }
        }

        public void UpdateHealth(int current, int max)
        {
            if (healthText != null)
                healthText.text = $"HP: {current}/{max}";
        }

        public void PauseGame()
        {
            _isPaused = true;
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
        }

        public void ResumeGame()
        {
            _isPaused = false;
            pauseMenu.SetActive(false);
            Time.timeScale = 1f;
        }

        public void ShowDeathMenu()
        {
            deathMenu.SetActive(true);
            Time.timeScale = 0f;
        }

        public void ShowWinnerMenu()
        {
            winnerMenu.SetActive(true);
            Time.timeScale = 0f;
        }

        public void RestartGame()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void QuitToMenu()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("MainMenu");
        }
    }
}