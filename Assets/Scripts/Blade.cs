using UnityEngine;

public class Blade : MonoBehaviour
{
    private Vector2 previousMousePosition;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            previousMousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        }
        else if (Input.GetMouseButton(0))
        {
            Vector2 currentMousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Linecast(previousMousePosition, currentMousePosition);

            if (hit.collider != null)
            {
                Hitbox target = hit.collider.GetComponent<Hitbox>();

                if (target != null)
                {
                    target.Slice();
                }
            }

            previousMousePosition = currentMousePosition;
        }
    }
}
