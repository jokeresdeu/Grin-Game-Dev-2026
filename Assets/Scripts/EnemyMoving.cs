using UnityEngine;

public class EnemyMoving : MonoBehaviour
{
    public GameObject player;
    public GameObject enemy;
    public GameObject enemyModel;

    public float speed = 3f;

    private float startPosition;

    Vector2 Move()
    {
        float moveX = 0f;
        float moveY = 0f;

        if (player.transform.position.y > enemy.transform.position.y) moveY = 1f;
        if (player.transform.position.y < enemy.transform.position.y) moveY = -1f;

        if (player.transform.position.x < enemy.transform.position.x)
        {
            moveX = -1f;
            enemyModel.transform.localScale = new Vector3(
                startPosition,
                enemyModel.transform.localScale.y,
                enemyModel.transform.localScale.z
            );
        }
        if (player.transform.position.x > enemy.transform.position.x)
        {
            moveX = 1f;
            enemyModel.transform.localScale = new Vector3(
                startPosition * (-1),
                enemyModel.transform.localScale.y,
                enemyModel.transform.localScale.z
            );
        }

        return new Vector2(moveX, moveY).normalized;
    }

    void Start()
    {
        startPosition = enemyModel.transform.localScale.x;
    }

    void Update()
    {
        enemy.transform.Translate(Move() * speed * Time.deltaTime);
    }
}
