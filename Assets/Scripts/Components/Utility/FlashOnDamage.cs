using System.Collections;
using UnityEngine;

/// <summary>
/// Flashes sprite when object takes damage.
/// Requires Health component on the same object.
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class FlashOnDamage : MonoBehaviour
{
    [Header("Flash Settings")]
    [SerializeField] private Color flashColor = Color.red;
    [SerializeField] private float flashDuration = 0.1f;
    [SerializeField] private int flashCount = 3;
    
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private Health health;
    private Coroutine flashCoroutine;
    
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        health = GetComponent<Health>();
    }
    
    private void OnEnable()
    {
        if (health != null)
        {
            health.GetComponent<Health>();
        }
    }
    
    public void Flash()
    {
        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
        }
        flashCoroutine = StartCoroutine(FlashRoutine());
    }
    
    private IEnumerator FlashRoutine()
    {
        for (int i = 0; i < flashCount; i++)
        {
            spriteRenderer.color = flashColor;
            yield return new WaitForSeconds(flashDuration);
            spriteRenderer.color = originalColor;
            yield return new WaitForSeconds(flashDuration);
        }
        
        spriteRenderer.color = originalColor;
        flashCoroutine = null;
    }
    
    public void SetOriginalColor(Color color)
    {
        originalColor = color;
    }
}
