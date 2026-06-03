using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
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

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}