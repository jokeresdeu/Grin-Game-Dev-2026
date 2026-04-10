using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    public static GameUI Instance;

    public int score = 0;
    public int baseHP = 5;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI hpText;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateScoreUI();
        UpdateHPUI();
    }

    public void AddScore(int value)
    {
        score += value;
        UpdateScoreUI();
    }

    public void LoseHP(int value)
    {
        baseHP -= value;

        if (baseHP < 0)
            baseHP = 0;

        UpdateHPUI();

        if (baseHP <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }

    private void UpdateHPUI()
    {
        if (hpText != null)
            hpText.text = "HP: " + baseHP;
    }
}