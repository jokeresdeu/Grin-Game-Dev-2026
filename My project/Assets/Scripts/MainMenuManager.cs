using UnityEngine;
using UnityEngine.SceneManagement; 

public class MainMenuManager : MonoBehaviour
{
    public void StartGame()
    {
        // Завантажуємо ігрову сцену.
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        // Виходимо з гри 
        Application.Quit();
        Debug.Log("Ви вийшли з гри!"); 
    }
}
