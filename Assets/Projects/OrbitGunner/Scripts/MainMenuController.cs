using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Projects.OrbitGunner.Scripts
{
    public class MainMenuController : MonoBehaviour
    {
        private const string BestScoreKey = "OrbitGunner.BestScore";

        [SerializeField] private string _gameSceneName = "OrbitGunner_Game";
        [SerializeField] private TMP_Text _bestText;
        [SerializeField] private TMP_Text _prompt;
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _quitButton;
        [SerializeField] private float _inputDelay = 0.25f;

        private float _enabledTime;

        private void Awake()
        {
            if (_playButton != null) _playButton.onClick.AddListener(StartGame);
            if (_quitButton != null) _quitButton.onClick.AddListener(Quit);
        }

        private void OnEnable()
        {
            _enabledTime = Time.unscaledTime;
        }

        private void Start()
        {
            if (_bestText != null)
                _bestText.text = $"Рекорд: {PlayerPrefs.GetInt(BestScoreKey, 0):N0}";
        }

        private void Update()
        {
            PulsePrompt();

            if (Time.unscaledTime - _enabledTime < _inputDelay)
                return;

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Quit();
                return;
            }

            if (InputReader.ConfirmDown)
                StartGame();
        }

        private void PulsePrompt()
        {
            if (_prompt == null)
                return;

            float t = (Mathf.Sin(Time.unscaledTime * 2.4f) + 1f) * 0.5f;
            Color c = _prompt.color;
            c.a = Mathf.Lerp(0.4f, 1f, t);
            _prompt.color = c;
        }

        public void StartGame()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(_gameSceneName);
        }

        public void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
