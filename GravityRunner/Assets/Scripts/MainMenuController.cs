using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuController : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI menuHighScoreText;

    void Start()
    {
        int highScore = PlayerPrefs.GetInt("HighScore", 0);

        if (menuHighScoreText != null)
        {
            menuHighScoreText.text = "BEST: " + highScore;
        }
    }

    public void StartGame()
    {
        if (SceneTransition.instance != null)
        {
            SceneTransition.instance.LoadScene("GameScene");
        }
        else
        {
            SceneManager.LoadScene("GameScene");
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}