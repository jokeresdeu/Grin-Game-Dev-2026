using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteAnimator : MonoBehaviour
{
    public enum PlayMode
    {
        Loop,
        PingPong,
        PlayOnce
    }

    [Header("Frames")]
    [SerializeField] private Sprite[] _frames;
    [SerializeField] private float _frameRate = 8f;

    [Header("Playback")]
    [SerializeField] private PlayMode _playMode = PlayMode.Loop;
    [SerializeField] private bool _playOnAwake = true;

    private SpriteRenderer _spriteRenderer;
    private float _frameTimer;
    private int _currentFrame;
    private int _direction = 1;
    private bool _isPlaying;

    public bool IsPlaying => _isPlaying;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        if (_playOnAwake && _frames != null && _frames.Length > 0)
        {
            Play();
        }
    }

    private void Update()
    {
        if (!_isPlaying || _frames == null || _frames.Length == 0) return;

        _frameTimer += Time.deltaTime;
        float interval = 1f / _frameRate;

        if (_frameTimer >= interval)
        {
            _frameTimer -= interval;
            AdvanceFrame();
        }
    }

    private void AdvanceFrame()
    {
        switch (_playMode)
        {
            case PlayMode.Loop:
                _currentFrame = (_currentFrame + 1) % _frames.Length;
                break;

            case PlayMode.PingPong:
                _currentFrame += _direction;
                if (_currentFrame >= _frames.Length - 1)
                {
                    _currentFrame = _frames.Length - 1;
                    _direction = -1;
                }
                else if (_currentFrame <= 0)
                {
                    _currentFrame = 0;
                    _direction = 1;
                }
                break;

            case PlayMode.PlayOnce:
                _currentFrame++;
                if (_currentFrame >= _frames.Length)
                {
                    _currentFrame = _frames.Length - 1;
                    _isPlaying = false;
                }
                break;
        }

        _spriteRenderer.sprite = _frames[_currentFrame];
    }

    public void Play()
    {
        _isPlaying = true;
        _currentFrame = 0;
        _frameTimer = 0f;
        _direction = 1;

        if (_frames != null && _frames.Length > 0)
        {
            _spriteRenderer.sprite = _frames[0];
        }
    }

    public void Stop()
    {
        _isPlaying = false;
    }

    public void SetFrames(Sprite[] frames)
    {
        _frames = frames;
        _currentFrame = 0;
    }
}
