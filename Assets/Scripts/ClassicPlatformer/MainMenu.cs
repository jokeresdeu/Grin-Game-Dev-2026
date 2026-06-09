using UnityEngine;
using UnityEngine.SceneManagement;
 
namespace ClassicPlatformer
{
    public class MainMenu : MonoBehaviour
    {
        [Header("Panels")]
        [SerializeField] private GameObject _mainMenuPanel; 
        [SerializeField] private GameObject _pausePanel;    
 
        private void Start()
        {
            Time.timeScale = 0f;
            _mainMenuPanel.SetActive(true);
 
            if (_pausePanel != null)
                _pausePanel.SetActive(false);
        }

        public void PlayGame()
        {
            _mainMenuPanel.SetActive(false);
            Time.timeScale = 1f; 
        }
 
        public void QuitGame()
        {
            Application.Quit();
            Debug.Log("Quit!");
        }
    }
}