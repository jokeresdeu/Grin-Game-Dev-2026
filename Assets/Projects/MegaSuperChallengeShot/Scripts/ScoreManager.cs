using UnityEngine;
using TMPro; 

public class ScoreManager : MonoBehaviour
{
    public int score = 0;
    public int targetScore = 5; 
    public TextMeshProUGUI scoreText;

    public GameObject winMenuUI; 

    public void AddScore()
    {
        score++;
        UpdateUI();

        if (score >= targetScore)
        {
            WinGame();
        }
    }

    void UpdateUI()
    {
        scoreText.text = "Score: " + score;
    }

    void WinGame()
    {
        winMenuUI.SetActive(true);
        Time.timeScale = 0f;     
        Cursor.visible = true;    
        Cursor.lockState = CursorLockMode.None;
    }
}