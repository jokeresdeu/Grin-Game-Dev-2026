using UnityEngine;

namespace Projects.TowerDefense.Scripts
{
    public enum EnemyType
    {
        Grunt,
        Runner,
        Tank
    }

    /// <summary>
    /// Per-type enemy stats (level-1 base values). <see cref="For"/> returns a fresh
    /// instance each call, so the spawner can scale a copy per level without side effects.
    /// </summary>
    public class EnemyConfig
    {
        public EnemyType Type;
        public string DisplayName;
        public Color Color;
        public int MaxHp;
        public float Speed;
        public int Bounty;
        public int LeakDamage;
        public float Radius;

        public static EnemyConfig For(EnemyType type)
        {
            switch (type)
            {
                case EnemyType.Runner:
                    return new EnemyConfig
                    {
                        Type = EnemyType.Runner,
                        DisplayName = "Бігун",
                        Color = new Color(0.40f, 0.90f, 0.45f),
                        MaxHp = 3,
                        Speed = 3.0f,
                        Bounty = 6,
                        LeakDamage = 1,
                        Radius = 0.30f
                    };
                case EnemyType.Tank:
                    return new EnemyConfig
                    {
                        Type = EnemyType.Tank,
                        DisplayName = "Танк",
                        Color = new Color(0.60f, 0.40f, 0.85f),
                        MaxHp = 20,
                        Speed = 1.0f,
                        Bounty = 16,
                        LeakDamage = 2,
                        Radius = 0.46f
                    };
                default:
                    return new EnemyConfig
                    {
                        Type = EnemyType.Grunt,
                        DisplayName = "Грант",
                        Color = new Color(1.0f, 0.50f, 0.25f),
                        MaxHp = 6,
                        Speed = 1.6f,
                        Bounty = 8,
                        LeakDamage = 1,
                        Radius = 0.34f
                    };
            }
        }
    }
}
