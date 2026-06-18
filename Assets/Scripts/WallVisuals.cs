using UnityEngine;
using System.Collections;

public class WallVisuals : MonoBehaviour
{
    [Header("Animation Settings")]
    public float colorTransitionDuration = 0.5f;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private Coroutine colorCoroutine;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    public void FlashColor(Color targetColor)
    {
        if (colorCoroutine != null)
        {
            StopCoroutine(colorCoroutine);
        }
        colorCoroutine = StartCoroutine(SmoothColorChange(targetColor));
    }

    private IEnumerator SmoothColorChange(Color targetColor)
    {
        spriteRenderer.color = targetColor;
        float elapsedTime = 0f;

        while (elapsedTime < colorTransitionDuration)
        {
            spriteRenderer.color = Color.Lerp(targetColor, originalColor, elapsedTime / colorTransitionDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        spriteRenderer.color = originalColor;
    }
}