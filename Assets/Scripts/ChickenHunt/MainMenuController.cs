using UnityEngine;
using UnityEngine.SceneManagement;

namespace ChickenHunt
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private string _gameSceneName = "ChickenHunt";

        public void StartGame()
        {
            SceneManager.LoadScene(_gameSceneName);
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}