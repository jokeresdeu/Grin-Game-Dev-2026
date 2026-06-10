using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    public enum UpgradeType
    {
        ClickPower,
        AutoIncome
    }

    [Header("Upgrade Settings")]
    [SerializeField] private string upgradeId = "upgrade_1";
    [SerializeField] private string upgradeName = "Upgrade";
    [SerializeField] private UpgradeType upgradeType;

    [Header("Balance")]
    [SerializeField] private double baseCost = 10;
    [SerializeField] private double costMultiplier = 1.5;
    [SerializeField] private double addValue = 1;

    [Header("UI")]
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text costText;
    [SerializeField] private Button buyButton;

    private int level;

    private string SaveKey => "upgrade_level_" + upgradeId;

    private void Start()
    {
        if (buyButton == null)
            buyButton = GetComponent<Button>();

        level = PlayerPrefs.GetInt(SaveKey, 0);

        if (buyButton != null)
            buyButton.onClick.AddListener(BuyUpgrade);

        if (GameManager.Instance != null)
            GameManager.Instance.OnStatsChanged += UpdateUI;

        UpdateUI();
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnStatsChanged -= UpdateUI;
    }

    public void BuyUpgrade()
    {
        double cost = GetCurrentCost();

        if (!GameManager.Instance.TrySpendCoins(cost))
            return;

        level++;

        if (upgradeType == UpgradeType.ClickPower)
        {
            GameManager.Instance.AddClickPower(addValue);
        }
        else if (upgradeType == UpgradeType.AutoIncome)
        {
            GameManager.Instance.AddAutoIncome(addValue);
        }

        PlayerPrefs.SetInt(SaveKey, level);
        PlayerPrefs.Save();

        UpdateUI();
    }

    private double GetCurrentCost()
    {
        return baseCost * Math.Pow(costMultiplier, level);
    }

    private void UpdateUI()
    {
        double cost = GetCurrentCost();

        if (titleText != null)
            titleText.text = upgradeName;

        if (levelText != null)
            levelText.text = "Level: " + level;

        if (costText != null)
            costText.text = "Cost: " + GameManager.Instance.FormatNumber(cost);

        if (buyButton != null)
            buyButton.interactable = GameManager.Instance.CanAfford(cost);
    }
}