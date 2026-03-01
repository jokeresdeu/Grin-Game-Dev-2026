using System;
using Projects.MegaSuperChallengeShot.Scripts;
using TMPro;
using TMPro.EditorUtilities;
using UnityEngine;

public class CrosshairController : MonoBehaviour
{
    [SerializeField] private int _maxShots;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private LayerMask _target;
    
    private Camera _main;
    private int _currentShots;

    private void Start()
    {
        _main = Camera.main;
        SetMaxShots();
    }

    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 0; // Set a fixed distance from the camera
        Vector3 worldPosition = _main.ScreenToWorldPoint(mousePosition);
        worldPosition.z = 0;
        transform.position = worldPosition;

        if (Input.GetKeyDown(KeyCode.Mouse0) && _currentShots > 0 )
        {
            _currentShots--; 
            _text.text = $"{_currentShots}/{_maxShots}";
           Collider2D[] colliders = Physics2D.OverlapPointAll(worldPosition, _target);
           Debug.Log(colliders.Length);
           for (int i = 0; i < colliders.Length; i++)
           {
               ScoreManager.Instance.AddScore();
               Destroy(colliders[i].gameObject);
           }
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
           SetMaxShots();
        }
    }

    private void SetMaxShots()
    {
        _currentShots = _maxShots;
        _text.text = $"{_currentShots}/{_maxShots}";
    }
}
