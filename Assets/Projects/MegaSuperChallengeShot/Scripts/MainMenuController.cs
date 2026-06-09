using UnityEngine;
using UnityEngine.SceneManagement;

namespace Projects.MegaSuperChallengeShot.Scripts
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private string _gameSceneName = "lab3";
        [SerializeField] private float _inputDelay = 0.3f;
        [SerializeField] private KeyCode _quitKey = KeyCode.Escape;

        private float _enabledTime;

        private void OnEnable()
        {
            _enabledTime = Time.unscaledTime;
        }

        private void Update()
        {
            if (Time.unscaledTime - _enabledTime < _inputDelay)
                return;

            if (Input.GetKeyDown(_quitKey))
            {
                Quit();
                return;
            }

            bool clicked = Input.GetMouseButtonDown(0)
                           || Input.GetMouseButtonDown(1)
                           || Input.GetKeyDown(KeyCode.Space)
                           || Input.GetKeyDown(KeyCode.Return);

            if (clicked)
                StartGame();
        }

        private void StartGame()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(_gameSceneName);
        }

        private void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
