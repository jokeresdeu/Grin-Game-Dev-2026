using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public Sprite goldStarSprite;
    public Sprite emptyStarSprite;

    public GameObject[] stage1Stars;
    public GameObject[] stage2Stars;
    public GameObject[] stage3Stars;

    void Start()
    {
        ShowStars("Stage1", stage1Stars);
        ShowStars("Stage2", stage2Stars);
        ShowStars("Stage3", stage3Stars);
    }

    void ShowStars(string levelName, GameObject[] starIcons)
    {
        int earnedStars = PlayerPrefs.GetInt(levelName + "_BestStars", 0);

        for (int i = 0; i < starIcons.Length; i++)
        {
            Image starImage = starIcons[i].GetComponent<Image>();

            if (starImage != null)
            {
                if (i < earnedStars)
                {
                    starImage.sprite = goldStarSprite;
                }
                else
                {
                    starImage.sprite = emptyStarSprite;
                }
            }
        }
    }

    public void LoadLevel(string sceneName)
    {
        if (AudioManager.instance != null) AudioManager.instance.PlaySFX(AudioManager.instance.menuSelectSound);
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        if (AudioManager.instance != null) AudioManager.instance.PlaySFX(AudioManager.instance.menuSelectSound);
        Application.Quit();
    }
}