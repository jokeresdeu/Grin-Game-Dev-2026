using UnityEngine;

/// <summary>
/// Simple sound effect player.
/// Call PlaySound() from UnityEvents or other scripts.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class SoundPlayer : MonoBehaviour
{
    [Header("Sounds")]
    [SerializeField] private AudioClip[] sounds;
    
    [Header("Settings")]
    [SerializeField] private bool randomizePitch = false;
    [SerializeField] private float minPitch = 0.9f;
    [SerializeField] private float maxPitch = 1.1f;
    
    private AudioSource audioSource;
    
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    
    public void PlaySound()
    {
        if (sounds == null || sounds.Length == 0) return;
        
        AudioClip clip = sounds[Random.Range(0, sounds.Length)];
        PlayClip(clip);
    }
    
    public void PlaySound(int index)
    {
        if (sounds == null || index < 0 || index >= sounds.Length) return;
        PlayClip(sounds[index]);
    }
    
    public void PlaySound(AudioClip clip)
    {
        PlayClip(clip);
    }
    
    private void PlayClip(AudioClip clip)
    {
        if (clip == null) return;
        
        if (randomizePitch)
        {
            audioSource.pitch = Random.Range(minPitch, maxPitch);
        }
        
        audioSource.PlayOneShot(clip);
    }
    
    public void StopSound()
    {
        audioSource.Stop();
    }
}
