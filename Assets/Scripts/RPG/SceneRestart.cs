using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameUIManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject _window;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Button _reloadButton;

    [Header("Texts")]
    [TextArea] [SerializeField] private string _playerDeathText;
    [TextArea] [SerializeField] private string _enemyDeathText;

    private void Awake()
    {
        if (_window != null)
            _window.SetActive(false);

        if (_reloadButton != null)
            _reloadButton.onClick.AddListener(ReloadScene);
    }

    public void ShowPlayerDeath()
    {
        if (_window != null)
            _window.SetActive(true);

        if (_text != null)
            _text.text = _playerDeathText;
    }

    public void ShowEnemyDeath()
    {
        if (_window != null)
            _window.SetActive(true);

        if (_text != null)
            _text.text = _enemyDeathText;
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}