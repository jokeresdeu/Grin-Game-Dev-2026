using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject _fireballPrefab;
    [SerializeField] private float _spawnInterval = 2f;
    [SerializeField] private float _spawnHeight = 5f;
    [SerializeField] private int _playerMaxHp = 3;

    private int _playerHp;
    private GameObject _player;
    private float _timer;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerHp = _playerMaxHp;
        _timer = _spawnInterval;
    }

    private void Update()
    {
        if (_player == null) return;

        _timer -= Time.deltaTime;
        if (_timer <= 0f)
        {
            SpawnFireball();
            _timer = _spawnInterval;
        }
    }

    private void SpawnFireball()
    {
        if (_fireballPrefab == null || _player == null) return;

        Vector3 spawnPos = new Vector3(
            _player.transform.position.x,
            _player.transform.position.y + _spawnHeight,
            0f
        );

        Instantiate(_fireballPrefab, spawnPos, Quaternion.identity);
    }

    public void TakeDamage(int damage)
    {
        _playerHp -= damage;
        Debug.Log("HP: " + _playerHp);

        if (_playerHp <= 0)
        {
            Debug.Log("Game Over!");
            Invoke(nameof(Restart), 1.5f);
        }
    }

    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}