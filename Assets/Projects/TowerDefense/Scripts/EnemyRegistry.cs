using System.Collections.Generic;

namespace Projects.TowerDefense.Scripts
{
    /// <summary>
    /// Static list of live enemies. Towers query it for targets; LevelManager uses the
    /// count to detect when a wave is cleared. Cleared in GameManager.Awake on (re)load.
    /// </summary>
    public static class EnemyRegistry
    {
        private static readonly List<Enemy> _enemies = new List<Enemy>();

        public static IReadOnlyList<Enemy> Enemies => _enemies;
        public static int Count => _enemies.Count;

        public static void Register(Enemy enemy)
        {
            if (enemy != null && !_enemies.Contains(enemy))
                _enemies.Add(enemy);
        }

        public static void Unregister(Enemy enemy)
        {
            _enemies.Remove(enemy);
        }

        public static void Clear()
        {
            _enemies.Clear();
        }
    }
}
