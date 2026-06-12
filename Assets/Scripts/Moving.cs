using UnityEngine;

public class Moving: MonoBehaviour
{
    public GameObject player;
    public GameObject playerModel;

    public float speed = 5f;

    private float startPosition;

    Vector2 Move()
    {
        float moveX = 0f;
        float moveY = 0f;

        if (Input.GetKey(KeyCode.W)) moveY = 1f;
        if (Input.GetKey(KeyCode.S)) moveY = -1f;
        if (Input.GetKey(KeyCode.A))
        {
            moveX = -1f;
            playerModel.transform.localScale = new Vector3(
                startPosition * (-1),
                playerModel.transform.localScale.y,
                playerModel.transform.localScale.z
            );
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveX = 1f;
            playerModel.transform.localScale = new Vector3(
                startPosition,
                playerModel.transform.localScale.y,
                playerModel.transform.localScale.z
            );
        }

        return new Vector2(moveX, moveY).normalized;
    }

    void Start()
    {
        startPosition = playerModel.transform.localScale.x;
    }

    void Update()
    {
        player.transform.Translate(Move() * speed * Time.deltaTime);
    }
}
