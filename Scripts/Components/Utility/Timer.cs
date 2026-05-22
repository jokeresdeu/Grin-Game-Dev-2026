using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// General purpose timer with events.
/// Can count up or down, fires events on completion.
/// </summary>
public class Timer : MonoBehaviour
{
    [Header("Timer Settings")]
    [SerializeField] private float duration = 60f;
    [SerializeField] private bool countDown = true;
    [SerializeField] private bool startOnAwake = false;
    [SerializeField] private bool loop = false;
    
    [Header("Events")]
    [SerializeField] private UnityEvent onTimerStart;
    [SerializeField] private UnityEvent onTimerComplete;
    [SerializeField] private UnityEvent<float> onTimerUpdate;
    
    private float currentTime;
    private bool isRunning;
    
    public float CurrentTime => currentTime;
    public float Duration => duration;
    public float TimeRemaining => countDown ? currentTime : duration - currentTime;
    public float TimePercent => countDown ? currentTime / duration : 1f - (currentTime / duration);
    public bool IsRunning => isRunning;
    
    private void Start()
    {
        if (startOnAwake)
        {
            StartTimer();
        }
    }
    
    private void Update()
    {
        if (!isRunning) return;
        
        if (countDown)
        {
            currentTime -= Time.deltaTime;
            
            if (currentTime <= 0f)
            {
                currentTime = 0f;
                CompleteTimer();
            }
        }
        else
        {
            currentTime += Time.deltaTime;
            
            if (currentTime >= duration)
            {
                currentTime = duration;
                CompleteTimer();
            }
        }
        
        onTimerUpdate?.Invoke(currentTime);
    }
    
    public void StartTimer()
    {
        currentTime = countDown ? duration : 0f;
        isRunning = true;
        onTimerStart?.Invoke();
    }
    
    public void StartTimer(float newDuration)
    {
        duration = newDuration;
        StartTimer();
    }
    
    public void StopTimer()
    {
        isRunning = false;
    }
    
    public void PauseTimer()
    {
        isRunning = false;
    }
    
    public void ResumeTimer()
    {
        isRunning = true;
    }
    
    public void ResetTimer()
    {
        currentTime = countDown ? duration : 0f;
    }
    
    private void CompleteTimer()
    {
        onTimerComplete?.Invoke();
        
        if (loop)
        {
            ResetTimer();
        }
        else
        {
            isRunning = false;
        }
    }
    
    public void AddTime(float seconds)
    {
        if (countDown)
        {
            currentTime += seconds;
        }
        else
        {
            currentTime = Mathf.Max(0f, currentTime - seconds);
        }
    }
}
