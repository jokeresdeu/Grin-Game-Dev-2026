using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class CloudMover : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _driftSpeed = 0.5f;
    [SerializeField] private float _resetXLeft = -18f;
    [SerializeField] private float _resetXRight = 18f;
    [SerializeField] private bool _moveRight = true;

    [Header("Appearance")]
    [SerializeField] [Range(0.1f, 1f)] private float _minAlpha = 0.3f;
    [SerializeField] [Range(0.1f, 1f)] private float _maxAlpha = 0.7f;
    [SerializeField] [Range(0.5f, 2f)] private float _minScale = 0.7f;
    [SerializeField] [Range(0.5f, 2f)] private float _maxScale = 1.3f;

    [Header("Randomize on Start")]
    [SerializeField] private bool _randomizeOnStart = true;

    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        if (_randomizeOnStart)
        {
            RandomizeAppearance();
        }
    }

    private void Update()
    {
        float direction = _moveRight ? 1f : -1f;
        transform.position += Vector3.right * (direction * _driftSpeed * Time.deltaTime);

        if (_moveRight && transform.position.x > _resetXRight)
        {
            transform.position = new Vector3(_resetXLeft, transform.position.y, transform.position.z);
            if (_randomizeOnStart) RandomizeAppearance();
        }
        else if (!_moveRight && transform.position.x < _resetXLeft)
        {
            transform.position = new Vector3(_resetXRight, transform.position.y, transform.position.z);
            if (_randomizeOnStart) RandomizeAppearance();
        }
    }

    private void RandomizeAppearance()
    {
        float alpha = Random.Range(_minAlpha, _maxAlpha);
        Color c = _spriteRenderer.color;
        c.a = alpha;
        _spriteRenderer.color = c;

        float scale = Random.Range(_minScale, _maxScale);
        transform.localScale = new Vector3(scale, scale, 1f);
    }
}
