using UnityEngine;

public class BirdMover : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _speed = 2f;
    [SerializeField] private bool _moveRight = true;

    [Header("Boundaries")]
    [SerializeField] private float _destroyBoundaryX = 18f;

    private BirdAnimationController _animController;
    private bool _isDead;

    private void Awake()
    {
        _animController = GetComponent<BirdAnimationController>();
    }

    private void Start()
    {
        if (_moveRight)
        {
            Vector3 scale = transform.localScale;
            scale.x = -Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
        else
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x);
            transform.localScale = scale;
        }

        if (GameAnimationManager.Instance != null)
        {
            GameAnimationManager.Instance.RegisterBirdAnimation();
        }
    }

    private void Update()
    {
        if (_isDead) return;

        float direction = _moveRight ? 1f : -1f;
        transform.position += Vector3.right * (direction * _speed * Time.deltaTime);

        if (_animController != null)
        {
            _animController.SetFlySpeed(_speed);
        }

        if (Mathf.Abs(transform.position.x) > _destroyBoundaryX)
        {
            CleanupAndDestroy();
        }
    }

    public void SetDirection(bool moveRight)
    {
        _moveRight = moveRight;

        if (_moveRight)
        {
            Vector3 scale = transform.localScale;
            scale.x = -Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
        else
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
    }

    public void OnShot()
    {
        if (_isDead) return;

        _isDead = true;

        if (_animController != null)
        {
            _animController.OnDeathAnimationComplete += OnDeathComplete;
            _animController.PlayDeathAnimation();

            if (GameAnimationManager.Instance != null)
            {
                GameAnimationManager.Instance.RecordDeathAnimation();
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDeathComplete()
    {
    }

    private void CleanupAndDestroy()
    {
        if (GameAnimationManager.Instance != null)
        {
            GameAnimationManager.Instance.UnregisterBirdAnimation();
        }
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (_animController != null)
        {
            _animController.OnDeathAnimationComplete -= OnDeathComplete;
        }
    }
}
