using UnityEngine;

public class HomingMove : MonoBehaviour
{
    public float speed = 6f;
    public float trackingSpeed = 2f;

    private Transform player;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void Update()
    {
        transform.Translate(Vector2.left * speed * GameManager.instance.globalSpeedMultiplier * Time.deltaTime, Space.World);

        if (player != null && player.gameObject.activeSelf)
        {
            float newY = Mathf.Lerp(transform.position.y, player.position.y, trackingSpeed * Time.deltaTime);
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        }

        if (transform.position.x < -15f)
        {
            Destroy(gameObject);
        }
    }
}