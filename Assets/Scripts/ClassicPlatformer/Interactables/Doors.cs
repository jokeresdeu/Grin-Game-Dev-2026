using UnityEngine;
using UnityEngine.SceneManagement;
 
namespace ClassicPlatformer
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Doors : BaseInteractable
    {
        [SerializeField] private Sprite _openDoors;
 
        private SpriteRenderer _spriteRenderer;
        private bool _isOpen;
 
        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
 
        protected override void OnTriggerEnter2D(Collider2D other)
        {
            if (!_isOpen)
                return;
 
            base.OnTriggerEnter2D(other);
        }
 
        public void Open()
        {
            if (_openDoors != null)
                _spriteRenderer.sprite = _openDoors;
            _isOpen = true;
            Debug.Log("Door opened!");
        }
 
        public override void Interact(Player player)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}