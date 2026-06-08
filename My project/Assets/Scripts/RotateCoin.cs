using UnityEngine;

public class RotateCoin : MonoBehaviour
{
    public float rotationSpeed = 120f; // Ўвидк≥сть обертанн€

    void Update()
    {
        // ќбертаЇмо навколо ос≥ Y
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }
}