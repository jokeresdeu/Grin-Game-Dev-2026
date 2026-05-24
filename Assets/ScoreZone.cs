using UnityEngine;

public class ScoreZone : MonoBehaviour
{
    private bool _scored = false;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (_scored) return;

        if (collision.CompareTag("Player"))
        {
            _scored = true;
            ScoreManager.Instance.AddScore();
        }
    }
}