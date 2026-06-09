using UnityEngine;

namespace ChickenHunt
{
    public class SpawnPoint : MonoBehaviour
    {
        [Header("Spawn Settings")]
        [SerializeField] private Vector2 _flyDirection = Vector2.right;

        public Chicken Spawn(Chicken prefab)
        {
            if (prefab == null)
                return null;

            Chicken chicken = Instantiate(prefab, transform.position, Quaternion.identity);
            chicken.Initialize(_flyDirection);
            return chicken;
        }
    }
}