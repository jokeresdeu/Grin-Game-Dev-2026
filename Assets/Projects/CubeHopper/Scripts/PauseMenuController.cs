using UnityEngine;

namespace Projects.CubeHopper.Scripts
{
    public class PauseMenuController : MonoBehaviour
    {
        [SerializeField] private GameObject _panel;
        [SerializeField] private KeyCode _toggleKey = KeyCode.Escape;

        private bool _isSubscribed;

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
            if (_isSubscribed && GameManager.Instance != null)
            {
                GameManager.Instance.StateChanged -= OnStateChanged;
                _isSubscribed = false;
            }
        }

        private void Update()
        {
            if (GameManager.Instance == null)
                return;

            if (!_isSubscribed)
                TrySubscribe();

            if (Input.GetKeyDown(_toggleKey))
            {
                if (GameManager.Instance.State == GameState.Playing
                    || GameManager.Instance.State == GameState.Paused)
                {
                    GameManager.Instance.TogglePause();
                }
            }

            Refresh();
        }

        public void OnResumeClicked()
        {
            if (GameManager.Instance != null)
                GameManager.Instance.ResumeGame();
        }

        public void OnRestartClicked()
        {
            if (GameManager.Instance != null)
                GameManager.Instance.RestartScene();
        }

        public void OnMainMenuClicked()
        {
            if (GameManager.Instance != null)
                GameManager.Instance.LoadMainMenu();
        }

        public void OnQuitClicked()
        {
            if (GameManager.Instance != null)
                GameManager.Instance.QuitGame();
        }

        private void TrySubscribe()
        {
            if (_isSubscribed || GameManager.Instance == null)
                return;

            GameManager.Instance.StateChanged += OnStateChanged;
            _isSubscribed = true;
        }

        private void OnStateChanged(GameState state)
        {
            Refresh();
        }

        private void Refresh()
        {
            if (_panel == null)
                return;

            bool shouldShow = GameManager.Instance != null
                              && GameManager.Instance.State == GameState.Paused;

            if (_panel.activeSelf != shouldShow)
                _panel.SetActive(shouldShow);
        }
    }
}
