using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UkraineVsZombies
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] private string _gameSceneName = "UkraineVsZombies";
        [SerializeField] private GameObject _mainPanel;
        [SerializeField] private GameObject _creditsPanel;

        private void Start()
        {
            Time.timeScale = 1f;
            ShowMainPanel();
        }

        public void StartGame()
        {
            SceneManager.LoadScene(_gameSceneName);
        }

        public void ShowCredits()
        {
            if (_mainPanel != null)
                _mainPanel.SetActive(false);

            if (_creditsPanel != null)
                _creditsPanel.SetActive(true);
        }

        public void ShowMainPanel()
        {
            if (_mainPanel != null)
                _mainPanel.SetActive(true);

            if (_creditsPanel != null)
                _creditsPanel.SetActive(false);
        }

        public void Quit()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
