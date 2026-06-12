using UnityEngine;
using UnityEngine.SceneManagement; // Обов'язково для керування сценами

public class MainMenuManager : MonoBehaviour
{
    // Старий метод, який у тебе вже міг бути для запуску по індексу
    public void PlayGame()
    {
        // Завантажує наступну сцену в черзі (наприклад, індекс 1)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // НАШ НОВИЙ УНІВЕРСАЛЬНИЙ МЕТОД
    // Дозволяє передати назву сцени прямо з інспектора Unity
    public void LoadSceneByName(string sceneName)
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError("Назва сцени порожня! Будь ласка, вкажи її в On Click () в Unity.");
        }
    }

    // Метод для виходу з гри (для кнопки QUIT)
    public void QuitGame()
    {
        Debug.Log("Гра закрилася!"); // Працює в інспекторі
        Application.Quit(); // Працює в зібраній грі (.app / .exe)
    }
}