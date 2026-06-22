using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Projects.TowerDefense.Scripts
{
    /// <summary>
    /// Shows the Win or Lose screen based on the game state. Each screen has Restart and
    /// Main menu buttons. Uses the TrySubscribe retry pattern; also polls as a fallback so
    /// the screen never gets stuck hidden. Mirrors OrbitGunner's GameOverUI.
    /// </summary>
    public class EndScreenController : MonoBehaviour
    {
        [SerializeField] private GameObject _winPanel;
        [SerializeField] private GameObject _losePanel;
        [SerializeField] private TMP_Text _loseInfo;
        [SerializeField] private Button _winRestartButton;
        [SerializeField] private Button _winMenuButton;
        [SerializeField] private Button _loseRestartButton;
        [SerializeField] private Button _loseMenuButton;

        private bool _subscribed;
        private bool _shown;

        private void Awake()
        {
            if (_winRestartButton != null) _winRestartButton.onClick.AddListener(Restart);
            if (_winMenuButton != null) _winMenuButton.onClick.AddListener(Menu);
            if (_loseRestartButton != null) _loseRestartButton.onClick.AddListener(Restart);
            if (_loseMenuButton != null) _loseMenuButton.onClick.AddListener(Menu);
        }

        private void OnEnable()
        {
            TrySubscribe();
        }

        private void Start()
        {
            TrySubscribe();
            if (_winPanel != null) _winPanel.SetActive(false);
            if (_losePanel != null) _losePanel.SetActive(false);
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

            if (!_shown)
            {
                if (GameManager.Instance.State == GameState.Won)
                    Show(true);
                else if (GameManager.Instance.State == GameState.Lost)
                    Show(false);
            }
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
            if (_shown)
                return;

            if (state == GameState.Won)
                Show(true);
            else if (state == GameState.Lost)
                Show(false);
        }

        private void Show(bool won)
        {
            _shown = true;

            if (_winPanel != null) _winPanel.SetActive(won);
            if (_losePanel != null) _losePanel.SetActive(!won);

            if (!won && _loseInfo != null && LevelManager.Instance != null)
            {
                _loseInfo.text = $"Ви протрималися до рівня {LevelManager.Instance.LevelIndex + 1}, хвилі {LevelManager.Instance.WaveIndex + 1}.";
            }
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
    }
}
