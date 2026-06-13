using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG
{
    public class MainMenuController : MonoBehaviour
    {
        public void PlayGame()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("RPG");
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}

