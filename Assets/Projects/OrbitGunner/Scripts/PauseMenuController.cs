using UnityEngine;
using UnityEngine.UI;

namespace Projects.OrbitGunner.Scripts
{
    public class PauseMenuController : MonoBehaviour
    {
        [SerializeField] private GameObject _panel;
        [SerializeField] private Button _resumeButton;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _menuButton;
        [SerializeField] private Button _quitButton;
        [SerializeField] private KeyCode _toggleKey = KeyCode.Escape;

        private bool _subscribed;

        private void Awake()
        {
            if (_resumeButton != null) _resumeButton.onClick.AddListener(Resume);
            if (_restartButton != null) _restartButton.onClick.AddListener(Restart);
            if (_menuButton != null) _menuButton.onClick.AddListener(Menu);
            if (_quitButton != null) _quitButton.onClick.AddListener(Quit);
        }

        private void OnEnable()
        {
            TrySubscribe();
            Refresh();
        }

        private void Start()
        {
            TrySubscribe();
            Refresh();
        }

        private void OnDisable()
        {
            if (_subscribed && GameManager.Instance != null)
            {
                GameManager.Instance.StateChanged -= OnStateChanged;
                _subscribed = false;
            }
        }

        private void Update()
        {
            if (GameManager.Instance == null)
                return;

            if (!_subscribed)
                TrySubscribe();

            if (Input.GetKeyDown(_toggleKey)
                && (GameManager.Instance.State == GameState.Playing || GameManager.Instance.State == GameState.Paused))
            {
                GameManager.Instance.TogglePause();
            }

            Refresh();
        }

        public void Resume()
        {
            if (GameManager.Instance != null)
                GameManager.Instance.ResumeGame();
        }

        public void Restart()
        {
            if (GameManager.Instance != null)
                GameManager.Instance.RestartScene();
        }

        public void Menu()
        {
            if (GameManager.Instance != null)
                GameManager.Instance.LoadMainMenu();
        }

        public void Quit()
        {
            if (GameManager.Instance != null)
                GameManager.Instance.QuitGame();
        }

        private void TrySubscribe()
        {
            if (_subscribed || GameManager.Instance == null)
                return;

            GameManager.Instance.StateChanged += OnStateChanged;
            _subscribed = true;
        }

        private void OnStateChanged(GameState state)
        {
            Refresh();
        }

        private void Refresh()
        {
            if (_panel == null)
                return;

            bool shouldShow = GameManager.Instance != null && GameManager.Instance.State == GameState.Paused;
            if (_panel.activeSelf != shouldShow)
                _panel.SetActive(shouldShow);
        }
    }
}
