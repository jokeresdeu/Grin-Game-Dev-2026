using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Projects.CubeHopper.Scripts
{
    public class MainMenuController : MonoBehaviour
    {
        private const string BestScoreKey = "CubeHopper.BestScore";

        [SerializeField] private string _gameSceneName = "CubeHopper_Game";
        [SerializeField] private float _inputDelay = 0.25f;
        [SerializeField] private KeyCode _quitKey = KeyCode.Escape;
        [SerializeField] private TMP_Text _bestScoreLabel;
        [SerializeField] private TMP_Text _promptLabel;
        [SerializeField] private float _promptPulseSpeed = 2.4f;
        [SerializeField] private float _promptAlphaMin = 0.45f;
        [SerializeField] private float _promptAlphaMax = 1f;

        private float _enabledTime;

        private void OnEnable()
        {
            _enabledTime = Time.unscaledTime;
        }

        private void Start()
        {
            if (_bestScoreLabel != null)
            {
                int best = PlayerPrefs.GetInt(BestScoreKey, 0);
                _bestScoreLabel.text = $"Best: {best}";
            }
        }

        private void Update()
        {
            PulsePromptLabel();

            if (Time.unscaledTime - _enabledTime < _inputDelay)
                return;

            if (Input.GetKeyDown(_quitKey))
            {
                Quit();
                return;
            }

            bool startPressed = Input.GetMouseButtonDown(0)
                                || Input.GetKeyDown(KeyCode.Space)
                                || Input.GetKeyDown(KeyCode.Return)
                                || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began);

            if (startPressed)
                StartGame();
        }

        private void PulsePromptLabel()
        {
            if (_promptLabel == null)
                return;

            float t = (Mathf.Sin(Time.unscaledTime * _promptPulseSpeed) + 1f) * 0.5f;
            float alpha = Mathf.Lerp(_promptAlphaMin, _promptAlphaMax, t);
            Color color = _promptLabel.color;
            color.a = alpha;
            _promptLabel.color = color;
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
