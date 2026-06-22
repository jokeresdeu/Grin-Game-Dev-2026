using System;
using UnityEngine;

namespace Projects.TowerDefense.Scripts
{
    /// <summary>
    /// Singleton gold economy. Gold is reset to <see cref="StartingGold"/> at the start of
    /// each level by <see cref="LevelManager"/>. Mirrors OrbitGunner's ScoreManager role.
    /// </summary>
    public class ResourceManager : MonoBehaviour
    {
        public static ResourceManager Instance { get; private set; }

        public event Action<int> GoldChanged;

        [SerializeField] private int _startingGold = 100;

        public int Gold { get; private set; }
        public int StartingGold => _startingGold;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            Gold = _startingGold;
        }

        private void Start()
        {
            GoldChanged?.Invoke(Gold);
        }

        public void SetGold(int amount)
        {
            Gold = Mathf.Max(0, amount);
            GoldChanged?.Invoke(Gold);
        }

        public void AddGold(int amount)
        {
            Gold = Mathf.Max(0, Gold + amount);
            GoldChanged?.Invoke(Gold);
        }

        public bool CanAfford(int amount) => Gold >= amount;

        public bool TrySpend(int amount)
        {
            if (Gold < amount)
                return false;

            Gold -= amount;
            GoldChanged?.Invoke(Gold);
            return true;
        }

        private void OnDestroy()
        {
            if (Instance == this)
                Instance = null;
        }
    }
}
