using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ChickenHunt
{
    public enum GameState
    {
        Playing,
        GameOver,
        Win,
        Pause
    }

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("HP")]
        [SerializeField] private int _maxHp = 5;
        [SerializeField] private TextMeshProUGUI _hpText;
        [SerializeField] private Slider _hpSlider;

        [Header("Score")]
        [SerializeField] private TextMeshProUGUI _scoreText;

        [Header("Win Condition")]
        [SerializeField] private int _targetKills = 10;

        [Header("End Game")]
        [SerializeField] private GameObject _gameOverPanel;
        [SerializeField] private TextMeshProUGUI _endGameText;

        [Header("Pause Menu")]
        [SerializeField] private GameObject _pausePanel;

        [Header("Scenes")]
        [SerializeField] private int _mainMenuSceneIndex = 0;

        [Header("Animation")]
        [SerializeField] private Animator _endGameAnimator;
        [SerializeField] private Animator _winGateAnimator;

        private int _hp;
        private int _score;
        private int _kills;

        public GameState State { get; private set; }
        public bool IsPlaying => State == GameState.Playing;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        private void Start()
        {
            StartGame();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (State == GameState.Playing)
                    PauseGame();
                else if (State == GameState.Pause)
                    ResumeGame();
            }
        }

        private void StartGame()
        {
            Time.timeScale = 1f;

            State = GameState.Playing;
            _hp = _maxHp;
            _score = 0;
            _kills = 0;

            if (_gameOverPanel != null)
                _gameOverPanel.SetActive(false);

            if (_pausePanel != null)
                _pausePanel.SetActive(false);

            if (_hpSlider != null)
            {
                _hpSlider.minValue = 0;
                _hpSlider.maxValue = _maxHp;
                _hpSlider.value = _hp;
                _hpSlider.wholeNumbers = true;
            }

            if (_winGateAnimator != null)
                _winGateAnimator.SetBool("IsOpen", false);

            UpdateUI();
        }

        public void AddScore(int points)
        {
            if (!IsPlaying)
                return;

            _score += points;
            _kills++;

            UpdateUI();

            if (_kills >= _targetKills)
                WinGame();
        }

        public void LoseHp(int damage)
        {
            if (!IsPlaying)
                return;

            _hp -= damage;

            if (_hp < 0)
                _hp = 0;

            UpdateUI();

            if (_hp <= 0)
                GameOver();
        }

        public void PauseGame()
        {
            if (State != GameState.Playing)
                return;

            State = GameState.Pause;

            if (_pausePanel != null)
                _pausePanel.SetActive(true);

            Time.timeScale = 0f;
        }

        public void ResumeGame()
        {
            if (State != GameState.Pause)
                return;

            State = GameState.Playing;

            if (_pausePanel != null)
                _pausePanel.SetActive(false);

            Time.timeScale = 1f;
        }

        private void WinGame()
        {
            if (!IsPlaying)
                return;

            State = GameState.Win;

            if (_endGameText != null)
                _endGameText.text = "YOU WIN";

            if (_gameOverPanel != null)
                _gameOverPanel.SetActive(true);

            if (_endGameAnimator != null)
                _endGameAnimator.SetTrigger("ShowWin");

            if (_winGateAnimator != null)
                _winGateAnimator.SetBool("IsOpen", true);

            Time.timeScale = 0f;
        }

        private void GameOver()
        {
            if (!IsPlaying)
                return;

            State = GameState.GameOver;

            if (_endGameText != null)
                _endGameText.text = "GAME OVER";

            if (_gameOverPanel != null)
                _gameOverPanel.SetActive(true);

            if (_endGameAnimator != null)
                _endGameAnimator.SetTrigger("ShowGameOver");

            Time.timeScale = 0f;
        }

        public void RestartScene()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void LoadMainMenu()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(_mainMenuSceneIndex);
        }

        public void QuitGame()
        {
            Application.Quit();

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }

        private void UpdateUI()
        {
            if (_hpText != null)
                _hpText.text = $"HP: {_hp}/{_maxHp}";

            if (_hpSlider != null)
                _hpSlider.value = _hp;

            if (_scoreText != null)
                _scoreText.text = $"Score: {_score}  Kills: {_kills}/{_targetKills}";
        }
    }
}