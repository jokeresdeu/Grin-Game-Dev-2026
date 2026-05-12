using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("ChickenHunt"); 
        Time.timeScale = 1f;
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Вихід із гри");
    }
}