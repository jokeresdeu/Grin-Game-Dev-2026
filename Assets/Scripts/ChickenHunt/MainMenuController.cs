using UnityEngine;
using UnityEngine.SceneManagement;

namespace ChickenHunt
{
    public class MainMenuController : MonoBehaviour
    {
        public void PlayGame()
        {

            SceneManager.LoadScene(1);
        }

        public void QuitGame()
        {
            Debug.Log("Гра закривається!");
            Application.Quit();
        }
    }
}