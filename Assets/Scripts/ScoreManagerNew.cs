using UnityEngine;
using TMPro;

public class ScoreManagerNew : MonoBehaviour
{
    public static ScoreManagerNew Instance { get; private set; }

    public int Score { get; private set; }

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI scoreText;

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

    void Start()
    {
        UpdateScoreUI();
    }

    public void AddScore(int amount)
    {
        Score += amount;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + Score.ToString();
        }
    }

    public void SaveTopScore()
    {
        int currentTopScore = PlayerPrefs.GetInt("TopScore", 0);
        if (Score > currentTopScore)
        {
            PlayerPrefs.SetInt("TopScore", Score);
            PlayerPrefs.Save();
        }
    }
}