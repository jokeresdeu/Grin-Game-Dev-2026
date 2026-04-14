using System.Collections;
using UnityEngine;
using UkraineVsZombies;

public class WaveManager : MonoBehaviour
{
    [Header("Spawn")]
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private Enemy _enemyPrefab;
    [SerializeField] private float _spawnDelay = 0.5f;
    [SerializeField] private float _delayBetweenWaves = 2f;

    [Header("Wave Settings")]
    [SerializeField] private int _startEnemiesCount = 3;
    [SerializeField] private int _enemiesIncreasePerWave = 2;

    [Header("References")]
    [SerializeField] private GameUI _gameUI;

    private int _currentWave = 0;
    private int _enemiesLeft = 0;
    private bool _waveInProgress = false;

    private void Start()
    {
        StartNextWave();
    }

    private void StartNextWave()
    {
        _currentWave++;
        int enemiesThisWave = _startEnemiesCount + (_currentWave - 1) * _enemiesIncreasePerWave;

        _enemiesLeft = enemiesThisWave;
        _waveInProgress = true;

        if (_gameUI != null)
        {
            _gameUI.UpdateWaveUI(_currentWave);
            _gameUI.UpdateEnemiesUI(_enemiesLeft);
        }

        StartCoroutine(SpawnWave(enemiesThisWave));
    }

    private IEnumerator SpawnWave(int count)
    {
        for (int i = 0; i < count; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(_spawnDelay);
        }
    }

    private void SpawnEnemy()
    {
        if (_enemyPrefab == null || _spawnPoints == null || _spawnPoints.Length == 0)
        {
            Debug.LogWarning("WaveManager: enemy prefab or spawn points are missing.");
            return;
        }

        int randomIndex = Random.Range(0, _spawnPoints.Length);
        Transform spawnPoint = _spawnPoints[randomIndex];

        Enemy enemy = Instantiate(_enemyPrefab, spawnPoint.position, Quaternion.identity);
        enemy.Initialize();
        enemy.OnDeath += HandleEnemyDeath;
    }

    private void HandleEnemyDeath()
    {
        _enemiesLeft--;

        Debug.Log("Enemy died. Left: " + _enemiesLeft);

        if (_gameUI != null)
        {
            _gameUI.UpdateEnemiesUI(Mathf.Max(0, _enemiesLeft));
            _gameUI.AddScore(1);
        }

        if (_enemiesLeft <= 0 && _waveInProgress)
        {
            _waveInProgress = false;
            StartCoroutine(StartNextWaveWithDelay());
        }
    }

    private IEnumerator StartNextWaveWithDelay()
    {
        yield return new WaitForSeconds(_delayBetweenWaves);
        StartNextWave();
    }
}