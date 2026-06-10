using System;
using System.Collections;
using System.Globalization;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI")]
    [SerializeField] private TMP_Text coinsText;
    [SerializeField] private TMP_Text clickPowerText;
    [SerializeField] private TMP_Text autoIncomeText;

    [Header("Start Values")]
    [SerializeField] private double startCoins = 0;
    [SerializeField] private double startCoinsPerClick = 1;
    [SerializeField] private double startAutoCoinsPerSecond = 0;

    public double Coins { get; private set; }
    public double CoinsPerClick { get; private set; }
    public double AutoCoinsPerSecond { get; private set; }

    public event Action OnStatsChanged;

    private const string CoinsKey = "coins";
    private const string CoinsPerClickKey = "coins_per_click";
    private const string AutoCoinsKey = "auto_coins_per_second";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        LoadGame();
    }

    private void Start()
    {
        UpdateUI();
        StartCoroutine(AutoIncomeRoutine());
    }

    public void Click()
    {
        AddCoins(CoinsPerClick);
    }

    public void AddCoins(double amount)
    {
        Coins += amount;
        UpdateUI();
        SaveGame();
        OnStatsChanged?.Invoke();
    }

    public bool CanAfford(double amount)
    {
        return Coins >= amount;
    }

    public bool TrySpendCoins(double amount)
    {
        if (!CanAfford(amount))
            return false;

        Coins -= amount;
        UpdateUI();
        SaveGame();
        OnStatsChanged?.Invoke();

        return true;
    }

    public void AddClickPower(double amount)
    {
        CoinsPerClick += amount;
        UpdateUI();
        SaveGame();
        OnStatsChanged?.Invoke();
    }

    public void AddAutoIncome(double amount)
    {
        AutoCoinsPerSecond += amount;
        UpdateUI();
        SaveGame();
        OnStatsChanged?.Invoke();
    }

    private IEnumerator AutoIncomeRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            if (AutoCoinsPerSecond > 0)
            {
                AddCoins(AutoCoinsPerSecond);
            }
        }
    }

    private void UpdateUI()
    {
        if (coinsText != null)
            coinsText.text = "Coins: " + FormatNumber(Coins);

        if (clickPowerText != null)
            clickPowerText.text = "Per click: +" + FormatNumber(CoinsPerClick);

        if (autoIncomeText != null)
            autoIncomeText.text = "Per second: +" + FormatNumber(AutoCoinsPerSecond);
    }

    public void SaveGame()
    {
        PlayerPrefs.SetString(CoinsKey, DoubleToString(Coins));
        PlayerPrefs.SetString(CoinsPerClickKey, DoubleToString(CoinsPerClick));
        PlayerPrefs.SetString(AutoCoinsKey, DoubleToString(AutoCoinsPerSecond));
        PlayerPrefs.Save();
    }

    private void LoadGame()
    {
        Coins = GetDouble(CoinsKey, startCoins);
        CoinsPerClick = GetDouble(CoinsPerClickKey, startCoinsPerClick);
        AutoCoinsPerSecond = GetDouble(AutoCoinsKey, startAutoCoinsPerSecond);
    }

    private string DoubleToString(double value)
    {
        return value.ToString("R", CultureInfo.InvariantCulture);
    }

    private double GetDouble(string key, double defaultValue)
    {
        string savedValue = PlayerPrefs.GetString(key, "");

        if (double.TryParse(savedValue, NumberStyles.Float, CultureInfo.InvariantCulture, out double result))
            return result;

        return defaultValue;
    }

    public string FormatNumber(double value)
    {
        if (value >= 1_000_000_000)
            return (value / 1_000_000_000).ToString("0.##") + "B";

        if (value >= 1_000_000)
            return (value / 1_000_000).ToString("0.##") + "M";

        if (value >= 1_000)
            return (value / 1_000).ToString("0.##") + "K";

        return Math.Floor(value).ToString();
    }
}