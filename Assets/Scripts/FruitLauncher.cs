using UnityEngine;

public class FruitLauncher : MonoBehaviour
{
    public float minUpForce = 12f;
    public float maxUpForce = 16f;
    public float sideForce = 2f;
    public float spinForce = 5f; 

    private Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        LaunchFruit();
    }

    void LaunchFruit()
    {
        float upForce = Random.Range(minUpForce, maxUpForce);
        float xForce = Random.Range(-sideForce, sideForce);

        Vector2 force = new Vector2(xForce, upForce);

        rb.AddForce(force, ForceMode2D.Impulse);

        float rotation = Random.Range(-spinForce, spinForce);
        rb.AddTorque(rotation, ForceMode2D.Impulse);
    }
}
