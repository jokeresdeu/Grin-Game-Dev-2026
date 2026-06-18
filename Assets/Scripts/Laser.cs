using UnityEngine;
using System.Collections;

public class Laser : MonoBehaviour
{
    [Header("Timings")]
    public float warningDuration = 1f;
    public float activeDuration = 0.5f;

    private SpriteRenderer spriteRenderer;
    private Collider2D laserCollider;

    void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        laserCollider = GetComponent<Collider2D>();
    }

    void Start()
    {
        StartCoroutine(LaserSequence());
    }

    private IEnumerator LaserSequence()
    {
        laserCollider.enabled = false;
        spriteRenderer.color = new Color(1f, 0f, 0f, 0.3f);

        yield return new WaitForSeconds(warningDuration);

        laserCollider.enabled = true;
        spriteRenderer.color = new Color(1f, 0f, 0f, 1f);

        yield return new WaitForSeconds(activeDuration);

        Destroy(gameObject);
    }
}