using UnityEngine;

namespace RPG
{
    public class ProceduralSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject _enemyPrefab;
        public GameObject chestPrefab;

        [SerializeField] private float _spawnXOffset = 15f;
        [SerializeField] private float _minY = -4.5f;
        [SerializeField] private float _maxY = -2.5f;

        private Camera _mainCamera;
        private float _timer;
        private int _currentWave = 1;
        private int _aliveEnemies = 0;
        private bool _waitingForNextWave = true;
        private bool _isGameFinished = false;

        private void Start()
        {
            _mainCamera = Camera.main;
            _timer = 2f;
        }

        private RPGChest _spawnedChest;

        private void Update()
        {
            if (RPGGameManager.Instance != null && !RPGGameManager.Instance.IsArenaPaused)
            {
                if (_isGameFinished && _spawnedChest != null)
                {
                    if (_spawnedChest.transform.position.x <= _mainCamera.transform.position.x + 6f)
                    {
                        RPGGameManager.Instance.PauseArena();
                    }
                }
                else if (_aliveEnemies > 0)
                {
                    var enemies = FindObjectsByType<RPGEnemy>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
                    foreach (var enemy in enemies)
                    {
                        if (enemy.transform.position.x <= _mainCamera.transform.position.x + 8f)
                        {
                            RPGGameManager.Instance.PauseArena();
                            break;
                        }
                    }
                }
            }

            if (_isGameFinished) return;

            if (RPGGameManager.Instance != null && RPGGameManager.Instance.IsArenaPaused && _waitingForNextWave)
            {

                RPGGameManager.Instance.ResumeArena();
            }

            if (_waitingForNextWave)
            {
                _timer -= Time.deltaTime;
                if (_timer <= 0f)
                {
                    StartWave();
                }
            }
        }

        private void StartWave()
        {
            if (_mainCamera == null) return;

            _waitingForNextWave = false;

            if (_currentWave <= 3)
            {
                int wolvesToSpawn = _currentWave;
                for (int i = 0; i < wolvesToSpawn; i++)
                {
                    SpawnEnemy(i, wolvesToSpawn);
                }

            }
            else
            {
                SpawnChest();
            }
        }

        private void SpawnEnemy(int index, int totalInWave)
        {
            if (_enemyPrefab == null) return;
            
            float spawnX = _mainCamera.transform.position.x + 15f;
            

            float spawnY = -3.5f;
            if (totalInWave == 2)
            {
                spawnY = index == 0 ? -2.5f : -4.5f;
            }
            else if (totalInWave == 3)
            {
                if (index == 0) spawnY = -2.5f;
                else if (index == 1) spawnY = -3.5f;
                else spawnY = -4.5f;
            }

            Vector3 spawnPos = new Vector3(spawnX, spawnY, 0f);

            var enemy = Instantiate(_enemyPrefab, spawnPos, Quaternion.identity);
            _aliveEnemies++;
        }

        private void SpawnChest()
        {
            if (chestPrefab == null) return;
            _isGameFinished = true;

            float spawnX = _mainCamera.transform.position.x + 15f;
            float spawnY = -3.5f;
            Vector3 spawnPos = new Vector3(spawnX, spawnY, 0f);

            var chestObj = Instantiate(chestPrefab, spawnPos, Quaternion.identity);
            _spawnedChest = chestObj.GetComponent<RPGChest>();
            

            if (chestObj.GetComponent<MoveLeft>() == null)
            {
                chestObj.AddComponent<MoveLeft>().speed = 5f;
            }
        }

        public void EnemyDefeated()
        {
            _aliveEnemies--;
            if (_aliveEnemies <= 0 && !_isGameFinished)
            {
                _aliveEnemies = 0;
                _currentWave++;
                _waitingForNextWave = true;
                _timer = 4f;
                
                if (RPGGameManager.Instance != null)
                    RPGGameManager.Instance.ResumeArena();
            }
        }
        
        public void SetPrefabs(GameObject enemy)
        {
            _enemyPrefab = enemy;
        }
    }
}


