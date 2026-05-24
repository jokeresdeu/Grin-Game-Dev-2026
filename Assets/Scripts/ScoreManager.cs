using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance; // Дозволяє літаку легко надсилати сюди дані

    [Header("Спрайті цифр ВІД 0 ДО 9")]
    public Sprite[] numberSprites;

    public GameObject digitPrefab;

    private int score = 0;
    private List<GameObject> activeDigits = new List<GameObject>();

    void Awake()
    {
        instance = this; // Ініціалізуємо інстанс
    }

    void Start()
    {
        UpdateScoreDisplay();
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreDisplay();
    }

    private void UpdateScoreDisplay()
    {
        // Видаляємо старі цифри
        foreach (var digit in activeDigits)
        {
            Destroy(digit);
        }
        activeDigits.Clear();

        string scoreString = score.ToString();

        // Створюємо нові картинки для кожної цифри
        foreach (char c in scoreString)
        {
            int digitValue = int.Parse(c.ToString());

            GameObject newDigit = Instantiate(digitPrefab, transform);
            newDigit.GetComponent<Image>().sprite = numberSprites[digitValue];
            newDigit.GetComponent<Image>().SetNativeSize(); // Робить цифру оригінального розміру
            activeDigits.Add(newDigit);
        }
    }

    public int GetScore()
    {
        return score;
    }
}