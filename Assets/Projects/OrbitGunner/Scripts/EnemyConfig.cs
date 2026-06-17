using UnityEngine;

namespace Projects.OrbitGunner.Scripts
{

    public enum EnemyType
    {
        Grunt,
        Tank,
        Runner
    }

    [System.Serializable]
    public class EnemyConfig
    {
        public EnemyType Type;
        public Color Color;
        public float Radius;
        public int MaxHp;
        public float Speed;
        public int Score;
        public float Overdrive;
        public float SpawnWeight;

        public static EnemyConfig For(EnemyType type)
        {
            switch (type)
            {
                case EnemyType.Tank:
                    return new EnemyConfig
                    {
                        Type = EnemyType.Tank,
                        Color = new Color(0.62f, 0.42f, 0.95f),
                        Radius = 0.84f,
                        MaxHp = 3,
                        Speed = 1.25f,
                        Score = 30,
                        Overdrive = 2f,
                        SpawnWeight = 0.18f
                    };
                case EnemyType.Runner:
                    return new EnemyConfig
                    {
                        Type = EnemyType.Runner,
                        Color = new Color(0.30f, 0.92f, 0.60f),
                        Radius = 0.44f,
                        MaxHp = 1,
                        Speed = 3.6f,
                        Score = 15,
                        Overdrive = 1f,
                        SpawnWeight = 0.24f
                    };
                default:
                    return new EnemyConfig
                    {
                        Type = EnemyType.Grunt,
                        Color = new Color(1f, 0.46f, 0.30f),
                        Radius = 0.58f,
                        MaxHp = 1,
                        Speed = 2.2f,
                        Score = 10,
                        Overdrive = 1f,
                        SpawnWeight = 0.58f
                    };
            }
        }

        public static EnemyType WeightedRandom()
        {
            float grunt = For(EnemyType.Grunt).SpawnWeight;
            float tank = For(EnemyType.Tank).SpawnWeight;
            float runner = For(EnemyType.Runner).SpawnWeight;

            float roll = Random.value * (grunt + tank + runner);

            if (roll < grunt)
                return EnemyType.Grunt;
            if (roll < grunt + tank)
                return EnemyType.Tank;
            return EnemyType.Runner;
        }
    }
}
