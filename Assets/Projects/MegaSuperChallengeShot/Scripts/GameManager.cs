using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Projects.MegaSuperChallengeShot.Scripts
{
    public enum GameState
    {
        Playing,
        GameOver
    }

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public event Action<GameState> StateChanged;

        [SerializeField] private GameOverUI _gameOverUI;

        public GameState State { get; private set; } = GameState.Playing;

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
            SetState(GameState.Playing);
        }

        public void TriggerGameOver()
        {
            if (State == GameState.GameOver)
                return;

            SetState(GameState.GameOver);

            if (_gameOverUI != null)
                _gameOverUI.Show(ScoreManager.Instance != null ? ScoreManager.Instance.Score : 0);
        }

        public void RestartScene()
        {
            Scene active = SceneManager.GetActiveScene();
            Time.timeScale = 1f;
            SceneManager.LoadScene(active.name);
        }

        private void SetState(GameState next)
        {
            State = next;
            StateChanged?.Invoke(next);
        }

        private void OnDestroy()
        {
            if (Instance == this)
                Instance = null;
        }
    }
}
