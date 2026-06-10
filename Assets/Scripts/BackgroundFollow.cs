using UnityEngine;

public class BackgroundFollow : MonoBehaviour
{
    [SerializeField] private Transform targetCamera;
    [SerializeField] private bool followX = true;
    [SerializeField] private bool followY = false;

    private float startX;
    private float startY;

    private void Start()
    {
        if (targetCamera == null)
        {
            targetCamera = Camera.main.transform;
        }

        startX = transform.position.x;
        startY = transform.position.y;
    }

    private void LateUpdate()
    {
        if (targetCamera == null)
        {
            return;
        }

        float x = followX ? targetCamera.position.x : startX;
        float y = followY ? targetCamera.position.y : startY;

        transform.position = new Vector3(x, y, transform.position.z);
    }
}
