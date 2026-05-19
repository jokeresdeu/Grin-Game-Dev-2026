using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public TextMeshProUGUI shopCoinText;

    [Header("Кнопки Скінів")]
    public Button defaultBtn;
    public Button greenBtn;
    public Button goldBtn;

    [Header("Елементи Манекен")]
    public SpriteRenderer previewPlayerRender;
    public ParticleSystem previewPlayerTrail;

    private int coins;
    private Color defaultColor = Color.white;
    private Color greenColor = Color.green;
    private Color goldColor = new Color(1f, 0.84f, 0f, 1f);

    void OnEnable()
    {
        UpdateShopUI();
        UpdatePreviewVisuals();
    }

    public void UpdateShopUI()
    {
        coins = PlayerPrefs.GetInt("TotalCoins", 0);
        if (shopCoinText != null) shopCoinText.text = "Coins: " + coins;

        // Оновлюємо текст
        if (PlayerPrefs.GetInt("Skin_Green_Unlocked", 0) == 1)
            SafeSetText(greenBtn, "Green (Purchased)");
        else
            SafeSetText(greenBtn, "Green (10 Coins)");

        if (PlayerPrefs.GetInt("Skin_Gold_Unlocked", 0) == 1)
            SafeSetText(goldBtn, "Gold (Purchased)");
        else
            SafeSetText(goldBtn, "Gold (30 Coins)");

        string selected = PlayerPrefs.GetString("SelectedSkin", "Default");

        if (defaultBtn != null && defaultBtn.image != null)
            defaultBtn.image.color = (selected == "Default") ? Color.green : Color.white;

        if (greenBtn != null && greenBtn.image != null)
            greenBtn.image.color = (selected == "Green") ? Color.green : Color.white;

        if (goldBtn != null && goldBtn.image != null)
            goldBtn.image.color = (selected == "Gold") ? Color.green : Color.white;
    }

    private void SafeSetText(Button btn, string newText)
    {
        if (btn == null) return;

        TextMeshProUGUI tmpText = btn.GetComponentInChildren<TextMeshProUGUI>();
        if (tmpText != null)
        {
            tmpText.text = newText;
            return;
        }

        Text legacyText = btn.GetComponentInChildren<Text>();
        if (legacyText != null)
        {
            legacyText.text = newText;
            return;
        }
    }

    private void UpdatePreviewVisuals()
    {
        string currentSelectedInMenu = PlayerPrefs.GetString("SelectedSkin", "Default");

        Color targetColor = defaultColor;
        if (currentSelectedInMenu == "Green") targetColor = greenColor;
        else if (currentSelectedInMenu == "Gold") targetColor = goldColor;

        if (previewPlayerRender != null) previewPlayerRender.color = targetColor;

        if (previewPlayerTrail != null)
        {
            var main = previewPlayerTrail.main;
            main.startColor = targetColor;
        }
    }

    public void SelectDefaultSkin()
    {
        PlayerPrefs.SetString("SelectedSkin", "Default");
        UpdateShopUI();
        UpdatePreviewVisuals();
    }

    public void BuyOrSelectGreen()
    {
        if (PlayerPrefs.GetInt("Skin_Green_Unlocked", 0) == 1)
            PlayerPrefs.SetString("SelectedSkin", "Green");
        else if (coins >= 10)
        {
            coins -= 10;
            PlayerPrefs.SetInt("TotalCoins", coins);
            PlayerPrefs.SetInt("Skin_Green_Unlocked", 1);
            PlayerPrefs.SetString("SelectedSkin", "Green");
        }
        UpdateShopUI();
        UpdatePreviewVisuals();
    }

    public void BuyOrSelectGold()
    {
        if (PlayerPrefs.GetInt("Skin_Gold_Unlocked", 0) == 1)
            PlayerPrefs.SetString("SelectedSkin", "Gold");
        else if (coins >= 30)
        {
            coins -= 30;
            PlayerPrefs.SetInt("TotalCoins", coins);
            PlayerPrefs.SetInt("Skin_Gold_Unlocked", 1);
            PlayerPrefs.SetString("SelectedSkin", "Gold");
        }
        UpdateShopUI();
        UpdatePreviewVisuals();
    }
}