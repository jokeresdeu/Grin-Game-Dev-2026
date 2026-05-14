using System;
using TMPro;
using UnityEngine;

namespace Projects.MegaSuperChallengeShot.Scripts
{
    public class ScoreManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text _scoreText;
        public static ScoreManager Instance { get; private set; }

        private int _score;

        private void Start()
        {
            _scoreText.text = $"Score: {0}";
        }
        
        private void Awake()
        {
            if(Instance != null)
                Destroy(gameObject);
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        public void AddScore()
        {
            _score++;
            _scoreText.text = $"Score: {_score}";
        }
    }
}