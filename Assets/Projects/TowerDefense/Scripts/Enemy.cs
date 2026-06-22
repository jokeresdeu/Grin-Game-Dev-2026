using UnityEngine;

namespace Projects.TowerDefense.Scripts
{
    /// <summary>
    /// A path-following enemy. Physics-free: walks waypoint to waypoint toward the base,
    /// damages the base on arrival (a "leak"), awards gold on death. Sized from its sprite
    /// bounds to the config radius, flashes white on hit, and darkens as HP drops.
    /// </summary>
    public class Enemy : MonoBehaviour
    {
        private const float FlashDuration = 0.08f;

        private EnemyConfig _config;
        private EnemyPath _path;
        private SpriteRenderer _renderer;
        private int _hp;
        private int _targetIndex;
        private float _distanceTraveled;
        private bool _dead;
        private float _flash;

        public float Radius => _config != null ? _config.Radius : 0.3f;
        public Vector3 Position => transform.position;
        public bool IsDead => _dead;
        public float Progress => _distanceTraveled;

        public void Init(EnemyConfig config, EnemyPath path)
        {
            _config = config;
            _path = path;
            _hp = config.MaxHp;
            _dead = false;
            _flash = 0f;
            _distanceTraveled = 0f;
            _targetIndex = 1;

            transform.position = path.GetPoint(0);

            _renderer = GetComponent<SpriteRenderer>();
            if (_renderer != null)
            {
                float diameter = config.Radius * 2f;
                float native = _renderer.sprite != null ? _renderer.sprite.bounds.size.x : 1f;
                float s = native > 0.0001f ? diameter / native : diameter;
                transform.localScale = new Vector3(s, s, 1f);
                ApplyColor();
            }

            EnemyRegistry.Register(this);
        }

        private void Update()
        {
            if (_dead || _path == null)
                return;

            if (GameManager.Instance != null && GameManager.Instance.State != GameState.Playing)
                return;

            Vector3 target = _path.GetPoint(_targetIndex);
            Vector3 to = target - transform.position;
            float dist = to.magnitude;
            float step = _config.Speed * Time.deltaTime;

            if (step >= dist)
            {
                transform.position = target;
                _distanceTraveled += dist;
                _targetIndex++;
                if (_targetIndex >= _path.Count)
                {
                    ReachBase();
                    return;
                }
            }
            else if (dist > 0.0001f)
            {
                Vector3 dir = to / dist;
                transform.position += dir * step;
                _distanceTraveled += step;
                transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
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
                Die(true);
        }

        private void Die(bool reward)
        {
            if (_dead)
                return;

            _dead = true;
            if (reward && ResourceManager.Instance != null)
                ResourceManager.Instance.AddGold(_config.Bounty);

            Despawn();
        }

        private void ReachBase()
        {
            if (_dead)
                return;

            _dead = true;
            if (BaseHealth.Instance != null)
                BaseHealth.Instance.TakeDamage(_config.LeakDamage);

            Despawn();
        }

        private void Despawn()
        {
            EnemyRegistry.Unregister(this);
            Destroy(gameObject);
        }

        private void UpdateFlash()
        {
            if (_flash > 0f)
                _flash -= Time.deltaTime;
            ApplyColor();
        }

        private void ApplyColor()
        {
            if (_renderer == null || _config == null)
                return;

            float frac = _config.MaxHp > 0 ? Mathf.Clamp01((float)_hp / _config.MaxHp) : 1f;
            Color dark = _config.Color * 0.45f;
            dark.a = 1f;
            Color hpColor = Color.Lerp(dark, _config.Color, frac);

            if (_flash > 0f)
            {
                float t = Mathf.Clamp01(_flash / FlashDuration);
                _renderer.color = Color.Lerp(hpColor, Color.white, t);
            }
            else
            {
                _renderer.color = hpColor;
            }
        }

        private void OnDestroy()
        {
            EnemyRegistry.Unregister(this);
        }
    }
}
