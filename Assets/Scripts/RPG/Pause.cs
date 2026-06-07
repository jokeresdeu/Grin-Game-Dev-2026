using UnityEngine;

namespace RPG
{
    public class Pause : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private GameObject _window;

        private bool _isPaused;

        private void Awake()
        {
            Time.timeScale = 1f;

            if (_window != null)
                _window.SetActive(false);
        }

        public void TogglePause()
        {
            _isPaused = !_isPaused;
            Time.timeScale = _isPaused ? 0f : 1f;

            if (_window != null)
                _window.SetActive(_isPaused);
        }

        public void Resume()
        {
            if (!_isPaused)
                return;

            _isPaused = false;
            Time.timeScale = 1f;

            if (_window != null)
                _window.SetActive(false);
        }

        public bool IsPaused()
        {
            return _isPaused;
        }
    }
}
