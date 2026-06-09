using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI recordText;
    public GameObject[] kunaiIcons;
    public GameObject mainMenuPanel;
    public GameObject gameOverPanel;
    public GameObject pausePanel;

    public int Score { get; private set; }
    public int Record { get; private set; }
    public int KunaiCount { get; private set; }
    public bool IsGameOver { get; private set; }
    public bool IsPaused { get; private set; }
    public bool HasStarted { get; private set; }

    static bool autoStart;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        Record = PlayerPrefs.GetInt("NinjaRecord", 0);
    }

    void Start()
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (pausePanel != null)    pausePanel.SetActive(false);

        if (autoStart)
        {
            autoStart = false;
            HasStarted = true;
            if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
            CubeSpawner.Instance?.BeginFirstWave();
        }
        else
        {
            if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
        }

        RefreshUI();
    }

    void Update()
    {
        if (HasStarted && !IsGameOver && Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
            TogglePause();
    }

    public void StartGame()
    {
        HasStarted = true;
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        CubeSpawner.Instance?.BeginFirstWave();
    }

    public void SetInitialKunaiCount(int count)
    {
        KunaiCount = count;
        RefreshUI();
    }

    public void AddScore(int amount)
    {
        if (IsGameOver) return;
        Score += amount;
        if (Score > Record)
        {
            Record = Score;
            PlayerPrefs.SetInt("NinjaRecord", Record);
        }
        RefreshUI();
    }

    public void LoseKunai()
    {
        if (IsGameOver) return;
        KunaiCount = Mathf.Max(0, KunaiCount - 1);
        RefreshUI();
    }

    public void GainKunai()
    {
        if (IsGameOver) return;
        KunaiCount++;
        RefreshUI();
    }

    public void NotifyAllKunaiGone()
    {
        if (!IsGameOver && KunaiCount <= 0)
            TriggerGameOver();
    }

    void TriggerGameOver()
    {
        if (IsPaused) TogglePause();
        IsGameOver = true;
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
    }

    public void TogglePause()
    {
        IsPaused = !IsPaused;
        Time.timeScale = IsPaused ? 0f : 1f;
        if (pausePanel != null) pausePanel.SetActive(IsPaused);
    }

    public void Resume()
    {
        if (IsPaused) TogglePause();
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        autoStart = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    void RefreshUI()
    {
        if (scoreText != null)  scoreText.text  = "SCORE\n" + Score.ToString();
        if (recordText != null) recordText.text = "RECORD\n" + Record.ToString();

        if (kunaiIcons == null) return;
        for (int i = 0; i < kunaiIcons.Length; i++)
            if (kunaiIcons[i] != null)
                kunaiIcons[i].SetActive(i < KunaiCount);
    }
}
