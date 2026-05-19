using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransition : MonoBehaviour
{
    public static SceneTransition instance;

    [Header("Налаштування")]
    public Image fadeImage;
    public float fadeSpeed = 3f;

    void Awake()
    {
        if (instance == null) instance = this;
    }

    void Start()
    {
        this.gameObject.SetActive(true);

        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true);
            StartCoroutine(FadeIn());
        }
    }

    public void LoadScene(string sceneName)
    {
        this.gameObject.SetActive(true);
        StartCoroutine(FadeOutAndLoad(sceneName));
    }

    IEnumerator FadeIn()
    {
        if (fadeImage == null) yield break;

        fadeImage.raycastTarget = true;
        fadeImage.color = new Color(0, 0, 0, 1);

        while (fadeImage.color.a > 0.01f)
        {
            float alpha = Mathf.Lerp(fadeImage.color.a, 0f, Time.unscaledDeltaTime * fadeSpeed);
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        fadeImage.color = new Color(0, 0, 0, 0);
        fadeImage.raycastTarget = false;
    }

    IEnumerator FadeOutAndLoad(string sceneName)
    {
        if (fadeImage == null)
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(sceneName);
            yield break;
        }

        fadeImage.gameObject.SetActive(true);
        fadeImage.raycastTarget = true;

        while (fadeImage.color.a < 0.99f)
        {
            float alpha = Mathf.Lerp(fadeImage.color.a, 1f, Time.unscaledDeltaTime * fadeSpeed);
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        fadeImage.color = new Color(0, 0, 0, 1);

        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }
}