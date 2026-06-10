using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Projects.CubeHopper.Scripts
{
    public class GameOverUI : MonoBehaviour
    {
        [SerializeField] private GameObject _panel;
        [SerializeField] private TMP_Text _scoreLabel;
        [SerializeField] private TMP_Text _bestScoreLabel;
        [SerializeField] private TMP_Text _newBestBadge;
        [SerializeField] private float _inputDelay = 0.5f;
        [SerializeField] private KeyCode _restartKey = KeyCode.Space;
        [SerializeField] private KeyCode _menuKey = KeyCode.Escape;

        private float _shownAt;
        private bool _isShown;
        private bool _isSubscribed;

        private void OnEnable()
        {
            TrySubscribe();
        }

        private void Start()
        {
            TrySubscribe();
            if (_panel != null)
                _panel.SetActive(false);
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

            if (!_isShown && GameManager.Instance.State == GameState.GameOver)
                Show();

            if (!_isShown)
                return;

            if (Time.unscaledTime - _shownAt < _inputDelay)
                return;

            if (Input.GetKeyDown(_menuKey))
            {
                GameManager.Instance.LoadMainMenu();
                return;
            }

            bool keyRestart = Input.GetKeyDown(_restartKey);
            bool mouseRestart = Input.GetMouseButtonDown(0) && !IsPointerOverUI();
            bool touchRestart = Input.touchCount > 0
                                && Input.GetTouch(0).phase == TouchPhase.Began
                                && !IsTouchOverUI(Input.GetTouch(0));

            if (keyRestart || mouseRestart || touchRestart)
                GameManager.Instance.RestartScene();
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

        private bool IsPointerOverUI()
        {
            return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
        }

        private bool IsTouchOverUI(Touch touch)
        {
            return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(touch.fingerId);
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
            if (state == GameState.GameOver && !_isShown)
                Show();
        }

        private void Show()
        {
            _isShown = true;
            _shownAt = Time.unscaledTime;

            if (_panel != null)
                _panel.SetActive(true);

            int score = ScoreManager.Instance != null ? ScoreManager.Instance.Score : 0;
            int best = ScoreManager.Instance != null ? ScoreManager.Instance.BestScore : 0;

            if (_scoreLabel != null)
                _scoreLabel.text = $"Score: {score}";
            if (_bestScoreLabel != null)
                _bestScoreLabel.text = $"Best: {best}";
            if (_newBestBadge != null)
                _newBestBadge.gameObject.SetActive(score >= best && score > 0);
        }
    }
}
