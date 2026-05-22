using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ClassicPlatformer
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public enum GameState { Playing, GameOver }

        public static int Score { get; private set; }

        public GameState CurrentState { get; private set; } = GameState.Playing;

        public event Action<int> OnScoreChanged;
        public event Action OnGameOver;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            Score = 0;
        }

        // Додає очки (монети через CoinCollectible)
        public void AddScore(int amount)
        {
            if (CurrentState != GameState.Playing) return;
            Score += amount;
            OnScoreChanged?.Invoke(Score);
        }

        // Зворотна сумісність зі старим кодом
        public void AddCoins(int amount) => AddScore(amount);

        public void TriggerGameOver()
        {
            if (CurrentState == GameState.GameOver) return;
            CurrentState = GameState.GameOver;
            OnGameOver?.Invoke();
        }

        public void RestartGame()
        {
            Score = 0;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
