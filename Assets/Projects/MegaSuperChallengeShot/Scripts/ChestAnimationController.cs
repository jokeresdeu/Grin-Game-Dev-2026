using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class ChestAnimationController : MonoBehaviour
{
    [Header("Sprite Frames (assign sliced sprites from TX Chest Animation)")]
    [SerializeField] private Sprite[] _openFrames;
    [SerializeField] private Sprite _closedSprite;

    [Header("Animation Settings")]
    [SerializeField] private float _frameRate = 8f;
    [SerializeField] private float _autoCloseDelay = 3f;
    [SerializeField] private bool _autoCloseEnabled = true;

    [Header("Idle Bounce")]
    [SerializeField] private float _idleBounceAmount = 0.05f;
    [SerializeField] private float _idleBounceSpeed = 2f;

    [Header("Score")]
    [SerializeField] private int _scoreValue = 5;

    [Header("Events")]
    [SerializeField] private UnityEvent _onChestOpened;
    [SerializeField] private UnityEvent _onChestClosed;

    private SpriteRenderer _spriteRenderer;
    private bool _isOpen;
    private bool _isAnimating;
    private Vector3 _startPosition;
    private Coroutine _idleCoroutine;

    public bool IsOpen => _isOpen;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _startPosition = transform.localPosition;
    }

    private void Start()
    {
        if (_closedSprite != null)
        {
            _spriteRenderer.sprite = _closedSprite;
        }

        _idleCoroutine = StartCoroutine(IdleBounceCoroutine());
    }

    public void ToggleChest()
    {
        if (_isAnimating) return;

        if (_isOpen)
            CloseChest();
        else
            OpenChest();
    }

    public void OpenChest()
    {
        if (_isOpen || _isAnimating) return;
        StartCoroutine(OpenAnimationCoroutine());
    }

    public void CloseChest()
    {
        if (!_isOpen || _isAnimating) return;
        StartCoroutine(CloseAnimationCoroutine());
    }

    private IEnumerator OpenAnimationCoroutine()
    {
        _isAnimating = true;

        if (_idleCoroutine != null)
        {
            StopCoroutine(_idleCoroutine);
            transform.localPosition = _startPosition;
        }

        float frameInterval = 1f / _frameRate;
        for (int i = 0; i < _openFrames.Length; i++)
        {
            _spriteRenderer.sprite = _openFrames[i];
            yield return new WaitForSeconds(frameInterval);
        }

        _isOpen = true;
        _isAnimating = false;
        _onChestOpened?.Invoke();

        if (Projects.MegaSuperChallengeShot.Scripts.ScoreManager.Instance != null)
        {
            for (int i = 0; i < _scoreValue; i++)
            {
                Projects.MegaSuperChallengeShot.Scripts.ScoreManager.Instance.AddScore();
            }
        }

        _idleCoroutine = StartCoroutine(IdleBounceCoroutine());

        if (_autoCloseEnabled)
        {
            yield return new WaitForSeconds(_autoCloseDelay);
            CloseChest();
        }
    }

    private IEnumerator CloseAnimationCoroutine()
    {
        _isAnimating = true;

        if (_idleCoroutine != null)
        {
            StopCoroutine(_idleCoroutine);
            transform.localPosition = _startPosition;
        }

        float frameInterval = 1f / _frameRate;
        for (int i = _openFrames.Length - 1; i >= 0; i--)
        {
            _spriteRenderer.sprite = _openFrames[i];
            yield return new WaitForSeconds(frameInterval);
        }

        _spriteRenderer.sprite = _closedSprite;
        _isOpen = false;
        _isAnimating = false;
        _onChestClosed?.Invoke();

        _idleCoroutine = StartCoroutine(IdleBounceCoroutine());
    }

    private IEnumerator IdleBounceCoroutine()
    {
        while (true)
        {
            float offsetY = Mathf.Sin(Time.time * _idleBounceSpeed) * _idleBounceAmount;
            transform.localPosition = _startPosition + new Vector3(0f, offsetY, 0f);
            yield return null;
        }
    }

    private void OnMouseDown()
    {
        ToggleChest();
    }
}
