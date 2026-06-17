using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Projects.OrbitGunner.Scripts
{
    public class GameOverUI : MonoBehaviour
    {
        [SerializeField] private GameObject _panel;
        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private TMP_Text _bestText;
        [SerializeField] private TMP_Text _badge;
        [SerializeField] private TMP_Text _prompt;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _menuButton;
        [SerializeField] private float _inputDelay = 0.5f;
        [SerializeField] private KeyCode _menuKey = KeyCode.Escape;

        private float _shownAt;
        private bool _isShown;
        private bool _subscribed;

        private void Awake()
        {
            if (_restartButton != null) _restartButton.onClick.AddListener(Restart);
            if (_menuButton != null) _menuButton.onClick.AddListener(Menu);
        }

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

            if (!_isShown && GameManager.Instance.State == GameState.GameOver)
                Show();

            if (!_isShown)
                return;

            PulsePrompt();

            if (Time.unscaledTime - _shownAt < _inputDelay)
                return;

            if (Input.GetKeyDown(_menuKey))
            {
                Menu();
                return;
            }

            if (InputReader.ConfirmDown)
                Restart();
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

        private void TrySubscribe()
        {
            if (_subscribed || GameManager.Instance == null)
                return;

            GameManager.Instance.StateChanged += OnStateChanged;
            _subscribed = true;
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

            if (_scoreText != null)
                _scoreText.text = $"Рахунок: {score:N0}";
            if (_bestText != null)
                _bestText.text = $"Рекорд: {best:N0}";

            bool isNewBest = ScoreManager.Instance != null && ScoreManager.Instance.NewBestThisRun && score > 0;
            if (_badge != null)
                _badge.gameObject.SetActive(isNewBest);
        }

        private void PulsePrompt()
        {
            if (_prompt == null)
                return;

            float t = (Mathf.Sin(Time.unscaledTime * 2.6f) + 1f) * 0.5f;
            Color c = _prompt.color;
            c.a = Mathf.Lerp(0.35f, 1f, t);
            _prompt.color = c;
        }
    }
}
