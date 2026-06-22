using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Projects.TowerDefense.Scripts
{
    /// <summary>
    /// Singleton state machine for the game scene. Owns the <see cref="GameState"/>,
    /// Time.timeScale, and scene-flow. Mirrors OrbitGunner's GameManager but with
    /// Won/Lost outcomes instead of a single GameOver.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public event Action<GameState> StateChanged;

        [SerializeField] private string _mainMenuSceneName = "TowerDefense_MainMenu";
        [SerializeField] private string _gameSceneName = "TowerDefense_Game";

        public GameState State { get; private set; } = GameState.Playing;

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

        public void TriggerWin()
        {
            if (State == GameState.Won || State == GameState.Lost)
                return;

            Time.timeScale = 1f;
            SetState(GameState.Won);
        }

        public void TriggerLose()
        {
            if (State == GameState.Won || State == GameState.Lost)
                return;

            Time.timeScale = 1f;
            SetState(GameState.Lost);
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
