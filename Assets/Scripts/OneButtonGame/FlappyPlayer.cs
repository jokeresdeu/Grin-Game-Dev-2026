using UnityEngine;

namespace OneButtonGame
{
    public class FlappyPlayer : MonoBehaviour
    {
        [Header("Налаштування польоту")]
        [SerializeField] private float _bounceForce = 6f; // Сила стрибка
        
        private Rigidbody2D _rb;

        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            if (_rb != null)
            {
                _rb.gravityScale = 1.8f; // Вага пташки
                _rb.linearVelocity = Vector2.zero;
            }
        }

        private void Update()
        {
            // Головна умова лаби: одна кнопка (Пробіл або Клік)
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                if (Time.timeScale > 0f) // Стрибаємо тільки якщо гра не на паузі
                {
                    _rb.linearVelocity = new Vector2(0f, _bounceForce);
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            // Якщо врізалися в камінь із тегом Obstacle
            if (other.CompareTag("Obstacle"))
            {
                if (FlappyManager.Instance != null)
                {
                    FlappyManager.Instance.GameOver();
                }
            }
        }
    }
}