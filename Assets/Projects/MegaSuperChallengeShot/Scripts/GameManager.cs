using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Projects.MegaSuperChallengeShot.Scripts
{
    public enum GameState
    {
        Playing,
        Paused,
        GameOver
    }

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public event Action<GameState> StateChanged;

        [SerializeField] private GameOverUI _gameOverUI;
        [SerializeField] private string _mainMenuSceneName = "MainMenu";

        public GameState State { get; private set; } = GameState.Playing;

        private GameState _stateBeforePause = GameState.Playing;

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
            Time.timeScale = 1f;
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

        public void PauseGame()
        {
            if (State == GameState.Paused || State == GameState.GameOver)
                return;

            _stateBeforePause = State;
            Time.timeScale = 0f;
            SetState(GameState.Paused);
        }

        public void ResumeGame()
        {
            if (State != GameState.Paused)
                return;

            Time.timeScale = 1f;
            SetState(_stateBeforePause);
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
            Scene active = SceneManager.GetActiveScene();
            Time.timeScale = 1f;
            SceneManager.LoadScene(active.name);
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
