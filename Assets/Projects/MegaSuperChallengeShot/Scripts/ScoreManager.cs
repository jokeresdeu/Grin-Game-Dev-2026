using TMPro;
using UnityEngine;

namespace Projects.MegaSuperChallengeShot.Scripts
{
    public class ScoreManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private int _pointsPerBird = 10;
        [SerializeField] private ScorePopup _popupPrefab;
        [SerializeField] private Transform _popupParent;

        public static ScoreManager Instance { get; private set; }

        public int Score { get; private set; }

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
            UpdateUI();
        }

        public void AddScore()
        {
            AddScore(transform.position);
        }

        public void AddScore(Vector3 worldPosition)
        {
            if (GameManager.Instance != null && GameManager.Instance.State != GameState.Playing)
                return;

            Score += _pointsPerBird;
            UpdateUI();
            SpawnPopup(worldPosition);
        }

        private void SpawnPopup(Vector3 worldPosition)
        {
            if (_popupPrefab == null)
                return;

            Transform parent = _popupParent != null ? _popupParent : transform;
            ScorePopup popup = Instantiate(_popupPrefab, worldPosition, Quaternion.identity, parent);
            popup.Setup(_pointsPerBird);
        }

        private void UpdateUI()
        {
            if (_scoreText != null)
                _scoreText.text = $"Score: {Score}";
        }

        private void OnDestroy()
        {
            if (Instance == this)
                Instance = null;
        }
    }
}
