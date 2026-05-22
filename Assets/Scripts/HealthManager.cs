using UnityEngine;
using TMPro;

public class HealthManager : MonoBehaviour
{
    public static HealthManager instance;
    public int maxHealth = 3;
    private int currentHealth;
    public TextMeshProUGUI healthText;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthText();
    }

    public void LoseHealth()
    {
        currentHealth = currentHealth - 1;
        UpdateHealthText();

        if (currentHealth <= 0)
        {
            healthText.gameObject.SetActive(false);

            if (GameOver.instance != null)
            {
                GameOver.instance.TriggerGameOver();
            }
        }
    }

    private void UpdateHealthText()
    {
        if (healthText != null)
        {
            healthText.text = "HP: " + currentHealth.ToString();
        }
    }
}
