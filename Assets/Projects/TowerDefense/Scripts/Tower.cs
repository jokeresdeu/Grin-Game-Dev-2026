using UnityEngine;

namespace Projects.TowerDefense.Scripts
{
    /// <summary>
    /// A placed tower. Hitscan combat: every fire interval it picks the in-range enemy
    /// furthest along the path, deals instant damage, and draws a tracer. Holds its level
    /// (1-3) and total gold invested (for sell refunds). Sized from sprite bounds.
    /// </summary>
    public class Tower : MonoBehaviour
    {
        private SpriteRenderer _renderer;
        private TowerConfig _config;
        private int _level = 1;
        private int _invested;
        private float _cooldown;

        public int Level => _level;
        public TowerConfig Config => _config;
        public int Invested => _invested;
        public bool CanUpgrade => _config != null && _config.CanUpgrade(_level);
        public int UpgradeCost => _config != null ? _config.UpgradeCostTo(_level + 1) : 0;
        public int SellValue => Mathf.RoundToInt(_invested * 0.5f);

        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
        }

        public void Setup(TowerConfig config, int buildCost)
        {
            _config = config;
            _level = 1;
            _invested = buildCost;
            _cooldown = 0f;
            ApplyVisual();
        }

        public void Upgrade(int cost)
        {
            if (!CanUpgrade)
                return;

            _level++;
            _invested += cost;
            ApplyVisual();
        }

        private void ApplyVisual()
        {
            if (_renderer == null)
                _renderer = GetComponent<SpriteRenderer>();
            if (_renderer == null || _config == null)
                return;

            float lift = 1f + (_level - 1) * 0.12f;
            Color c = _config.Color * lift;
            c.a = 1f;
            _renderer.color = c;

            float diameter = _config.Diameter * (1f + (_level - 1) * 0.10f);
            Sprite sprite = _renderer.sprite;
            float native = sprite != null ? sprite.bounds.size.x : 1f;
            float s = native > 0.0001f ? diameter / native : diameter;
            transform.localScale = new Vector3(s, s, 1f);
        }

        private void Update()
        {
            if (GameManager.Instance != null && GameManager.Instance.State != GameState.Playing)
                return;
            if (_config == null)
                return;

            _cooldown -= Time.deltaTime;
            if (_cooldown > 0f)
                return;

            Enemy target = FindTarget();
            if (target == null)
                return;

            target.TakeDamage(_config.DamageAt(_level));
            ShotEffect.Spawn(transform.position, target.Position, _config.Color);
            _cooldown = _config.IntervalAt(_level);
        }

        private Enemy FindTarget()
        {
            float range = _config.RangeAt(_level);
            Enemy best = null;
            float bestProgress = float.NegativeInfinity;

            var enemies = EnemyRegistry.Enemies;
            for (int i = 0; i < enemies.Count; i++)
            {
                Enemy e = enemies[i];
                if (e == null || e.IsDead)
                    continue;

                float d = (e.Position - transform.position).magnitude;
                if (d <= range + e.Radius && e.Progress > bestProgress)
                {
                    best = e;
                    bestProgress = e.Progress;
                }
            }
            return best;
        }
    }
}
