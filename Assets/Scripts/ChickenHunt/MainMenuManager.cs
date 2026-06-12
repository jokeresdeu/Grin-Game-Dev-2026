using UnityEngine;
using UnityEngine.SceneManagement;

namespace ChickenHunt
{
    public class MainMenuManager : MonoBehaviour
    {
        [Header("Назва ігрової сцени")]
        [SerializeField] private string _gameSceneName = "ChickenHunt"; // Перевіримо назву пізніше

        // Цей метод викликатиметься при натисканні на кнопку PLAY
        public void PlayGame()
        {
            SceneManager.LoadScene(_gameSceneName);
        }

        // Цей метод викликатиметься при натисканні на кнопку QUIT
        public void QuitGame()
        {
            Debug.Log("Гра закрилася (це спрацює в білді)");
            Application.Quit();
        }
    }
}