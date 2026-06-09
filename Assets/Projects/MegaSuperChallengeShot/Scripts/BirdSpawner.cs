using Projects.MegaSuperChallengeShot.Scripts;
using UnityEngine;

public class BirdSpawner : MonoBehaviour
{
    [SerializeField] private BirdMover _birdPrefab;
    [SerializeField] private float _spawnInterval = 1.5f;
    [SerializeField] private float _minY = -3f;
    [SerializeField] private float _maxY = 3f;

    private float _timer;

    private void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.State != GameState.Playing)
            return;

        _timer += Time.deltaTime;
        if (_timer < _spawnInterval)
            return;

        _timer = 0f;
        Spawn();
    }

    private void Spawn()
    {
        Vector3 spawnPos = new Vector3(
            transform.position.x,
            Random.Range(_minY, _maxY),
            0f);

        Instantiate(_birdPrefab, spawnPos, Quaternion.identity);
    }
}
