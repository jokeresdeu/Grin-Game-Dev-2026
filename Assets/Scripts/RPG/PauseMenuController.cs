using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG
{
    public class PauseMenuController : MonoBehaviour
    {
        [SerializeField] private GameObject _pausePanel;
        [SerializeField] private GameObject _pauseButton;
        
        private void Start()
        {
            if (_pausePanel != null) _pausePanel.SetActive(false);
            if (_pauseButton != null) _pauseButton.SetActive(true);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (_pausePanel != null && _pausePanel.activeSelf)
                    ResumeGame();
                else
                    PauseGame();
            }
        }

        public void PauseGame()
        {
            if (RPGGameManager.Instance != null && RPGGameManager.Instance.IsGameOver) return;

            if (_pausePanel != null) _pausePanel.SetActive(true);
            if (_pauseButton != null) _pauseButton.SetActive(false);
            
            Time.timeScale = 0f;
            if (RPGGameManager.Instance != null)
                RPGGameManager.Instance.PauseArena();
        }

        public void ResumeGame()
        {
            if (_pausePanel != null) _pausePanel.SetActive(false);
            if (_pauseButton != null) _pauseButton.SetActive(true);
            
            Time.timeScale = 1f;
            if (RPGGameManager.Instance != null)
                RPGGameManager.Instance.ResumeArena();
        }

        public void RestartGame()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void QuitToMainMenu()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("MainMenu");
        }
    }
}

