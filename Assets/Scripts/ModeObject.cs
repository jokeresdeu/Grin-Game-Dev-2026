using UnityEngine;

public class ModeObject : MonoBehaviour
{
    [Header("Mode")]
    [SerializeField] private WorldMode activeMode = WorldMode.Light;

    [Header("Visual")]
    [SerializeField] private bool hideWhenInactive = false;
    [SerializeField, Range(0f, 1f)] private float inactiveAlpha = 0.2f;

    private SpriteRenderer[] spriteRenderers;
    private Collider2D[] colliders;

    private void Awake()
    {
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        colliders = GetComponentsInChildren<Collider2D>();
    }

    public void SetActiveMode(WorldMode mode)
    {
        activeMode = mode;
    }

    public void ApplyMode(WorldMode currentMode)
    {
        if (spriteRenderers == null || colliders == null)
        {
            spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
            colliders = GetComponentsInChildren<Collider2D>();
        }

        bool isActive = currentMode == activeMode;

        foreach (Collider2D col in colliders)
        {
            col.enabled = isActive;
        }

        foreach (SpriteRenderer sr in spriteRenderers)
        {
            if (hideWhenInactive)
            {
                sr.enabled = isActive;
            }
            else
            {
                Color color = sr.color;
                color.a = isActive ? 1f : inactiveAlpha;
                sr.color = color;
            }
        }
    }
}