using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerView : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TMP_Text _nameText;    // Ім'я гравця
    [SerializeField] private TMP_Text _levelText;   // Рівень
    [SerializeField] private Image _hpBar;          // HP бар (fillAmount)
    [SerializeField] private Image _expBar;         // EXP бар (fillAmount)
    [SerializeField] private Button _restartButton; // Кнопка рестарту

    private void Awake()
    {
        if (_restartButton != null)
        {
            _restartButton.onClick.AddListener(() =>
            {
                Debug.Log("Restart button pressed");
                Restart();
            });
        }
    }

    private void OnDestroy()
    {
        if (_restartButton != null)
            _restartButton.onClick.RemoveAllListeners();
    }

    /// <summary>
    /// Встановлює ім'я та рівень гравця
    /// </summary>
    public void SetPlayer(string playerName, int level)
    {
        if (_nameText != null)
            _nameText.text = playerName;

        if (_levelText != null)
            _levelText.text = $"Lv {level}";
    }

    /// <summary>
    /// Оновлює HP бар
    /// </summary>
    public void UpdateHp(float normalizedValue)
    {
        if (_hpBar != null)
            _hpBar.fillAmount = Mathf.Clamp01(normalizedValue);
    }

    /// <summary>
    /// Оновлює EXP бар
    /// </summary>
    public void UpdateExp(float normalizedValue)
    {
        if (_expBar != null)
            _expBar.fillAmount = Mathf.Clamp01(normalizedValue);
    }

    /// <summary>
    /// Оновлює усі UI-елементи разом
    /// </summary>
    public void UpdateAll(float hpNormalized, float expNormalized, int level)
    {
        UpdateHp(hpNormalized);
        UpdateExp(expNormalized);
        if (_levelText != null)
            _levelText.text = $"Lv {level}";
    }

    /// <summary>
    /// Рестарт сцени
    /// </summary>
    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void Menu()
    {
        SceneManager.LoadScene(0);
    }
}