using UnityEngine;

public class VerticalMove : MonoBehaviour
{
    public float verticalSpeed = 3f;

    public float topLimit = 3.5f;
    public float bottomLimit = -3.5f;

    private bool movingUp;

    void Start()
    {
        movingUp = Random.Range(0, 2) == 0;
    }

    void Update()
    {
        if (movingUp)
        {
            transform.Translate(Vector3.up * verticalSpeed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.down * verticalSpeed * Time.deltaTime);
        }

        if (transform.position.y >= topLimit)
        {
            movingUp = false;
        }
        if (transform.position.y <= bottomLimit)
        {
            movingUp = true;
        }
    }
}