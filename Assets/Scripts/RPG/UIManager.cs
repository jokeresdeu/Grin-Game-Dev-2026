using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RPG {
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;

        [Header("Panels")]
        [SerializeField] private GameObject _introPanel;
        [SerializeField] private GameObject _pausePanel;
        [SerializeField] private GameObject _indicatorsPanel;

        [Header("Intro")]
        [SerializeField] private TMP_InputField _nameInput;

        [Header("Indicators UI")]
        [SerializeField] private TMP_Text _playerNameText;
        [SerializeField] private TMP_Text _timeText;      
        [SerializeField] private Image _hpFill;           
        [SerializeField] private Image _manaFill;         

        [Header("Player Ref")]
        [SerializeField] private Player _player;

        private bool _isGameStarted;
        private bool _isPaused;

        private float _timeInRaid;
        private string _playerName;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            ShowIntro();
        }

        private void Update()
        {
            if (!_isGameStarted) return;

            _timeInRaid += Time.deltaTime;
            UpdateTimeUI();

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (_isPaused)
                    ResumeGame();
                else
                    PauseGame();
            }
            UpdateBars();
        }


        private void ShowIntro()
        {
            Time.timeScale = 0f;

            _introPanel.SetActive(true);
            _pausePanel.SetActive(false);
            _indicatorsPanel.SetActive(false);
            _isGameStarted = false;
        }

        public void OnPlayPressed()
        {
            if (string.IsNullOrEmpty(_nameInput.text))
            {
                Debug.Log("Enter name!");
                return;
            }

            _playerName = _nameInput.text;
            _playerNameText.text = _playerName;

            _introPanel.SetActive(false);
            _indicatorsPanel.SetActive(true);

            Time.timeScale = 1f;
            _isGameStarted = true;

            _timeInRaid = 0f;
        }

        private void PauseGame()
        {
            _pausePanel.SetActive(true);
            Time.timeScale = 0f;
            _isPaused = true;
        }

        private void ResumeGame()
        {
            _pausePanel.SetActive(false);
            Time.timeScale = 1f;
            _isPaused = false;
        }

        public void OnResumePressed()
        {
            ResumeGame();
        }

        public void OnRestartPressed()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void OnQuitPressed()
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
        private void UpdateTimeUI()
        {
            int minutes = Mathf.FloorToInt(_timeInRaid / 60f);
            int seconds = Mathf.FloorToInt(_timeInRaid % 60f);
            _timeText.text = $"{minutes:00}:{seconds:00}";
        }

        private void UpdateBars()
        {
            if (_player == null) return;
            _hpFill.fillAmount = (float)_player.CurrentHealth / _player.MaxHealth;
            _manaFill.fillAmount = (float)_player.CurrentMana / _player.MaxMana;
        }
    }
}