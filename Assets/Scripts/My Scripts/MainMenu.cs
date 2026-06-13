using UnityEngine;
using UnityEngine.SceneManagement;

namespace ClassicPlatformer
{
    public class MainMenu : MonoBehaviour
    {
        public void PlayGame()
        {
            SceneManager.LoadScene("ClassicPlatformer");
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}