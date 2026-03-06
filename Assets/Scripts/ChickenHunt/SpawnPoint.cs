using UnityEngine;

namespace ChickenHunt
{
    public class SpawnPoint : MonoBehaviour
    {
        [Header("Chickens")]
        [SerializeField] private GameObject[] _chickenPrefabs;

        [Header("Spawn Direction")]
        [SerializeField] private Vector2 _flyDirection = Vector2.left;

        public Chicken Spawn()
        {
            if (_chickenPrefabs == null || _chickenPrefabs.Length == 0) 
                return null;

            int index = Random.Range(0, _chickenPrefabs.Length);
            GameObject prefab = _chickenPrefabs[index];

            if (prefab == null) return null;

            GameObject chickenObj = Instantiate(prefab, transform.position, Quaternion.identity);
            Chicken chicken = chickenObj.GetComponent<Chicken>();

            if (chicken != null)
            {
                chicken.Initialize(_flyDirection);
            }

            return chicken;
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
