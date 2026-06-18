using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [HideInInspector]
    public Vector2 moveDirection;
    public float speed = 3f;
    public float lifeTime = 7f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.Translate(moveDirection * speed * Time.deltaTime);
    }
}