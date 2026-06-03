using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("ClassicPlatformer");
    }

    public void QuitGame()
    {
        Debug.Log("Гравець натиснув вихід!");

        Application.Quit();
    }
}