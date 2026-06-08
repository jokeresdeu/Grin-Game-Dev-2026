using TMPro;
using UnityEngine;

namespace ClassicPlatformer
{
    public class MyGameManager : MonoBehaviour
    {
        public static MyGameManager Instance;

        [SerializeField] private TextMeshProUGUI _coinsText;

        private int _coins;

        private void Awake()
        {
            Instance = this;
            UpdateUI();
        }

        public void AddCoins(int amount)
        {
            _coins += amount;
            UpdateUI();
        }

        private void UpdateUI()
        {
            if (_coinsText != null)
                _coinsText.text = "Coins: " + _coins;
        }
    }
}