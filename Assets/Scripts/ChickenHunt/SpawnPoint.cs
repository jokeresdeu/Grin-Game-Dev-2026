using UnityEngine;

namespace ChickenHunt
{
    public class SpawnPoint : MonoBehaviour
    {
        [Header("Normal Chickens")]
        [SerializeField] private Chicken[] _normalChickenPrefabs;

        [Header("Armored Chickens")]
        [SerializeField] private Chicken[] _armoredChickenPrefabs;

        [Header("Spawn Weights")]
        [SerializeField] private int _normalWeight = 10;
        [SerializeField] private int _armoredWeight = 2;

        [Header("Spawn Direction")]
        [SerializeField] private Vector2 _flyDirection = Vector2.left;

        public Chicken Spawn()
        {
            Chicken prefabToSpawn = GetRandomChickenPrefab();

            if (prefabToSpawn == null)
                return null;

            Chicken chicken = Instantiate(prefabToSpawn, transform.position, Quaternion.identity);

            if (chicken != null)
                chicken.Initialize(_flyDirection);

            return chicken;
        }

        private Chicken GetRandomChickenPrefab()
        {
            bool spawnArmored = ShouldSpawnArmored();

            if (spawnArmored)
            {
                if (_armoredChickenPrefabs != null && _armoredChickenPrefabs.Length > 0)
                {
                    int index = Random.Range(0, _armoredChickenPrefabs.Length);
                    return _armoredChickenPrefabs[index];
                }
            }
            else
            {
                if (_normalChickenPrefabs != null && _normalChickenPrefabs.Length > 0)
                {
                    int index = Random.Range(0, _normalChickenPrefabs.Length);
                    return _normalChickenPrefabs[index];
                }
            }

            if (_normalChickenPrefabs != null && _normalChickenPrefabs.Length > 0)
            {
                int index = Random.Range(0, _normalChickenPrefabs.Length);
                return _normalChickenPrefabs[index];
            }

            if (_armoredChickenPrefabs != null && _armoredChickenPrefabs.Length > 0)
            {
                int index = Random.Range(0, _armoredChickenPrefabs.Length);
                return _armoredChickenPrefabs[index];
            }

            return null;
        }

        private bool ShouldSpawnArmored()
        {
            int totalWeight = _normalWeight + _armoredWeight;
            if (totalWeight <= 0)
                return false;

            int roll = Random.Range(0, totalWeight);
            return roll >= _normalWeight;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, 0.3f);

            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, transform.position + (Vector3)_flyDirection.normalized * 1.5f);
        }
    }
}