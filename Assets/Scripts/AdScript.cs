using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdScript : MonoBehaviour, IUnityAdsInitializationListener,
    IUnityAdsLoadListener, IUnityAdsShowListener
{
    public static GameObject playRewardVideoButton;
    public static GameObject skipVideoButton;
    public static int numOfDeaths;
    private static bool isPlaying = false;
    private static LevelManager levelManager;

    private const string _androidGameId = "1580635";
    private const string _videoAdUnitId = "Interstitial_Android";
    private const string _rewardedAdUnitId = "Rewarded_Android";

    void Start()
    {
        levelManager = GameObject.Find("Level Manager").GetComponent<LevelManager>();
        AdScript.playRewardVideoButton = GameObject.Find("Reward Video");
        AdScript.skipVideoButton = GameObject.Find("Continue");
        AdScript.playRewardVideoButton.SetActive(false);
        AdScript.skipVideoButton.SetActive(false);
        AdScript.numOfDeaths = PlayerPrefsManager.GetAdDeaths();

        if (Advertisement.isSupported)
            Advertisement.Initialize(_androidGameId, false, this);
    }

    public void OnInitializationComplete()
    {
        Advertisement.Load(_videoAdUnitId, this);
        Advertisement.Load(_rewardedAdUnitId, this);
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.LogError($"Ads init failed: {error} - {message}");
    }

    public void OnUnityAdsAdLoaded(string adUnitId) { }
    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.LogError($"Ad failed to load: {adUnitId} - {error} - {message}");
    }

    public static void ShowAd(bool died)
    {
        if (died && AdScript.numOfDeaths >= 6 || !died)
        {
            AdScript.numOfDeaths = 0;
            PlayerPrefsManager.SetAdDeaths(0);
            if (!AdScript.isPlaying)
            {
                Advertisement.Show("Interstitial_Android",
                    FindObjectOfType<AdScript>());
                AdScript.isPlaying = true;
            }
        }
        else if (died && AdScript.numOfDeaths == 3 || !died)
        {
            AdScript.numOfDeaths++;
            PlayerPrefsManager.SetAdDeaths(AdScript.numOfDeaths);
            AdScript.playRewardVideoButton.SetActive(true);
            AdScript.skipVideoButton.SetActive(true);
            Time.timeScale = 0;
        }
        else if (died)
        {
            AdScript.numOfDeaths++;
            PlayerPrefsManager.SetAdDeaths(AdScript.numOfDeaths);
            Time.timeScale = 1;
            levelManager.MenuLoadLevel(PlayerPrefsManager.GetMap());
        }
    }

    public void PlayRewardVideo()
    {
        if (!AdScript.isPlaying)
        {
            Advertisement.Show(_rewardedAdUnitId, this);
            AdScript.isPlaying = true;
        }
    }

    public void SkipRewardVideo()
    {
        Time.timeScale = 1;
        playRewardVideoButton.SetActive(false);
        skipVideoButton.SetActive(false);
        levelManager.MenuLoadLevel(PlayerPrefsManager.GetMap());
    }

    public void OnUnityAdsShowComplete(string adUnitId,
        UnityAdsShowCompletionState showCompletionState)
    {
        AdScript.isPlaying = false;
        Time.timeScale = 1;

        if (adUnitId == _rewardedAdUnitId &&
            showCompletionState == UnityAdsShowCompletionState.COMPLETED)
        {
            int coins = PlayerPrefsManager.GetNumOfCoins();
            coins += 5;
            PlayerPrefsManager.SetNumOfCoins(coins);
        }

        levelManager.MenuLoadLevel(PlayerPrefsManager.GetMap());
        Advertisement.Load(adUnitId, this);
    }

    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        Debug.LogError($"Ad show failed: {adUnitId} - {error} - {message}");
        AdScript.isPlaying = false;
    }

    public void OnUnityAdsShowStart(string adUnitId) { }
    public void OnUnityAdsShowClick(string adUnitId) { }
}