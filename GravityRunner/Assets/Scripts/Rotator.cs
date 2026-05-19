using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float rotationSpeed = 100f;

    void Update()
    {
        float currentSpeed = rotationSpeed * GameManager.instance.globalSpeedMultiplier;

        transform.Rotate(0, 0, currentSpeed * Time.deltaTime);
    }
}