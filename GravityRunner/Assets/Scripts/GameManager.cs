using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("UI Елементи")]
    public TextMeshProUGUI hudScoreText;
    public TextMeshProUGUI currentScoreText;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI coinText;

    [Header("Панелі Меню")]
    public GameObject gameOverPanel;
    public GameObject pausePanel;

    public float globalSpeedMultiplier = 1f;

    private int score = 0;
    private int totalCoins = 0;
    private bool isGameOver = false;
    private bool isPaused = false;

    private Coroutine scoreBounceCoroutine;
    private Coroutine coinBounceCoroutine;
    public static GameManager instance;

    void Awake()
    {
        if (instance == null) instance = this;
    }

    void Start()
    {
        Time.timeScale = 1f;
        score = 0;
        globalSpeedMultiplier = 1f;
        hudScoreText.text = "Score: 0";

        totalCoins = PlayerPrefs.GetInt("TotalCoins", 0);
        if (coinText != null) coinText.text = "Coins: " + totalCoins;

        gameOverPanel.SetActive(false);
        pausePanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isGameOver)
        {
            if (isPaused) ResumeGame();
            else PauseGame();
        }
    }

    public void GameOver()
    {
        if (isGameOver) return;
        isGameOver = true;
        Time.timeScale = 0f;

        currentScoreText.text = "CURRENT SCORE: " + score;

        int bestScore = PlayerPrefs.GetInt("HighScore", 0);

        if (score > bestScore)
        {
            bestScore = score;
            PlayerPrefs.SetInt("HighScore", bestScore);
            PlayerPrefs.Save();
        }

        highScoreText.text = "BEST SCORE: " + bestScore;

        gameOverPanel.SetActive(true);

        if (AudioManager.instance != null)
        {
            AudioManager.instance.SetMusicMuffled(true);
        }
    }

    public void AddScore()
    {
        if (!isGameOver)
        {
            score++;
            hudScoreText.text = "Score: " + score;
            globalSpeedMultiplier += 0.05f;

            if (scoreBounceCoroutine != null) StopCoroutine(scoreBounceCoroutine);
            scoreBounceCoroutine = StartCoroutine(BounceText(hudScoreText));
        }
    }

    public void AddCoin()
    {
        if (!isGameOver)
        {
            totalCoins++;
            PlayerPrefs.SetInt("TotalCoins", totalCoins);
            PlayerPrefs.Save();

            if (AudioManager.instance != null)
                AudioManager.instance.PlaySFX(AudioManager.instance.coinSound);

            if (coinText != null)
            {
                coinText.text = "Coins: " + totalCoins;

                if (coinBounceCoroutine != null) StopCoroutine(coinBounceCoroutine);
                coinBounceCoroutine = StartCoroutine(BounceText(coinText));
            }
        }
    }

    System.Collections.IEnumerator BounceText(TextMeshProUGUI textElement)
    {
        if (textElement == null) yield break;

        Transform textTransform = textElement.transform;
        Vector3 originalScale = Vector3.one;
        float bumpScale = 1.1f;

        float t = 0;
        while (t < 1f)
        {
            t += Time.unscaledDeltaTime * 25f;
            textTransform.localScale = Vector3.Lerp(originalScale, originalScale * bumpScale, t);
            yield return null;
        }

        t = 0;
        while (t < 1f)
        {
            t += Time.unscaledDeltaTime * 15f;
            textTransform.localScale = Vector3.Lerp(originalScale * bumpScale, originalScale, t);
            yield return null;
        }

        textTransform.localScale = originalScale;
    }

    public void RestartGame()
    {
        if (SceneTransition.instance != null)
            SceneTransition.instance.LoadScene(SceneManager.GetActiveScene().name);
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadMainMenu()
    {
        if (SceneTransition.instance != null)
            SceneTransition.instance.LoadScene("MainMenu");
        else
            SceneManager.LoadScene("MainMenu");
    }

    public void PauseGame()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        if (AudioManager.instance != null) AudioManager.instance.SetMusicMuffled(true);
    }

    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        if (AudioManager.instance != null) AudioManager.instance.SetMusicMuffled(false);
    }
}