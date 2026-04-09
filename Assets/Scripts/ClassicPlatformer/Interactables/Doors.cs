using UnityEngine;
using UnityEngine.SceneManagement;

namespace ClassicPlatformer
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Doors : BaseInteractable
    {
        [Header("Sprites")]
        [SerializeField] private Sprite _closedDoors;
        [SerializeField] private Sprite _openDoors;

        [Header("Scene")]
        [SerializeField] private string _nextSceneName = "";

        private SpriteRenderer _spriteRenderer;
        private bool _isOpen;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            Close();
        }

        public void Open()
        {
            _isOpen = true;

            if (_spriteRenderer != null && _openDoors != null)
                _spriteRenderer.sprite = _openDoors;
        }

        public void Close()
        {
            _isOpen = false;

            if (_spriteRenderer != null && _closedDoors != null)
                _spriteRenderer.sprite = _closedDoors;
        }

        public override void Interact(Player player)
        {
            if (!_isOpen)
                return;

            if (!string.IsNullOrWhiteSpace(_nextSceneName))
            {
                SceneManager.LoadScene(_nextSceneName);
                return;
            }

            Debug.Log("Door is open, but next scene is not set. Reloading current scene.");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}