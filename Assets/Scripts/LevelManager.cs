using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    public Image progressBarFill;
    public RectTransform flagMarker;
    public GameObject levelCompletePanel;

    public GameObject[] stars;
    public float starPopupDelay = 0.4f;
    public float fadeDuration = 0.6f;

    public float levelDuration = 30f;
    public float startPointX = -150f;
    public float endPointX = 150f;

    private float currentFlightTime = 0f;
    private bool isLevelFinished = false;
    private PlaneController plane;

    void Start()
    {
        levelCompletePanel.SetActive(false);

        foreach (GameObject star in stars)
        {
            star.SetActive(false);
        }

        if (progressBarFill != null)
        {
            progressBarFill.fillAmount = 0f;
        }

        plane = FindFirstObjectByType<PlaneController>();
    }

    void Update()
    {
        if (isLevelFinished) return;
        if (plane == null || !plane.gameStarted) return;
        if (plane.isDead) return;

        currentFlightTime += Time.deltaTime;
        float progress = currentFlightTime / levelDuration;

        if (progressBarFill != null)
        {
            progressBarFill.fillAmount = progress;
        }

        if (flagMarker != null)
        {
            float currentX = Mathf.Lerp(startPointX, endPointX, progress);
            flagMarker.anchoredPosition = new Vector2(currentX, flagMarker.anchoredPosition.y);
        }

        if (currentFlightTime >= levelDuration)
        {
            FinishLevel();
        }
    }

    private void FinishLevel()
    {
        isLevelFinished = true;

        if (plane != null)
        {
            plane.enabled = false;
            Rigidbody2D rb = plane.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Kinematic;
                rb.linearVelocity = Vector2.zero;
            }
            Animator anim = plane.GetComponent<Animator>();
            if (anim != null)
            {
                anim.enabled = false;
            }
        }

        if (AudioManager.instance != null) AudioManager.instance.PlaySFX(AudioManager.instance.levelCompleteSound);
        levelCompletePanel.SetActive(true);

        int finalScore = ScoreManager.instance.GetScore();

        int starsToSave = 0;
        if (finalScore >= 10) starsToSave = 3;
        else if (finalScore >= 7) starsToSave = 2;
        else if (finalScore >= 3) starsToSave = 1;

        string currentLevelName = SceneManager.GetActiveScene().name;
        int previousBest = PlayerPrefs.GetInt(currentLevelName + "_BestStars", 0);

        if (starsToSave > previousBest)
        {
            PlayerPrefs.SetInt(currentLevelName + "_BestStars", starsToSave);
            PlayerPrefs.Save();
        }

        StartCoroutine(ShowStarsWithDelay(finalScore));
    }

    IEnumerator ShowStarsWithDelay(int score)
    {
        yield return new WaitForSeconds(0.5f);

        if (score >= 3)
        {
            StartCoroutine(FadeInStar(0));
            yield return new WaitForSeconds(starPopupDelay);
        }

        if (score >= 7)
        {
            StartCoroutine(FadeInStar(1));
            yield return new WaitForSeconds(starPopupDelay);
        }

        if (score >= 10)
        {
            StartCoroutine(FadeInStar(2));
        }
    }

    IEnumerator FadeInStar(int index)
    {
        GameObject star = stars[index];
        star.SetActive(true);
        star.transform.localScale = Vector3.one;

        Image starImage = star.GetComponent<Image>();
        if (starImage == null) yield break;

        if (AudioManager.instance != null) AudioManager.instance.PlaySFX(AudioManager.instance.starSound);

        Color originalColor = starImage.color;
        starImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);

        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float normalizedProgress = timer / fadeDuration;
            starImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, normalizedProgress);
            yield return null;
        }

        starImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1f);
    }
}