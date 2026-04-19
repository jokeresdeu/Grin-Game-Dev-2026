using UnityEngine;
using UnityEngine.SceneManagement;

namespace ClassicPlatformer
{
    public class PauseMenu : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private GameObject _pausePanel;

        private bool _isPaused;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (_isPaused) Resume();
                else Pause();
            }
        }

        public void Pause()
        {
            _isPaused = true;
            _pausePanel.SetActive(true);
            Time.timeScale = 0f;
        }

        public void Resume()
        {
            _isPaused = false;
            _pausePanel.SetActive(false);
            Time.timeScale = 1f;
        }

        public void Restart()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void Quit()
        {
            Time.timeScale = 1f;
            Application.Quit();
        }
    }
}