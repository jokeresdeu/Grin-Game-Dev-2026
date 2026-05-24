using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    private float offsetX;

    void Start()
    {
        offsetX = transform.position.x - player.position.x;
    }

    void Update()
    {
        transform.position = new Vector3(player.position.x + offsetX, transform.position.y, transform.position.z);
    }
}