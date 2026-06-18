using Projects.MegaSuperChallengeShot.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool IsGameOver { get; private set; }

    [Header("UI References")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI topScoreText;
    [SerializeField] private TextMeshProUGUI currentScoreText;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void GameOver()
    {
        IsGameOver = true;
        Time.timeScale = 0f;

        if (ScoreManagerNew.Instance != null)
        {
            ScoreManagerNew.Instance.SaveTopScore();

            if (currentScoreText != null)
            {
                currentScoreText.text = "Score: " + ScoreManagerNew.Instance.Score.ToString();
            }
        }

        if (topScoreText != null)
        {
            topScoreText.text = "Top Score: " + PlayerPrefs.GetInt("TopScore", 0).ToString();
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}