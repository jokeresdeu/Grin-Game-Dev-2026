using UnityEngine;

public class SineMovement : MonoBehaviour
{
    public float frequency = 3f;
    public float amplitude = 2f;

    private float startY;
    private float randomOffset;

    void Start()
    {
        startY = transform.position.y;

        randomOffset = Random.Range(0f, 10f);
    }

    void Update()
    {
        float newY = startY + Mathf.Sin((Time.time + randomOffset) * frequency) * amplitude;

        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}