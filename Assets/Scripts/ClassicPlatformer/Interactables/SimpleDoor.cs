using UnityEngine;
using UnityEngine.SceneManagement;

namespace ClassicPlatformer
{
    [RequireComponent(typeof(SpriteRenderer), typeof(Collider2D))]
    public class SimpleDoor : MonoBehaviour
    {
        [Header("Door Visual")]
        [SerializeField] private Sprite _openSprite;

        [Header("Scene Settings")]
        [SerializeField] private string _nextSceneName = "Level2"; // Назва сцени для переходу

        private SpriteRenderer _spriteRenderer;
        private bool _isOpen = false;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        /// <summary>
        /// Відкриває двері, змінюючи спрайт
        /// </summary>
        public void Open()
        {
            if (_isOpen) return;

            _isOpen = true;

            if (_spriteRenderer != null && _openSprite != null)
                _spriteRenderer.sprite = _openSprite;

            Debug.Log("Door is now open!");
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!_isOpen) return;

            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                Debug.Log("Player entered open door. Loading next scene...");
                SceneManager.LoadScene(_nextSceneName);
            }
        }

        public bool IsOpen => _isOpen;
    }
}