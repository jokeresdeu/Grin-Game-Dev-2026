using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace FruitSlice
{
    public class FruitsManager : MonoBehaviour
    {
        [Header("Spawn Points")]
        [SerializeField] private SpawnPoint[] _spawnPoints;

        [Header("Spawn Settings")]
        [SerializeField] private float _minSpawnTime = 0.8f;
        [SerializeField] private float _maxSpawnTime = 1.5f;
        [SerializeField] private int _maxFruits = 10;

        [Header("Bounds")]
        [SerializeField] private float _killY = -10f;

        [Header("UI")]
        [SerializeField] private TextMeshProUGUI _scoreText;

        private readonly List<Fruit> _activeFruits = new();
        private float _spawnTimer;
        private int _score;
        private bool _isSpawning;

        private void Start()
        {
            StartSpawning();
        }

        private void Update()
        {
            if (!_isSpawning) return;

            UpdateSpawning();
            CheckOutOfBounds();
        }

        private void UpdateSpawning()
        {
            _spawnTimer -= Time.deltaTime;

            if (_spawnTimer <= 0f && _activeFruits.Count < _maxFruits)
            {
                SpawnFruit();
                _spawnTimer = Random.Range(_minSpawnTime, _maxSpawnTime);
            }
        }

        private void CheckOutOfBounds()
        {
            for (int i = _activeFruits.Count - 1; i >= 0; i--)
            {
                var fruit = _activeFruits[i];

                if (fruit == null)
                {
                    _activeFruits.RemoveAt(i);
                    continue;
                }

                if (fruit.transform.position.y < _killY)
                {
                    fruit.OnSliced -= OnFruitSliced;
                    _activeFruits.RemoveAt(i);
                    Destroy(fruit.gameObject);
                }
            }
        }

        private void StartSpawning()
        {
            _isSpawning = true;
            _spawnTimer = 0f;
            _score = 0;
            UpdateScoreUI();
        }

        public void StopSpawning()
        {
            _isSpawning = false;
        }

        private void SpawnFruit()
        {
            if (_spawnPoints == null || _spawnPoints.Length == 0)
                return;

            int pointIndex = Random.Range(0, _spawnPoints.Length);
            SpawnPoint spawnPoint = _spawnPoints[pointIndex];

            if (spawnPoint == null)
                return;

            Fruit fruit = spawnPoint.Spawn();

            if (fruit != null)
            {
                fruit.OnSliced += OnFruitSliced;
                _activeFruits.Add(fruit);
            }
        }

        private void OnFruitSliced(int points)
        {
            _score += points;
            UpdateScoreUI();
        }

        private void UpdateScoreUI()
        {
            if (_scoreText != null)
                _scoreText.text = $"Score: {_score}";
        }

        private void OnDestroy()
        {
            foreach (var fruit in _activeFruits)
            {
                if (fruit != null)
                {
                    fruit.OnSliced -= OnFruitSliced;
                }
            }
        }
    }
}
