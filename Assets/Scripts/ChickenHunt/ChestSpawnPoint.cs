using UnityEngine;

namespace ChickenHunt
{
    public class ChestSpawnPoint : MonoBehaviour
    {
        [SerializeField] private Chest _chestPrefab;
        [SerializeField] private Vector2 _flyDirection = Vector2.left;

        public Chest Spawn(ChickensManager manager)
        {
            if (_chestPrefab == null)
                return null;

            Chest chest = Instantiate(_chestPrefab, transform.position, Quaternion.identity);
            chest.Initialize(_flyDirection, manager);
            return chest;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, 0.35f);

            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + (Vector3)_flyDirection.normalized * 1.5f);
        }
    }
}