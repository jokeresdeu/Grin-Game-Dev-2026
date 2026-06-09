using UnityEngine;
using UnityEngine.SceneManagement;
 
namespace ClassicPlatformer
{
    public class PauseMenu : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private GameObject _pausePanel; 
 
        private bool _isPaused = false;
 
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (_isPaused)
                    Resume();
                else
                    Pause();
            }
        }
 
        public void Pause()
        {
            _pausePanel.SetActive(true);
            Time.timeScale = 0f; 
            _isPaused = true;
        }
 
        public void Resume()
        {
            _pausePanel.SetActive(false);
            Time.timeScale = 1f; 
            _isPaused = false;
        }
 
        public void Restart()
        {
            Time.timeScale = 1f; 
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
 
        public void Quit()
        {
            Time.timeScale = 1f;
            Application.Quit();
            Debug.Log("Quit!"); 
        }
    }
}