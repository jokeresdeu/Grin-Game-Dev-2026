using UnityEngine;

public class BirdMovement : MonoBehaviour
{
    [Header("Налаштування руху")]
    public float speed = 3f;             
    public Vector2 moveRange = new Vector2(7f, 4f); 
    public float waitTime = 1f;         

    private Vector2 targetPosition;
    private bool isWaiting = false;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetRandomTarget();
    }

    void Update()
    {
        if (isWaiting) return;

        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
        {
            Invoke("SetRandomTarget", waitTime);
            isWaiting = true;
        }
    }

    void SetRandomTarget()
    {
        isWaiting = false;

        float randomX = Random.Range(-moveRange.x, moveRange.x);
        float randomY = Random.Range(-moveRange.y, moveRange.y);
        targetPosition = new Vector2(randomX, randomY);

        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = targetPosition.x < transform.position.x;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(moveRange.x * 2, moveRange.y * 2, 0));
    }
}