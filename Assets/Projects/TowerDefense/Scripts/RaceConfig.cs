using UnityEngine;

namespace Projects.TowerDefense.Scripts
{
    public enum RaceId
    {
        Humans,
        Orcs,
        Undead
    }

    /// <summary>
    /// Data for one playable race. Differences are color + stat multipliers only
    /// (applied to the shared tower role base stats in <see cref="TowerConfig"/>).
    /// </summary>
    public class RaceConfig
    {
        public RaceId Id;
        public string DisplayName;
        public string Blurb;
        public Color Color;
        public float DamageMult;
        public float IntervalMult; // >1 = slower fire
        public float CostMult;

        public static readonly RaceId[] All = { RaceId.Humans, RaceId.Orcs, RaceId.Undead };

        public static RaceConfig For(RaceId id)
        {
            switch (id)
            {
                case RaceId.Orcs:
                    return new RaceConfig
                    {
                        Id = RaceId.Orcs,
                        DisplayName = "Орки",
                        Blurb = "Потужні, але повільні й дорогі вежі. Великий урон за постріл.",
                        Color = new Color(0.45f, 0.75f, 0.35f),
                        DamageMult = 1.4f,
                        IntervalMult = 1.3f,
                        CostMult = 1.25f
                    };
                case RaceId.Undead:
                    return new RaceConfig
                    {
                        Id = RaceId.Undead,
                        DisplayName = "Нежить",
                        Blurb = "Дешеві та швидкі вежі, слабші за постріл. Беруть кількістю.",
                        Color = new Color(0.70f, 0.45f, 0.90f),
                        DamageMult = 0.7f,
                        IntervalMult = 0.7f,
                        CostMult = 0.75f
                    };
                default:
                    return new RaceConfig
                    {
                        Id = RaceId.Humans,
                        DisplayName = "Люди",
                        Blurb = "Збалансовані вежі — надійний вибір для будь-якої ситуації.",
                        Color = new Color(0.35f, 0.60f, 1.00f),
                        DamageMult = 1f,
                        IntervalMult = 1f,
                        CostMult = 1f
                    };
            }
        }
    }
}
