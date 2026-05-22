using UnityEngine;
using UnityEngine.SceneManagement; 

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverMenuUI; 

    public void EnableGameOver()
    {
        if (gameOverMenuUI != null)
        {
            gameOverMenuUI.SetActive(true); 
            Time.timeScale = 0f;        

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
    }
}