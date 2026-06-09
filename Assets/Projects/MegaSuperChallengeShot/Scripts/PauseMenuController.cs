using UnityEngine;
using UnityEngine.UI;

namespace Projects.MegaSuperChallengeShot.Scripts
{
    public class PauseMenuController : MonoBehaviour
    {
        [SerializeField] private GameObject _panel;
        [SerializeField] private Button _resumeButton;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _quitButton;
        [SerializeField] private KeyCode _toggleKey = KeyCode.Escape;

        private void Awake()
        {
            if (_panel != null)
                _panel.SetActive(false);

            if (_resumeButton != null)
                _resumeButton.onClick.AddListener(OnResumeClicked);

            if (_restartButton != null)
                _restartButton.onClick.AddListener(OnRestartClicked);

            if (_quitButton != null)
                _quitButton.onClick.AddListener(OnQuitClicked);
        }

        private void OnEnable()
        {
            if (GameManager.Instance != null)
                GameManager.Instance.StateChanged += OnStateChanged;
        }

        private void OnDisable()
        {
            if (GameManager.Instance != null)
                GameManager.Instance.StateChanged -= OnStateChanged;
        }

        private void Update()
        {
            if (GameManager.Instance == null)
                return;

            if (GameManager.Instance.State == GameState.GameOver)
                return;

            if (Input.GetKeyDown(_toggleKey))
                GameManager.Instance.TogglePause();
        }

        private void OnStateChanged(GameState state)
        {
            if (_panel != null)
                _panel.SetActive(state == GameState.Paused);
        }

        private void OnResumeClicked()
        {
            if (GameManager.Instance != null)
                GameManager.Instance.ResumeGame();
        }

        private void OnRestartClicked()
        {
            if (GameManager.Instance != null)
                GameManager.Instance.RestartScene();
        }

        private void OnQuitClicked()
        {
            if (GameManager.Instance != null)
                GameManager.Instance.LoadMainMenu();
        }

        private void OnDestroy()
        {
            if (_resumeButton != null)
                _resumeButton.onClick.RemoveListener(OnResumeClicked);

            if (_restartButton != null)
                _restartButton.onClick.RemoveListener(OnRestartClicked);

            if (_quitButton != null)
                _quitButton.onClick.RemoveListener(OnQuitClicked);
        }
    }
}
