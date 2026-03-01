using System;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Центральний менеджер гри. Керує станом гри (Playing / GameOver),
/// рахунком, кількістю життів та перезапуском сцени.
/// Singleton — один екземпляр на сцену (без DontDestroyOnLoad,
/// щоб коректно працював перезапуск).
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game Settings")]
    [SerializeField] private int maxLives = 10;
    [SerializeField] private int maxShots = 6;
    [SerializeField] private float restartDelay = 2f;

    // --- стан ---
    private GameState _state;
    private int _score;
    private int _currentLives;
    private int _currentShots;

    // --- події для UI ---
    public event Action<int> OnScoreChanged;
    public event Action<int, int> OnLivesChanged;      // current, max
    public event Action<int, int> OnShotsChanged;       // current, max
    public event Action<GameState> OnStateChanged;

    // --- публічні властивості ---
    public GameState State => _state;
    public int Score => _score;
    public int CurrentLives => _currentLives;
    public int MaxLives => maxLives;
    public int CurrentShots => _currentShots;
    public int MaxShots => maxShots;

    // =========================================================================
    // Unity lifecycle
    // =========================================================================
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
        StartNewGame();
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    // =========================================================================
    // Game flow
    // =========================================================================
    public void StartNewGame()
    {
        _score = 0;
        _currentLives = maxLives;
        _currentShots = maxShots;
        SetState(GameState.Playing);

        OnScoreChanged?.Invoke(_score);
        OnLivesChanged?.Invoke(_currentLives, maxLives);
        OnShotsChanged?.Invoke(_currentShots, maxShots);
    }

    // --- Score ---
    public void AddScore(int amount = 1)
    {
        if (_state != GameState.Playing) return;
        _score += amount;
        OnScoreChanged?.Invoke(_score);
    }

    // --- Lives ---
    public void LoseLife()
    {
        if (_state != GameState.Playing) return;
        _currentLives = Mathf.Max(0, _currentLives - 1);
        Debug.Log($"[GameManager] Lost a life! Lives remaining: {_currentLives}");
        OnLivesChanged?.Invoke(_currentLives, maxLives);

        if (_currentLives <= 0)
        {
            Debug.Log("[GameManager] GAME OVER!");
            SetState(GameState.GameOver);
            Invoke(nameof(RestartScene), restartDelay);
        }
    }

    // --- Shots ---
    public bool TryShoot()
    {
        if (_state != GameState.Playing) return false;
        if (_currentShots <= 0) return false;

        _currentShots--;
        OnShotsChanged?.Invoke(_currentShots, maxShots);
        return true;
    }

    public void Reload()
    {
        _currentShots = maxShots;
        OnShotsChanged?.Invoke(_currentShots, maxShots);
    }

    // --- Restart ---
    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // --- State machine ---
    private void SetState(GameState newState)
    {
        _state = newState;
        Time.timeScale = newState == GameState.Playing ? 1f : 0f;
        OnStateChanged?.Invoke(_state);
    }
}
