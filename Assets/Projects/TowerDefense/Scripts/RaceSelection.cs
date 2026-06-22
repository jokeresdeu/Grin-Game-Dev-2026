using UnityEngine;

namespace Projects.TowerDefense.Scripts
{
    /// <summary>
    /// Static holder for the race chosen on the main menu, read by the game scene.
    /// Backed by PlayerPrefs so it survives the scene load.
    /// </summary>
    public static class RaceSelection
    {
        private const string Key = "TowerDefense.Race";

        private static RaceId _selected = RaceId.Humans;
        private static bool _loaded;

        public static RaceId Selected
        {
            get
            {
                if (!_loaded)
                {
                    _selected = (RaceId)PlayerPrefs.GetInt(Key, 0);
                    _loaded = true;
                }
                return _selected;
            }
            set
            {
                _selected = value;
                _loaded = true;
                PlayerPrefs.SetInt(Key, (int)value);
                PlayerPrefs.Save();
            }
        }
    }
}
