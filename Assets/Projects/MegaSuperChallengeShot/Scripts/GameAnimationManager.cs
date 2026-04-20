using UnityEngine;

public class GameAnimationManager : MonoBehaviour
{
    public static GameAnimationManager Instance { get; private set; }

    [Header("Global Animation Settings")]
    [SerializeField] private float _globalAnimationSpeed = 1f;
    [SerializeField] private bool _animationsPaused = false;

    private int _activeBirdAnimations;
    private int _totalDeathAnimationsPlayed;

    public float GlobalAnimationSpeed => _globalAnimationSpeed;
    public bool AnimationsPaused => _animationsPaused;
    public int TotalDeathAnimationsPlayed => _totalDeathAnimationsPlayed;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    public void SetGlobalSpeed(float speed)
    {
        _globalAnimationSpeed = Mathf.Clamp(speed, 0.1f, 3f);
        Time.timeScale = _globalAnimationSpeed;
    }

    public void SetAnimationsPaused(bool paused)
    {
        _animationsPaused = paused;
        Time.timeScale = paused ? 0f : _globalAnimationSpeed;
    }

    public void RegisterBirdAnimation()
    {
        _activeBirdAnimations++;
    }

    public void UnregisterBirdAnimation()
    {
        _activeBirdAnimations--;
    }

    public void RecordDeathAnimation()
    {
        _totalDeathAnimationsPlayed++;
    }
}
