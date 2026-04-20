using UnityEngine;

public class PropFloatAnimator : MonoBehaviour
{
    [Header("Vertical Bob")]
    [SerializeField] private float _bobAmplitude = 0.1f;
    [SerializeField] private float _bobSpeed = 1.5f;

    [Header("Rotation Sway")]
    [SerializeField] private bool _enableSway = true;
    [SerializeField] private float _swayAngle = 3f;
    [SerializeField] private float _swaySpeed = 1f;

    [Header("Scale Breathe")]
    [SerializeField] private bool _enableBreathe = false;
    [SerializeField] private float _breatheAmount = 0.02f;
    [SerializeField] private float _breatheSpeed = 1f;

    [Header("Randomize")]
    [SerializeField] private bool _randomizePhase = true;

    private Vector3 _startPosition;
    private Quaternion _startRotation;
    private Vector3 _startScale;
    private float _phaseOffset;

    private void Awake()
    {
        _startPosition = transform.localPosition;
        _startRotation = transform.localRotation;
        _startScale = transform.localScale;

        if (_randomizePhase)
        {
            _phaseOffset = Random.Range(0f, Mathf.PI * 2f);
        }
    }

    private void Update()
    {
        float time = Time.time + _phaseOffset;

        float bobOffset = Mathf.Sin(time * _bobSpeed) * _bobAmplitude;
        transform.localPosition = _startPosition + new Vector3(0f, bobOffset, 0f);

        if (_enableSway)
        {
            float swayOffset = Mathf.Sin(time * _swaySpeed) * _swayAngle;
            transform.localRotation = _startRotation * Quaternion.Euler(0f, 0f, swayOffset);
        }

        if (_enableBreathe)
        {
            float breathe = 1f + Mathf.Sin(time * _breatheSpeed) * _breatheAmount;
            transform.localScale = _startScale * breathe;
        }
    }
}
