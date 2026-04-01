using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private TMP_Text _nameText; //Text
    [SerializeField] private TMP_Text _level;
    [SerializeField] public Image _hpBar; //Slider
    [SerializeField] private Image _mpBar;
    [SerializeField] private Button _restartButton;

    private void Awake()
    {
        _restartButton.onClick.AddListener(() =>
        {
            Debug.Log("Button us pressed");
            UpdateHp(0.5f);
        });
        _restartButton.onClick.AddListener(Restart);
    }

    private void OnDestroy()
    {
        _restartButton.onClick.RemoveAllListeners();
    }

    public void SetPlayer(string playerName, string level)
    {
        _nameText.text = playerName;
        _level.text = level;
    }

    public void UpdateHp(float value)
    {
        _hpBar.fillAmount = value;
    }

    public void UpdateMp(float value)
    {
        _mpBar.fillAmount = value;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
