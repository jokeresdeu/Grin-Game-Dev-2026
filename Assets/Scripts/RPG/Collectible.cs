using UnityEngine;

namespace RPG
{
    [RequireComponent(typeof(Collider2D))]
    public class Collectible : MonoBehaviour
    {
        [SerializeField] private int _scoreValue = 10;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                if (RPGGameManager.Instance != null)
                {
                    RPGGameManager.Instance.AddScore(_scoreValue);
                }
                Destroy(gameObject);
            }
        }
    }
}

