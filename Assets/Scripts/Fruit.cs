using UnityEngine;

public class Fruit : MonoBehaviour
{
    [Header("Fruit")]
    [SerializeField] private int scoreValue = 1;

    [Header("Sliced Fruit")]
    [SerializeField] private GameObject slicedFruitPrefab;
    [SerializeField] private float halfForce = 4f;
    [SerializeField] private float upwardForce = 2f;
    [SerializeField] private float halfTorque = 300f;
    [SerializeField] private float destroySlicedTime = 3f;

    private bool isSliced;
    private bool wasVisible;

    private void OnBecameVisible()
    {
        wasVisible = true;
    }

    private void OnBecameInvisible()
    {
        if (!wasVisible) return;
        if (isSliced) return;
        if (GameManager.Instance == null) return;
        if (GameManager.Instance.IsGameOver) return;

        GameManager.Instance.LoseHealth();
        Destroy(gameObject);
    }

    public void Slice()
    {
        if (isSliced) return;

        isSliced = true;

        if (GameManager.Instance != null)
            GameManager.Instance.AddScore(scoreValue);

        SpawnSlicedFruit();

        Destroy(gameObject);
    }

    private void SpawnSlicedFruit()
    {
        if (slicedFruitPrefab == null)
        {
            Debug.LogWarning("Sliced Fruit Prefab is empty on " + gameObject.name);
            return;
        }

        GameObject slicedFruit = Instantiate(
            slicedFruitPrefab,
            transform.position,
            transform.rotation
        );

        Transform[] parts = slicedFruit.GetComponentsInChildren<Transform>();

        int partIndex = 0;

        foreach (Transform part in parts)
        {
            if (part == slicedFruit.transform)
                continue;

            SpriteRenderer spriteRenderer = part.GetComponent<SpriteRenderer>();

            if (spriteRenderer != null)
            {
                spriteRenderer.sortingOrder = 20;
            }

            Rigidbody2D rb = part.GetComponent<Rigidbody2D>();

            if (rb == null)
                rb = part.gameObject.AddComponent<Rigidbody2D>();

            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.gravityScale = 2f;
            rb.linearDamping = 0f;
            rb.angularDamping = 0.05f;
            rb.constraints = RigidbodyConstraints2D.None;

            Collider2D col = part.GetComponent<Collider2D>();

            if (col == null)
                part.gameObject.AddComponent<BoxCollider2D>();

            float side = partIndex == 0 ? -1f : 1f;

            part.position = transform.position + new Vector3(side * 0.2f, 0f, 0f);

            Vector2 forceDirection = new Vector2(side * halfForce, upwardForce);
            rb.AddForce(forceDirection, ForceMode2D.Impulse);
            rb.AddTorque(side * halfTorque);

            partIndex++;
        }

        Destroy(slicedFruit, destroySlicedTime);
    }
}