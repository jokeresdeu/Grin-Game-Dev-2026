using UnityEngine;

namespace ClassicPlatformer
{
    public class KeyCollector : MonoBehaviour
    {
        [Header("Keys")]
        [SerializeField] private int _currentKeys = 0;

        [Header("UI")]
        [SerializeField] private KeysUI _keysUI;

        [Header("Door Reference")]
        [SerializeField] private Doors _doors;

        public int CurrentKeys => _currentKeys;

        private void Start()
        {
            UpdateKeysUI();
        }

        public void AddKey(int amount = 1)
        {
            _currentKeys += amount;
            Debug.Log($"Keys collected: {_currentKeys}");

            UpdateKeysUI();
        }

        public bool HasEnoughKeys(int requiredKeys)
        {
            return _currentKeys >= requiredKeys;
        }

        public void ResetKeys()
        {
            _currentKeys = 0;
            UpdateKeysUI();
        }

        private void UpdateKeysUI()
        {
            if (_keysUI == null || _doors == null) return;

            _keysUI.UpdateKeys(_currentKeys, _doors.RequiredKeys);
        }
    }
}