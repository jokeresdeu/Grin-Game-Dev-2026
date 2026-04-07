using UnityEngine;
using TMPro;

public class CrosshairController : MonoBehaviour
{
    [Header("Налаштування стрільби")]
    [SerializeField] private int _maxShots = 5;
    [SerializeField] private LayerMask _target; 

    [Header("UI Елементи")]
    [SerializeField] private TMP_Text _text;

    private Camera _main;
    private int _currentShots;
    private ScoreManager _scoreManager;

    private void Start()
    {
        _main = Camera.main;

        _scoreManager = FindObjectOfType<ScoreManager>();

        SetMaxShots();

        Cursor.visible = false;
    }

    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 worldPosition = _main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 10f));
        worldPosition.z = 0;
        transform.position = worldPosition;

        if (Input.GetKeyDown(KeyCode.Mouse0) && _currentShots > 0)
        {
            Shoot(worldPosition);
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            SetMaxShots();
        }
    }

    private void Shoot(Vector3 position)
    {
        _currentShots--;
        UpdateUI();

        Collider2D[] colliders = Physics2D.OverlapPointAll(position, _target);

        foreach (var hit in colliders)
        {
            if (_scoreManager != null)
            {
                _scoreManager.AddScore(); 
            }

            Destroy(hit.gameObject); 
        }
    }

    private void SetMaxShots()
    {
        _currentShots = _maxShots;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (_text != null)
        {
            _text.text = $"{_currentShots}/{_maxShots}";
        }
    }
}