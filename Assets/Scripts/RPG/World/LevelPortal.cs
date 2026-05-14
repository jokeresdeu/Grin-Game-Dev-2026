using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelPortal : MonoBehaviour
{
    [SerializeField] private int nextLevelIndex = 2;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<RPG.Player>())
        {
            Time.timeScale = 1f; // якщо гра була на паузі
            SceneManager.LoadScene(nextLevelIndex);
        }
    }
    public void LoadNextLevel()
    {
        Time.timeScale = 1f; // якщо була пауза
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Debug.Log("Game is quitting...");
        Application.Quit();
    }
}