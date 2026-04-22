using TMPro;
using UnityEngine;

namespace Projects.MegaSuperChallengeShot.Scripts
{
    public class ScoreManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text _scoreText;
        public static ScoreManager Instance { get; private set; }
        public TMP_Text ScoreText => _scoreText;

        private int _score;

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
            _score = 0;
            _scoreText.text = $"Score: {_score}";
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }

        public void AddScore()
        {
            _score++;
            _scoreText.text = $"Score: {_score}";
        }

        public void AddScore(int amount)
        {
            _score += amount;
            _scoreText.text = $"Score: {_score}";
        }

        public int GetScore()
        {
            return _score;
        }

        public void ResetScore()
        {
            _score = 0;
            _scoreText.text = $"Score: {_score}";
        }
    }
}
