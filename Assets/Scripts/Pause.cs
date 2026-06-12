using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public bool PauseGame;
    public GameObject PauseMenu;

    public static string bestScore = "0";

    public Text bestScoreText;

    void Start()
    {
        bestScoreText.text = bestScore;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (PauseGame)
            {
                ResumeG();
            }
            else
            {
                PauseG();
            }
        }
    }

    public void ResumeG()
    {
        PauseMenu.SetActive(false);
        Time.timeScale = 1f;
        PauseGame = false;
    }
    public void PauseG()
    {
        PauseMenu.SetActive(true);
        Time.timeScale = 0f;
        PauseGame = true;
    }
    public void RestartG()
    {
        bestScore = bestScoreText.text;
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
    }
}
