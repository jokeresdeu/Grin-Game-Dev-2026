using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private int scoreValue = 50;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.Instance.AddScore(scoreValue);
            Destroy(gameObject);
        }
    }
}