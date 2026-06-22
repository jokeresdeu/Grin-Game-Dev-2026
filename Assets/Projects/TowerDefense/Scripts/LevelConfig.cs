using UnityEngine;

namespace Projects.TowerDefense.Scripts
{
    /// <summary>
    /// One level: its path (waypoint polyline, last point = the base), the buildable
    /// slot positions, its 3 waves, and the per-level enemy stat multipliers.
    /// </summary>
    public class LevelConfig
    {
        public int Index;
        public Vector3[] Waypoints;
        public Vector3[] SlotPositions;
        public WaveConfig[] Waves;
        public float HpMult;
        public float BountyMult;
        public float SpeedMult;
    }
}
