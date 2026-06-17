using UnityEngine;

namespace Projects.OrbitGunner.Scripts
{

    public class NovaBurst : MonoBehaviour
    {
        public static NovaBurst Instance { get; private set; }

        [SerializeField] private Sprite _ringSprite;
        [SerializeField] private Color _ringColor = new Color(0.45f, 0.95f, 1f, 0.9f);
        [SerializeField] private float _ringDuration = 0.55f;
        [SerializeField] private int _ringSortingOrder = 6;

        private bool _bursting;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        public void Trigger()
        {

            if (_bursting)
                return;
            _bursting = true;

            float radius = EnemySpawner.Instance != null ? EnemySpawner.Instance.SpawnRadius : 11f;

            if (_ringSprite != null)
                ExpandingSprite.Spawn(
                    _ringSprite,
                    _ringColor,
                    Vector3.zero,
                    1f,
                    radius * 2f,
                    _ringDuration,
                    _ringSortingOrder);

            if (CameraShake.Instance != null)
                CameraShake.Instance.Shake(0.7f);

            foreach (Enemy enemy in EnemyRegistry.Snapshot())
            {
                if (enemy != null && !enemy.IsDead)
                    enemy.KillByNova();
            }

            _bursting = false;
        }

        private void OnDestroy()
        {
            if (Instance == this)
                Instance = null;
        }
    }
}
