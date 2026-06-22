using UnityEngine;

namespace Projects.TowerDefense.Scripts
{
    /// <summary>
    /// Static source of all level/wave/path data. The Editor scene builder reads this to
    /// lay out paths and slots; the runtime reads it for wave composition and scaling.
    /// All coordinates assume an orthographic camera of size 5.5, base at (8, 0).
    /// </summary>
    public static class LevelLibrary
    {
        public static readonly Vector3 BasePosition = new Vector3(8f, 0f, 0f);

        public static LevelConfig[] Build()
        {
            return new[] { Level1(), Level2(), Level3() };
        }

        private static LevelConfig Level1()
        {
            return new LevelConfig
            {
                Index = 0,
                HpMult = 1.0f,
                BountyMult = 1.0f,
                SpeedMult = 1.0f,
                Waypoints = new[]
                {
                    new Vector3(-10f, 3f, 0f),
                    new Vector3(1.5f, 3f, 0f),
                    new Vector3(1.5f, -3f, 0f),
                    new Vector3(8f, -3f, 0f),
                    new Vector3(8f, 0f, 0f)
                },
                SlotPositions = new[]
                {
                    new Vector3(-6f, 1.3f, 0f),
                    new Vector3(-1.5f, 1.3f, 0f),
                    new Vector3(-3.5f, 4.4f, 0f),
                    new Vector3(3.4f, 0.5f, 0f),
                    new Vector3(3.4f, -1.4f, 0f),
                    new Vector3(5.6f, -1.4f, 0f)
                },
                Waves = new[]
                {
                    new WaveConfig(0.70f, new WaveSpawn(EnemyType.Grunt, 8)),
                    new WaveConfig(0.65f, new WaveSpawn(EnemyType.Grunt, 10), new WaveSpawn(EnemyType.Runner, 4)),
                    new WaveConfig(0.60f, new WaveSpawn(EnemyType.Grunt, 6), new WaveSpawn(EnemyType.Runner, 6), new WaveSpawn(EnemyType.Tank, 2))
                }
            };
        }

        private static LevelConfig Level2()
        {
            return new LevelConfig
            {
                Index = 1,
                HpMult = 1.6f,
                BountyMult = 1.2f,
                SpeedMult = 1.05f,
                Waypoints = new[]
                {
                    new Vector3(-10f, -3f, 0f),
                    new Vector3(-6f, -3f, 0f),
                    new Vector3(-6f, 3f, 0f),
                    new Vector3(-1f, 3f, 0f),
                    new Vector3(-1f, -3f, 0f),
                    new Vector3(4f, -3f, 0f),
                    new Vector3(4f, 3f, 0f),
                    new Vector3(8f, 3f, 0f),
                    new Vector3(8f, 0f, 0f)
                },
                SlotPositions = new[]
                {
                    new Vector3(-7.9f, -1.3f, 0f),
                    new Vector3(-4.2f, -0.8f, 0f),
                    new Vector3(-4.2f, 1.2f, 0f),
                    new Vector3(0.6f, 0f, 0f),
                    new Vector3(2.5f, -1.2f, 0f),
                    new Vector3(2.5f, 1.2f, 0f),
                    new Vector3(6f, 1.4f, 0f)
                },
                Waves = new[]
                {
                    new WaveConfig(0.65f, new WaveSpawn(EnemyType.Grunt, 12), new WaveSpawn(EnemyType.Runner, 6)),
                    new WaveConfig(0.55f, new WaveSpawn(EnemyType.Grunt, 8), new WaveSpawn(EnemyType.Runner, 10), new WaveSpawn(EnemyType.Tank, 3)),
                    new WaveConfig(0.55f, new WaveSpawn(EnemyType.Grunt, 10), new WaveSpawn(EnemyType.Runner, 8), new WaveSpawn(EnemyType.Tank, 5))
                }
            };
        }

        private static LevelConfig Level3()
        {
            return new LevelConfig
            {
                Index = 2,
                HpMult = 2.4f,
                BountyMult = 1.4f,
                SpeedMult = 1.1f,
                Waypoints = new[]
                {
                    new Vector3(-10f, 4f, 0f),
                    new Vector3(-6f, 4f, 0f),
                    new Vector3(-6f, -1f, 0f),
                    new Vector3(-2f, -1f, 0f),
                    new Vector3(-2f, 4f, 0f),
                    new Vector3(2f, 4f, 0f),
                    new Vector3(2f, -4f, 0f),
                    new Vector3(6f, -4f, 0f),
                    new Vector3(6f, 2f, 0f),
                    new Vector3(8f, 2f, 0f),
                    new Vector3(8f, 0f, 0f)
                },
                SlotPositions = new[]
                {
                    new Vector3(-8f, 2.3f, 0f),
                    new Vector3(-4.2f, 1.4f, 0f),
                    new Vector3(-4.2f, -2.6f, 0f),
                    new Vector3(-0.2f, 1.5f, 0f),
                    new Vector3(0.3f, -1.6f, 0f),
                    new Vector3(3.9f, -1.6f, 0f),
                    new Vector3(4.2f, 0.3f, 0f),
                    new Vector3(7f, -1f, 0f)
                },
                Waves = new[]
                {
                    new WaveConfig(0.60f, new WaveSpawn(EnemyType.Grunt, 14), new WaveSpawn(EnemyType.Runner, 10), new WaveSpawn(EnemyType.Tank, 4)),
                    new WaveConfig(0.55f, new WaveSpawn(EnemyType.Grunt, 10), new WaveSpawn(EnemyType.Runner, 12), new WaveSpawn(EnemyType.Tank, 8)),
                    new WaveConfig(0.50f, new WaveSpawn(EnemyType.Grunt, 16), new WaveSpawn(EnemyType.Runner, 12), new WaveSpawn(EnemyType.Tank, 8))
                }
            };
        }
    }
}
