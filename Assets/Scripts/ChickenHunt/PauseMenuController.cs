using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenuPanel;
    private bool _isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_isPaused) Resume();
            else Pause();
        }
    }

    public void Resume()
    {
        _pauseMenuPanel.SetActive(false);
        Time.timeScale = 1f;
        _isPaused = false;
    }

    void Pause()
    {
        _pauseMenuPanel.SetActive(true);
        Time.timeScale = 0f;
        _isPaused = true;
    }

    public void ToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}