using UnityEngine;

/// <summary>
/// Moves transform vertically based on keyboard input (W/S or Arrow keys).
/// </summary>
public class VerticalMover : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 5f;
    
    private void Update()
    {
        float vertical = Input.GetAxis("Vertical");
        transform.Translate(Vector3.up * vertical * speed * Time.deltaTime);
    }
}
