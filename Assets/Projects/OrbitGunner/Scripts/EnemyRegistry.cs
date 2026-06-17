using System.Collections.Generic;

namespace Projects.OrbitGunner.Scripts
{

    public static class EnemyRegistry
    {
        private static readonly List<Enemy> _active = new List<Enemy>();

        public static IReadOnlyList<Enemy> Active => _active;
        public static int Count => _active.Count;

        public static void Register(Enemy enemy)
        {
            if (enemy != null && !_active.Contains(enemy))
                _active.Add(enemy);
        }

        public static void Unregister(Enemy enemy)
        {
            _active.Remove(enemy);
        }

        public static Enemy[] Snapshot()
        {
            return _active.ToArray();
        }

        public static void Clear()
        {
            _active.Clear();
        }
    }
}
