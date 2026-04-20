using UnityEngine;

public class BirdSpawner : MonoBehaviour
{
    [Header("Bird Prefabs (assign all bird type prefabs)")]
    [SerializeField] private GameObject[] _birdPrefabs;

    [Header("Spawn Points (create empty child GameObjects at desired Y positions)")]
    [SerializeField] private Transform[] _spawnPoints;

    [Header("Timing")]
    [SerializeField] private float _spawnInterval = 2f;
    [SerializeField] private float _initialDelay = 1f;
    [SerializeField] private float _minSpawnInterval = 0.8f;
    [SerializeField] private float _difficultyRampRate = 0.02f;

    [Header("Speed Variation")]
    [SerializeField] private float _minSpeed = 1.5f;
    [SerializeField] private float _maxSpeed = 4f;

    [Header("Direction")]
    [SerializeField] private bool _alternateDirections = true;
    [SerializeField] private float _spawnOffsetX = 12f;

    [Header("Limits")]
    [SerializeField] private int _maxActiveBirds = 10;

    private float _timer;
    private int _spawnCount;
    private bool _spawnFromRight;

    private void Start()
    {
        _timer = _initialDelay;

        if (_spawnPoints == null || _spawnPoints.Length == 0)
        {
            _spawnPoints = new Transform[] { transform };
        }
    }

    private void Update()
    {
        _timer -= Time.deltaTime;

        if (_timer <= 0f)
        {
            SpawnBird();
            float currentInterval = Mathf.Max(_minSpawnInterval, _spawnInterval - (_spawnCount * _difficultyRampRate));
            _timer = currentInterval;
        }
    }

    private void SpawnBird()
    {
        if (_birdPrefabs == null || _birdPrefabs.Length == 0) return;

        BirdMover[] activeBirds = FindObjectsByType<BirdMover>(FindObjectsSortMode.None);
        if (activeBirds.Length >= _maxActiveBirds)
        {
            return;
        }

        int prefabIndex = Random.Range(0, _birdPrefabs.Length);
        GameObject prefab = _birdPrefabs[prefabIndex];

        int spawnIndex = Random.Range(0, _spawnPoints.Length);
        Vector3 spawnPos = _spawnPoints[spawnIndex].position;

        bool moveRight;
        if (_alternateDirections)
        {
            moveRight = _spawnFromRight;
            _spawnFromRight = !_spawnFromRight;
        }
        else
        {
            moveRight = true;
        }

        spawnPos.x = moveRight ? -_spawnOffsetX : _spawnOffsetX;
        spawnPos.z = 0f;

        GameObject bird = Instantiate(prefab, spawnPos, Quaternion.identity);
        bird.name = $"{prefab.name}_#{_spawnCount}";

        BirdMover mover = bird.GetComponent<BirdMover>();
        if (mover != null)
        {
            mover.SetDirection(moveRight);
        }

        _spawnCount++;
    }
}
