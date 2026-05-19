using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Джерела звуку")]
    public AudioSource sfxSource;
    public AudioSource bgmSource;

    [Header("SFX")]
    public AudioClip coinSound;
    public AudioClip clickSound;
    public AudioClip deathSound;
    public AudioClip laserShootSound;

    [Header("BGM")]
    public AudioClip menuMusic;
    public AudioClip gameMusic;

    [HideInInspector] public float maxMusicVolume = 0.4f;
    [HideInInspector] public float sfxVolume = 0.7f;

    private Coroutine musicFadeCoroutine;
    private Coroutine volumeModifierCoroutine;
    private bool isMuffled = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            maxMusicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.4f);
            sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 0.7f);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        isMuffled = false;
        if (scene.name == "MainMenu")
        {
            PlayMusic(menuMusic, false);
        }
        else if (scene.name == "GameScene")
        {
            PlayMusic(gameMusic, true);
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null && sfxSource != null)
        {
            sfxSource.volume = sfxVolume;
            sfxSource.PlayOneShot(clip);
        }
    }

    public void PlayClickSound()
    {
        PlaySFX(clickSound);
    }

    public void UpdateMusicVolume(float value)
    {
        maxMusicVolume = value;
        PlayerPrefs.SetFloat("MusicVolume", maxMusicVolume);

        if (!isMuffled && bgmSource != null)
        {
            bgmSource.volume = maxMusicVolume;
        }
        else if (isMuffled && bgmSource != null)
        {
            bgmSource.volume = maxMusicVolume * 0.25f;
        }
    }

    public void UpdateSFXVolume(float value)
    {
        sfxVolume = value;
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        if (sfxSource != null) sfxSource.volume = sfxVolume;
    }


    public void PlayMusic(AudioClip newClip, bool forceRestart = false)
    {
        if (newClip == null || bgmSource == null) return;

        if (bgmSource.clip == newClip && bgmSource.isPlaying)
        {
            if (forceRestart)
            {
                bgmSource.Stop();
                bgmSource.Play();
                bgmSource.volume = maxMusicVolume;
            }
            return;
        }

        if (musicFadeCoroutine != null) StopCoroutine(musicFadeCoroutine);
        musicFadeCoroutine = StartCoroutine(FadeMusic(newClip));
    }

    IEnumerator FadeMusic(AudioClip newClip)
    {
        float fadeTime = 1.5f;

        if (bgmSource.isPlaying)
        {
            float startVolume = bgmSource.volume;
            while (bgmSource.volume > 0)
            {
                bgmSource.volume -= startVolume * (Time.unscaledDeltaTime / fadeTime);
                yield return null;
            }
            bgmSource.Stop();
        }

        bgmSource.clip = newClip;
        bgmSource.volume = 0f;
        bgmSource.Play();

        while (bgmSource.volume < maxMusicVolume)
        {
            bgmSource.volume += (maxMusicVolume * Time.unscaledDeltaTime) / fadeTime;
            yield return null;
        }

        bgmSource.volume = maxMusicVolume;
    }

    public void SetMusicMuffled(bool muffle)
    {
        isMuffled = muffle;
        if (bgmSource == null || !bgmSource.isPlaying) return;

        if (volumeModifierCoroutine != null) StopCoroutine(volumeModifierCoroutine);

        float targetVolume = muffle ? maxMusicVolume * 0.25f : maxMusicVolume;
        volumeModifierCoroutine = StartCoroutine(FadeVolumeOnly(targetVolume, 0.4f));
    }

    IEnumerator FadeVolumeOnly(float targetVolume, float duration)
    {
        float startVolume = bgmSource.volume;
        float time = 0;

        while (time < duration)
        {
            time += Time.unscaledDeltaTime;
            bgmSource.volume = Mathf.Lerp(startVolume, targetVolume, time / duration);
            yield return null;
        }

        bgmSource.volume = targetVolume;
    }
}