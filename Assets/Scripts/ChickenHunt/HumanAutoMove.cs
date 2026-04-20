using UnityEngine;

public class HumanAutoMove : MonoBehaviour
{
    public float speed = 3f;
    public float leftBoundary = -10f; // Ліва межа сцени
    public float rightBoundary = 10f; // Права межа сцени
    private bool movingRight = true;

    void Update()
    {
        // 1. Рухаємо об'єкт
        if (movingRight)
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }

        // 2. Перевіряємо межі та розвертаємося
        if (transform.position.x >= rightBoundary)
        {
            movingRight = false;
            Flip();
        }
        else if (transform.position.x <= leftBoundary)
        {
            movingRight = true;
            Flip();
        }
    }

    void Flip()
    {
        // Розвертаємо модельку персонажа (дзеркально)
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }
}