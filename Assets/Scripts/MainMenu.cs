using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public TextMeshProUGUI topScoreText;
    void Start()
    {
        if (topScoreText != null)
        {
            int topScore = PlayerPrefs.GetInt("TopScore", 0);
            topScoreText.text = "Top Score: " + topScore.ToString();
        }
    }
    public void PlayGame()
    {
        StartCoroutine(LoadSceneWithDelay());
    }

    private IEnumerator LoadSceneWithDelay()
    {
        yield return new WaitForSeconds(0.5f);

        SceneManager.LoadScene("MainGame");
    }
}
