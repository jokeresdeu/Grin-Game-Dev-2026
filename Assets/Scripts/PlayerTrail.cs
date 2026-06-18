using UnityEngine;

public class PlayerTrail : MonoBehaviour
{
    private TrailRenderer trailRenderer;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        trailRenderer = GetComponent<TrailRenderer>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    void Start()
    {
        if (spriteRenderer != null)
        {
            Color playerColor = spriteRenderer.color;
            Gradient gradient = new Gradient();

            gradient.SetKeys(
                new GradientColorKey[] {
                    new GradientColorKey(playerColor, 0.0f),
                    new GradientColorKey(playerColor, 1.0f)
                },
                new GradientAlphaKey[] {
                    new GradientAlphaKey(1.0f, 0.0f),
                    new GradientAlphaKey(0.0f, 1.0f)
                }
            );

            trailRenderer.colorGradient = gradient;
        }
    }
}