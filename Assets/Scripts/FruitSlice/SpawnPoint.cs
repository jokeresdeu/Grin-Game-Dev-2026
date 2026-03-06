using UnityEngine;

namespace FruitSlice
{
    public class SpawnPoint : MonoBehaviour
    {
        [Header("Fruits")]
        [SerializeField] private GameObject[] _fruitPrefabs;

        [Header("Launch")]
        [SerializeField] private float _minLaunchForce = 10f;
        [SerializeField] private float _maxLaunchForce = 15f;
        [SerializeField] private float _launchAngleVariation = 15f;

        public Fruit Spawn()
        {
            if (_fruitPrefabs == null || _fruitPrefabs.Length == 0)
                return null;

            int index = Random.Range(0, _fruitPrefabs.Length);
            GameObject prefab = _fruitPrefabs[index];

            if (prefab == null) return null;

            GameObject fruitObj = Instantiate(prefab, transform.position, Quaternion.identity);
            Fruit fruit = fruitObj.GetComponent<Fruit>();

            if (fruit != null)
            {
                Vector2 launchDir = CalculateLaunchDirection();
                float force = Random.Range(_minLaunchForce, _maxLaunchForce);
                float angular = Random.Range(-180f, 180f);
                fruit.Initialize(launchDir, force, angular);
            }

            return fruit;
        }

        private Vector2 CalculateLaunchDirection()
        {
            float baseAngle = 90f;

            if (transform.position.x < 0)
                baseAngle += Random.Range(0f, _launchAngleVariation);
            else
                baseAngle -= Random.Range(0f, _launchAngleVariation);

            float angleRad = baseAngle * Mathf.Deg2Rad;
            return new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, 0.3f);

            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, transform.position + Vector3.up * 2f);
        }
    }
}
