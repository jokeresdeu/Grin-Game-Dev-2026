using UnityEngine;

/// <summary>
/// Continuously rotates the object around specified axis.
/// Great for spinning coins, obstacles, or decorative elements.
/// </summary>
public class Rotator : MonoBehaviour
{
    [Header("Rotation Settings")]
    [SerializeField] private float rotationSpeed = 180f;
    [SerializeField] private bool clockwise = true;
    
    private void Update()
    {
        float direction = clockwise ? -1f : 1f;
        transform.Rotate(0f, 0f, rotationSpeed * direction * Time.deltaTime);
    }
}
