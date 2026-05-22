using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public static GameOver instance;
    public GameObject gameOverCanvas;
    public bool isGameOver = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void TriggerGameOver()
    {
        if (isGameOver == false)
        {
            isGameOver = true;

            if (gameOverCanvas != null)
            {
                gameOverCanvas.SetActive(true);
            }
        }
    }

    public void Restart()
    {
        StartCoroutine(RestartWithDelay());
    }

    private IEnumerator RestartWithDelay()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}