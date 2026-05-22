using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ClassicPlatformer
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Doors : BaseInteractable
    {
        [SerializeField] private Sprite _openDoors;
        [SerializeField] private float _openAnimDuration = 1f;

        private SpriteRenderer _spriteRenderer;
        private Animator _animator;
        private bool _isOpen;
        private bool _transitioning;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();
        }

        protected override void OnTriggerEnter2D(Collider2D other)
        {
            if (!_isOpen || _transitioning) return;
            base.OnTriggerEnter2D(other);
        }

        public void Open()
        {
            if (_isOpen) return;
            _isOpen = true;

            if (_openDoors != null)
                _spriteRenderer.sprite = _openDoors;

            _animator?.SetBool("isOpen", true);
        }

        public override void Interact(Player player)
        {
            StartCoroutine(LoadAfterDelay());
        }

        private IEnumerator LoadAfterDelay()
        {
            _transitioning = true;
            yield return new WaitForSeconds(_openAnimDuration);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
