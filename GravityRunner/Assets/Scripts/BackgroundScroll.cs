using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    public float speed = 2f;
    private float backgroundWidth;

    void Start()
    {
        backgroundWidth = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        transform.Translate(Vector2.left * speed * GameManager.instance.globalSpeedMultiplier * Time.deltaTime);
        if (transform.position.x <= -backgroundWidth)
        {
            Vector2 resetPosition = new Vector2(backgroundWidth * 2f, 0);
            transform.position = (Vector2)transform.position + resetPosition;
        }
    }
}