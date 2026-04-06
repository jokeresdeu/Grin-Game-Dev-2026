using UnityEngine;
using UnityEngine.SceneManagement;

namespace ChickenHunt
{
    public class PauseMenuController : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private GameObject _pausePanel;

        private bool _isPaused = false;

        private void Start()
        {
            _pausePanel.SetActive(false);
            Time.timeScale = 1f;
        }

        private void Update()
        {
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

        public void PauseGame()
        {
            _pausePanel.SetActive(true);
            Time.timeScale = 0f;
            _isPaused = true;
        }

        public void ResumeGame()
        {
            _pausePanel.SetActive(false);
            Time.timeScale = 1f;
            _isPaused = false;
        }

        public void RestartGame()
        {
            Time.timeScale = 1f;

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void QuitToMainMenu()
        {
            Time.timeScale = 1f;

            SceneManager.LoadScene(0);
        }
    }
}