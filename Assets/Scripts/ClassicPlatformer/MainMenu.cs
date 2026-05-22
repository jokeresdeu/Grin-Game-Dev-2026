using UnityEngine;
using UnityEngine.SceneManagement;

namespace ClassicPlatformer
{
    public class MainMenu : MonoBehaviour
    {
        public void Play()
        {
            SceneManager.LoadScene(1);
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}