using UnityEngine;

/// <summary>
/// Moves transform in all directions using WASD or Arrow keys.
/// Perfect for top-down games.
/// </summary>
public class TopDownMover : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private bool normalizeMovement = true;
    
    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        
        Vector3 movement = new Vector3(horizontal, vertical, 0f);
        
        if (normalizeMovement && movement.magnitude > 1f)
        {
            movement.Normalize();
        }
        
        transform.Translate(movement * speed * Time.deltaTime);
    }
}
