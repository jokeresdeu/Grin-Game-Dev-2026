using TMPro;
using UnityEngine;

namespace Projects.MegaSuperChallengeShot.Scripts
{
    public class ScorePopup : MonoBehaviour
    {
        [SerializeField] private TMP_Text _label;
        [SerializeField] private float _lifetime = 0.8f;

        public void Setup(int amount)
        {
            if (_label != null)
                _label.text = $"+{amount}";

            Destroy(gameObject, _lifetime);
        }
    }
}
