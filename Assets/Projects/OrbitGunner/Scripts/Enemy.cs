using UnityEngine;

namespace Projects.OrbitGunner.Scripts
{
    public class Enemy : MonoBehaviour
    {
        private EnemyConfig _config;
        private SpriteRenderer _renderer;
        private int _hp;
        private bool _dead;
        private float _flash;

        private const float FlashDuration = 0.08f;
        private const float FallbackCoreRadius = 0.62f;

        public float Radius => _config != null ? _config.Radius : 0.4f;
        public Vector3 Position => transform.position;
        public bool IsDead => _dead;

        public void Init(EnemyConfig config, Vector3 spawnPosition, Sprite sprite)
        {
            _config = config;
            _hp = config.MaxHp;
            _dead = false;
            _flash = 0f;

            transform.position = spawnPosition;

            _renderer = GetComponent<SpriteRenderer>();
            if (_renderer != null)
            {
                if (sprite != null)
                    _renderer.sprite = sprite;
                _renderer.color = config.Color;
            }

            // Size the sprite to the config diameter using its real bounds, so the
            // visual matches the collision radius regardless of the sprite's native size.
            float diameter = config.Radius * 2f;
            float native = (_renderer != null && _renderer.sprite != null) ? _renderer.sprite.bounds.size.x : 1f;
            float scale = native > 0.0001f ? diameter / native : diameter;
            transform.localScale = new Vector3(scale, scale, 1f);

            EnemyRegistry.Register(this);
        }

        private void Update()
        {
            if (_dead)
                return;

            if (GameManager.Instance != null && GameManager.Instance.State != GameState.Playing)
                return;

            Vector3 toCore = -transform.position;
            float distance = toCore.magnitude;

            float coreRadius = CoreHealth.Instance != null ? CoreHealth.Instance.Radius : FallbackCoreRadius;
            if (distance <= coreRadius + Radius)
            {
                ReachCore();
                return;
            }

            Vector3 step = toCore.normalized * (_config.Speed * Time.deltaTime);
            transform.position += step;

            if (_config.Type == EnemyType.Runner)
            {
                float angle = Mathf.Atan2(toCore.y, toCore.x) * Mathf.Rad2Deg - 90f;
                transform.rotation = Quaternion.Euler(0f, 0f, angle);
            }

            UpdateFlash();
        }

        public void TakeDamage(int amount)
        {
            if (_dead)
                return;

            _hp -= amount;
            _flash = FlashDuration;

            if (_hp <= 0)
                Kill(awardReward: true);
        }

        public void KillByNova()
        {
            if (_dead)
                return;

            Kill(awardReward: true);
        }

        private void Kill(bool awardReward)
        {
            if (_dead)
                return;

            _dead = true;

            if (awardReward)
            {
                if (ScoreManager.Instance != null)
                    ScoreManager.Instance.RegisterKill(_config.Score);
                if (OverdriveMeter.Instance != null)
                    OverdriveMeter.Instance.AddCharge(_config.Overdrive);
            }

            SpawnPop();
            Despawn();
        }

        private void ReachCore()
        {
            if (_dead)
                return;

            _dead = true;

            if (CoreHealth.Instance != null)
                CoreHealth.Instance.TakeDamage(1);

            SpawnPop();
            Despawn();
        }

        private void SpawnPop()
        {
            Sprite sprite = _renderer != null ? _renderer.sprite : null;
            if (sprite == null)
                return;

            Color c = _config.Color;
            c.a = 0.85f;
            ExpandingSprite.Spawn(sprite, c, transform.position,
                _config.Radius * 2f, _config.Radius * 4.5f, 0.3f, 4);
        }

        private void Despawn()
        {
            EnemyRegistry.Unregister(this);
            Destroy(gameObject);
        }

        private void UpdateFlash()
        {
            if (_renderer == null)
                return;

            if (_flash > 0f)
            {
                _flash -= Time.deltaTime;
                float t = Mathf.Clamp01(_flash / FlashDuration);
                _renderer.color = Color.Lerp(_config.Color, Color.white, t);
            }
            else
            {
                _renderer.color = _config.Color;
            }
        }

        private void OnDestroy()
        {
            EnemyRegistry.Unregister(this);
        }
    }
}
