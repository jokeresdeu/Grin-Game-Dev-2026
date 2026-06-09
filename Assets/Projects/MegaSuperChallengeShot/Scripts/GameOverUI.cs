using TMPro;
using UnityEngine;

namespace Projects.MegaSuperChallengeShot.Scripts
{
    public class GameOverUI : MonoBehaviour
    {
        [SerializeField] private GameObject _panel;
        [SerializeField] private TMP_Text _finalScoreText;
        [SerializeField] private GameOverPanelAnimator _panelAnimator;
        [SerializeField] private float _restartInputDelay = 0.5f;

        private bool _isShown;
        private float _showTime;

        private void Awake()
        {
            if (_panel != null)
                _panel.SetActive(false);
        }

        public void Show(int finalScore)
        {
            if (_panel != null)
                _panel.SetActive(true);

            if (_finalScoreText != null)
                _finalScoreText.text = $"Final Score: {finalScore}\n\nClick anywhere to restart";

            if (_panelAnimator != null)
                _panelAnimator.PlayShow();

            _isShown = true;
            _showTime = Time.unscaledTime;
        }

        private void Update()
        {
            if (!_isShown)
                return;

            if (Time.unscaledTime - _showTime < _restartInputDelay)
                return;

            bool clicked = Input.GetMouseButtonDown(0)
                           || Input.GetMouseButtonDown(1)
                           || Input.GetKeyDown(KeyCode.Space)
                           || Input.GetKeyDown(KeyCode.Return)
                           || Input.GetKeyDown(KeyCode.R);

            if (!clicked)
                return;

            _isShown = false;

            if (GameManager.Instance != null)
                GameManager.Instance.RestartScene();
        }
    }
}
