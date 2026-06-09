using UnityEngine;
 
namespace ClassicPlatformer
{
    public class Lever : BaseInteractable
    {
        [SerializeField] private Doors _doors;
 
        [Header("Sprites")]
        [SerializeField] private Sprite _leverOff; 
        [SerializeField] private Sprite _leverOn;  
 
        private SpriteRenderer _spriteRenderer;
        private bool _isActivated = false;
 
        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
 
            if (_spriteRenderer != null && _leverOff != null)
                _spriteRenderer.sprite = _leverOff;
        }
 
        public override void Interact(Player player)
        {
            if (_isActivated) return; 
 
            _isActivated = true;

            if (_spriteRenderer != null && _leverOn != null)
                _spriteRenderer.sprite = _leverOn;
 
            if (_doors != null)
                _doors.Open();
 
            Debug.Log("Lever activated!");
        }
    }
}