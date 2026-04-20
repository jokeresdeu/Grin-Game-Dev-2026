using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Score")]
    [SerializeField] private TMP_Text scoreText;

    [Header("Lives")]
    [SerializeField] private Slider livesSlider;
    [SerializeField] private Image livesSliderFill;
    [SerializeField] private TMP_Text livesText;

    [Header("Shots")]
    [SerializeField] private TMP_Text shotsText;

    [Header("Game Over")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TMP_Text gameOverScoreText;
    [SerializeField] private Button restartButton;

    private void OnEnable()
    {
        if (GameManager.Instance == null) return;
        Subscribe();
    }

    private void Start()
    {
        Subscribe();

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (restartButton != null)
            restartButton.onClick.AddListener(OnRestartClicked);
    }

    private void OnDisable()
    {
        Unsubscribe();
    }

    private bool _subscribed;

    private void Subscribe()
    {
        if (_subscribed || GameManager.Instance == null) return;
        _subscribed = true;

        GameManager.Instance.OnScoreChanged += UpdateScore;
        GameManager.Instance.OnLivesChanged += UpdateLives;
        GameManager.Instance.OnShotsChanged += UpdateShots;
        GameManager.Instance.OnStateChanged += UpdateState;

        UpdateScore(GameManager.Instance.Score);
        UpdateLives(GameManager.Instance.CurrentLives, GameManager.Instance.MaxLives);
        UpdateShots(GameManager.Instance.CurrentShots, GameManager.Instance.MaxShots);
    }

    private void Unsubscribe()
    {
        if (!_subscribed || GameManager.Instance == null) return;
        _subscribed = false;

        GameManager.Instance.OnScoreChanged -= UpdateScore;
        GameManager.Instance.OnLivesChanged -= UpdateLives;
        GameManager.Instance.OnShotsChanged -= UpdateShots;
        GameManager.Instance.OnStateChanged -= UpdateState;
    }

    private void UpdateScore(int score)
    {
        if (scoreText != null)
            scoreText.text = $"Score: {score}";
    }

    private void UpdateLives(int current, int max)
    {
        if (livesSlider != null)
        {
            livesSlider.maxValue = max;
            livesSlider.value = current;
        }

        if (livesSliderFill != null)
        {
            float ratio = (float)current / max;
            livesSliderFill.color = ratio > 0.5f ? Color.green :
                                    ratio > 0.25f ? Color.yellow : Color.red;
        }

        if (livesText != null)
            livesText.text = $"HP: {current}/{max}";
    }

    private void UpdateShots(int current, int max)
    {
        if (shotsText != null)
            shotsText.text = $"Shots: {current}/{max}";
    }

    private void UpdateState(GameState state)
    {
        if (state == GameState.GameOver)
        {
            ShowGameOver();
        }
    }

    private void ShowGameOver()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        if (gameOverScoreText != null)
            gameOverScoreText.text = $"Final Score: {GameManager.Instance.Score}";
    }

    private void OnRestartClicked()
    {
        Time.timeScale = 1f;
        GameManager.Instance.RestartScene();
    }
}
