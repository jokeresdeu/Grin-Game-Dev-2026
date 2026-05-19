using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;
    private Vector3 initialPosition;

    private float shakeDuration = 0f;
    private float shakeMagnitude = 0.3f;

    void Awake()
    {
        if (instance == null) instance = this;
    }

    void Start()
    {
        initialPosition = transform.localPosition;
    }

    void Update()
    {
        if (shakeDuration > 0)
        {
            transform.localPosition = initialPosition + Random.insideUnitSphere * shakeMagnitude;

            shakeDuration -= Time.unscaledDeltaTime;
        }
        else
        {
            shakeDuration = 0f;
            transform.localPosition = initialPosition;
        }
    }

    public void TriggerShake()
    {
        shakeDuration = 0.3f;
    }
}