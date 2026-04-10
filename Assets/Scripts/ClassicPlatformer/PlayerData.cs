using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance;

    [Header("Player Stats")]
    public int CurrentHealth = 3;
    public int MaxHealth = 3;

    public int CurrentLevel = 1;
    public int CurrentExperience = 0;
    public int ExperienceToNextLevel = 50;

    public int AttackDamage = 1;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ResetData()
    {
        CurrentHealth = 3;
        MaxHealth = 3;
        CurrentLevel = 1;
        CurrentExperience = 0;
        ExperienceToNextLevel = 50;
        AttackDamage = 1;
    }
}