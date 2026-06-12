using UnityEngine;

namespace OneButtonGame
{
    public class StoneSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject _stonePrefab;
        [SerializeField] private float _spawnRate = 1.5f; // Частота спавну
        [SerializeField] private float _heightOffset = 4f; // Розкид вгору/вниз

        private float _timer = 0f;

        private void Start()
        {
            SpawnStone();
        }

        private void Update()
        {
            if (Time.timeScale == 0f) return;

            _timer += Time.deltaTime;
            if (_timer >= _spawnRate)
            {
                SpawnStone();
                _timer = 0f;
            }
        }

        private void SpawnStone()
        {
            float lowestPoint = transform.position.y - _heightOffset;
            float highestPoint = transform.position.y + _heightOffset;

            // Спавнимо праворуч за межами видимості (X: 12)
            Vector3 spawnPosition = new Vector3(12f, Random.Range(lowestPoint, highestPoint), 0f);
            GameObject newStone = Instantiate(_stonePrefab, spawnPosition, Quaternion.identity);

            // Випадковий розмір каменя від 0.6 до 1.4
            float randomScale = Random.Range(0.6f, 1.4f);
            newStone.transform.localScale = new Vector3(randomScale, randomScale, 1f);
        }
    }
}