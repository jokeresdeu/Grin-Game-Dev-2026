using TMPro;
using UnityEngine;

namespace ClassicPlatformer
{
    public class KeysUI : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private TMP_Text _keysText;

        [Header("Settings")]
        [SerializeField] private string _prefix = "Keys";

        public void UpdateKeys(int currentKeys, int requiredKeys)
        {
            if (_keysText == null) return;

            _keysText.text = $"{_prefix}: {currentKeys} / {requiredKeys}";
        }
    }
}