using UnityEngine;

public class UISwayEffect : MonoBehaviour
{
    [Header("Налаштування гойдання")]
    public float speed = 3f;      // Швидкість похитування
    public float angle = 8f;      // Максимальний кут нахилу (в градусах)

    private float randomOffset;

    void Start()
    {
        // Додаємо випадковий зсув, щоб об'єкти гойдалися не абсолютно синхронно
        randomOffset = Random.Range(0f, 100f);
    }

    void Update()
    {
        // Рахуємо плавний кут повороту навколо осі Z за допомогою Mathf.Sin
        float zRotation = Mathf.Sin(Time.time * speed + randomOffset) * angle;

        // Застосовуємо цей поворот до об'єкта
        transform.localRotation = Quaternion.Euler(0, 0, zRotation);
    }
}