using UnityEngine;

namespace ClassicPlatformer
{
    public class StartScreenUI : MonoBehaviour
    {
        [SerializeField] private GameObject _startPanel;
        [SerializeField] private GameObject _inGamePanel;
        [SerializeField] private UIManager _uiManager;

        private bool _started;

        private void Start()
        {
            Time.timeScale = 0f;
            _startPanel.SetActive(true);
            _inGamePanel.SetActive(false);
        }

        private void Update()
        {
            if (_started) return;
            if (!Input.GetKeyDown(KeyCode.Space)) return;

            _started = true;
            _startPanel.SetActive(false);
            _inGamePanel.SetActive(true);
            Time.timeScale = 1f;
            _uiManager?.StartTimer();
        }
    }
}
