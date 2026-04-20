using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenus : MonoBehaviour
{
    [Header("Налаштування панелей")]
    public GameObject mainMenuPanel;
    public GameObject pauseMenuPanel;

    [Header("Ігровий Світ")]
    public GameObject gameplayHolder; // Сюди ми покладемо наш об'єкт Gameplay

    private bool isGameStarted = false;

    void Start()
    {
        mainMenuPanel.SetActive(true);
        pauseMenuPanel.SetActive(false);

        // ВИМИКАЄМО гру повністю на старті
        gameplayHolder.SetActive(false);

        Time.timeScale = 0f;
    }

    public void StartGame()
    {
        mainMenuPanel.SetActive(false);

        // ВМИКАЄМО гру, коли гравець натиснув кнопку
        gameplayHolder.SetActive(true);

        Time.timeScale = 1f;
        isGameStarted = true;
    }

    public void PauseGame()
    {
        pauseMenuPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        pauseMenuPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BackToMainMenu()
    {
        RestartGame();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}