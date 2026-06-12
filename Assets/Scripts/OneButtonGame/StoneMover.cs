using UnityEngine;

namespace OneButtonGame
{
    public class StoneMover : MonoBehaviour
    {
        [SerializeField] private float _speed = 5f; // Швидкість польоту
        [SerializeField] private float _deadZone = -12f; // Де камінь зникає
        private float _rotationSpeed;

        private void Start()
        {
            // Випадкова швидкість обертання для краси
            _rotationSpeed = Random.Range(-120f, 120f);
        }

        private void Update()
        {
            if (Time.timeScale == 0f) return;

            // Рух вліво
            transform.Translate(Vector3.left * (_speed * Time.deltaTime), Space.World);
            // Обертання
            transform.Rotate(Vector3.forward * (_rotationSpeed * Time.deltaTime));

            // Якщо камінь вилетів за ліву межу екрана
            if (transform.position.x < _deadZone)
            {
                // Гравець ухилився — отримує +1 бал
                if (FlappyManager.Instance != null)
                {
                    FlappyManager.Instance.AddScore(1);
                }
                Destroy(gameObject);
            }
        }
    }
}