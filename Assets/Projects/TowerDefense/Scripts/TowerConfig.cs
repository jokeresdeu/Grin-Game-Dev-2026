using UnityEngine;

namespace Projects.TowerDefense.Scripts
{
    public enum TowerRole
    {
        Rapid,
        Heavy,
        Sniper
    }

    /// <summary>
    /// Fully-resolved stats for one tower (a race + role combination), across its 3 upgrade levels.
    /// Build it with <see cref="Resolve"/>, which applies the race's multipliers to the shared
    /// role base stats. All per-level arrays are length 3 (level 1, 2, 3).
    /// </summary>
    public class TowerConfig
    {
        public const int MaxLevel = 3;

        public TowerRole Role;
        public RaceId Race;
        public string DisplayName;
        public Color Color;
        public float Diameter;

        public int[] Damage;        // length 3
        public float[] FireInterval; // length 3
        public float[] Range;        // length 3
        public int BuildCost;
        public int[] UpgradeCost;    // length 2: [cost to L2, cost to L3]

        public static readonly TowerRole[] Roles = { TowerRole.Rapid, TowerRole.Heavy, TowerRole.Sniper };

        public int DamageAt(int level) => Damage[Mathf.Clamp(level - 1, 0, MaxLevel - 1)];
        public float IntervalAt(int level) => FireInterval[Mathf.Clamp(level - 1, 0, MaxLevel - 1)];
        public float RangeAt(int level) => Range[Mathf.Clamp(level - 1, 0, MaxLevel - 1)];
        public bool CanUpgrade(int level) => level < MaxLevel;
        public int UpgradeCostTo(int nextLevel) => UpgradeCost[Mathf.Clamp(nextLevel - 2, 0, UpgradeCost.Length - 1)];

        public static TowerConfig Resolve(RaceId race, TowerRole role)
        {
            RaceConfig rc = RaceConfig.For(race);

            // Shared base role stats (Humans baseline).
            int[] baseDamage;
            float[] baseInterval;
            float[] baseRange;
            int baseBuild;
            int[] baseUpgrade;
            string roleName;
            float diameter;
            float brightness;

            switch (role)
            {
                case TowerRole.Heavy:
                    baseDamage = new[] { 9, 16, 28 };
                    baseInterval = new[] { 1.40f, 1.25f, 1.10f };
                    baseRange = new[] { 2.8f, 3.0f, 3.3f };
                    baseBuild = 80;
                    baseUpgrade = new[] { 60, 110 };
                    roleName = "Важка";
                    diameter = 0.64f;
                    brightness = 0.80f;
                    break;
                case TowerRole.Sniper:
                    baseDamage = new[] { 6, 11, 18 };
                    baseInterval = new[] { 1.00f, 0.90f, 0.78f };
                    baseRange = new[] { 4.6f, 5.0f, 5.6f };
                    baseBuild = 60;
                    baseUpgrade = new[] { 45, 85 };
                    roleName = "Дальня";
                    diameter = 0.52f;
                    brightness = 1.00f;
                    break;
                default: // Rapid
                    baseDamage = new[] { 2, 4, 7 };
                    baseInterval = new[] { 0.50f, 0.42f, 0.35f };
                    baseRange = new[] { 2.6f, 2.8f, 3.1f };
                    baseBuild = 40;
                    baseUpgrade = new[] { 30, 55 };
                    roleName = "Швидка";
                    diameter = 0.45f;
                    brightness = 1.15f;
                    break;
            }

            var cfg = new TowerConfig
            {
                Role = role,
                Race = race,
                DisplayName = roleName,
                Diameter = diameter,
                Damage = new int[3],
                FireInterval = new float[3],
                Range = new float[3],
                UpgradeCost = new int[2]
            };

            for (int i = 0; i < 3; i++)
            {
                cfg.Damage[i] = Mathf.Max(1, Mathf.RoundToInt(baseDamage[i] * rc.DamageMult));
                cfg.FireInterval[i] = baseInterval[i] * rc.IntervalMult;
                cfg.Range[i] = baseRange[i];
            }

            cfg.BuildCost = Mathf.RoundToInt(baseBuild * rc.CostMult);
            cfg.UpgradeCost[0] = Mathf.RoundToInt(baseUpgrade[0] * rc.CostMult);
            cfg.UpgradeCost[1] = Mathf.RoundToInt(baseUpgrade[1] * rc.CostMult);

            Color c = rc.Color * brightness;
            c.a = 1f;
            cfg.Color = c;

            return cfg;
        }
    }
}
