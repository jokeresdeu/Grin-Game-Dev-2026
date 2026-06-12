using Projects.MegaSuperChallengeShot.Scripts;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            playerMovement.ReverseDirectionAndAccelerate();

            if (ScoreManagerNew.Instance != null)
            {
                ScoreManagerNew.Instance.AddScore(1);
            }

            WallVisuals wallVisuals = collision.gameObject.GetComponent<WallVisuals>();
            if (wallVisuals != null && spriteRenderer != null)
            {
                wallVisuals.FlashColor(spriteRenderer.color);
            }
        }
    }
}