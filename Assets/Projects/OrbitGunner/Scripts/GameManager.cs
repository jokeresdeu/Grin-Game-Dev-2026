using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Projects.OrbitGunner.Scripts
{

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public event Action<GameState> StateChanged;

        private string _mainMenuSceneName = "OrbitGunner_MainMenu";
        private string _gameSceneName = "OrbitGunner_Game";
        private float _gameOverInputLockSeconds = 0.5f;

        public GameState State { get; private set; } = GameState.Playing;
        public float GameOverInputUnlockTime { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            EnemyRegistry.Clear();
        }

        private void Start()
        {
            Time.timeScale = 1f;
            SetState(GameState.Playing);
        }

        public void TriggerGameOver()
        {
            if (State == GameState.GameOver)
                return;

            if (ScoreManager.Instance != null)
                ScoreManager.Instance.CommitBestScore();

            GameOverInputUnlockTime = Time.unscaledTime + _gameOverInputLockSeconds;
            SetState(GameState.GameOver);
        }

        public void PauseGame()
        {
            if (State != GameState.Playing)
                return;

            Time.timeScale = 0f;
            SetState(GameState.Paused);
        }

        public void ResumeGame()
        {
            if (State != GameState.Paused)
                return;

            Time.timeScale = 1f;
            SetState(GameState.Playing);
        }

        public void TogglePause()
        {
            if (State == GameState.Paused)
                ResumeGame();
            else if (State == GameState.Playing)
                PauseGame();
        }

        public void RestartScene()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(_gameSceneName);
        }

        public void LoadMainMenu()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(_mainMenuSceneName);
        }

        public void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
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
