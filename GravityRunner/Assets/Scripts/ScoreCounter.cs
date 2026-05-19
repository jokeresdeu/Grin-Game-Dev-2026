using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
    private bool scored = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !scored)
        {
            scored = true;
            GameManager.instance.AddScore();
        }
    }
}