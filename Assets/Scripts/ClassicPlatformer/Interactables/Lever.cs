using UnityEngine;

namespace ClassicPlatformer
{
    public class Lever : BaseInteractable
    {
        [SerializeField] private Doors _doors;

        [Header("Sprites")]
        [SerializeField] private Sprite _spriteOff; 
        [SerializeField] private Sprite _spriteOn;  

        private SpriteRenderer _spriteRenderer;
        private bool _activated = false;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();

            if (_spriteRenderer != null && _spriteOff != null)
                _spriteRenderer.sprite = _spriteOff;
        }

        public override void Interact(Player player)
        {
            if (_activated) return;

            _activated = true;

            if (_spriteRenderer != null && _spriteOn != null)
                _spriteRenderer.sprite = _spriteOn;

            _doors.Open();
            Debug.Log("Lever activated");
        }
    }
}