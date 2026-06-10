using UnityEngine;
using UnityEngine.SceneManagement;

namespace ChickenHunt
{
    public class MainMenu : MonoBehaviour
    {
        [Header("Scene Index")]
        [SerializeField] private int _gameSceneIndex = 1;

        public void StartGame()
        {
            Debug.Log("START GAME CLICKED");

            Time.timeScale = 1f;
            SceneManager.LoadScene(_gameSceneIndex);
        }

        public void QuitGame()
        {
            Debug.Log("QUIT GAME CLICKED");

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}