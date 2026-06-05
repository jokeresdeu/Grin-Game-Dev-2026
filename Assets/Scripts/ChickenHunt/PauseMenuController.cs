using UnityEngine;
using UnityEngine.SceneManagement;

namespace ChickenHunt
{
    public class PauseMenuController : MonoBehaviour
    {
        [SerializeField] private GameObject _pausePanel;
        [SerializeField] private GameObject _crosshairObject; // Посилання на приціл OccaSoftware
        private bool _isPaused = false;

        void Start()
        {
            if (_pausePanel != null)
            {
                _pausePanel.SetActive(false);
            }
        }

        void Update()
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
            _isPaused = true;
            if (_pausePanel != null) _pausePanel.SetActive(true);

            // ВИМИКАЄМО ПРИЦІЛ, щоб він не брав під контроль мишку
            if (_crosshairObject != null) _crosshairObject.SetActive(false);

            Time.timeScale = 0f;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public void ResumeGame()
        {
            _isPaused = false;
            if (_pausePanel != null) _pausePanel.SetActive(false);

            // ВМИКАЄМО ПРИЦІЛ НАЗАД В ГРУ
            if (_crosshairObject != null) _crosshairObject.SetActive(true);

            Time.timeScale = 1f;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
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