using System.Collections.Generic;

namespace Projects.TowerDefense.Scripts
{
    public struct WaveSpawn
    {
        public EnemyType Type;
        public int Count;

        public WaveSpawn(EnemyType type, int count)
        {
            Type = type;
            Count = count;
        }
    }

    /// <summary>
    /// One wave: how many of each enemy type, and the delay between spawns.
    /// <see cref="BuildSpawnOrder"/> expands the counts into a round-robin interleaved
    /// list so the wave is paced (not all of one type, then all of another).
    /// </summary>
    public class WaveConfig
    {
        public WaveSpawn[] Spawns;
        public float SpawnInterval;

        public WaveConfig(float spawnInterval, params WaveSpawn[] spawns)
        {
            SpawnInterval = spawnInterval;
            Spawns = spawns;
        }

        public int TotalCount
        {
            get
            {
                int total = 0;
                foreach (WaveSpawn s in Spawns)
                    total += s.Count;
                return total;
            }
        }

        public List<EnemyType> BuildSpawnOrder()
        {
            var remaining = new int[Spawns.Length];
            for (int i = 0; i < Spawns.Length; i++)
                remaining[i] = Spawns[i].Count;

            var order = new List<EnemyType>();
            bool any = true;
            while (any)
            {
                any = false;
                for (int i = 0; i < Spawns.Length; i++)
                {
                    if (remaining[i] <= 0)
                        continue;
                    order.Add(Spawns[i].Type);
                    remaining[i]--;
                    any = true;
                }
            }
            return order;
        }
    }
}
