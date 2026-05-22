using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelPortal : MonoBehaviour
{
    [SerializeField] private int nextLevelIndex;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<RPG.Player>())
        {
            Time.timeScale = 1f; // якщо гра була на паузі
            SceneManager.LoadScene(nextLevelIndex);
        }
    }
}