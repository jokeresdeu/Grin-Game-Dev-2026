using UnityEngine;
using UnityEngine.SceneManagement;

namespace ClassicPlatformer
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Collider2D))]
    public class Doors : MonoBehaviour
    {
        [Header("Door Visual")]
        [SerializeField] private Sprite _closedDoors;
        [SerializeField] private Sprite _openDoors;
        [SerializeField] private Animator _animator;
        [SerializeField] private string _openTriggerName = "Open";

        [Header("Door Settings")]
        [SerializeField] private int _requiredKeys = 3;
        [SerializeField] private bool _openAutomaticallyWhenEnoughKeys = true;
        [SerializeField] private bool _isEnd = false;
        [SerializeField] private int _nextSceneIndex = 2;

        [Header("UI / Win")]
        [SerializeField] private GameObject WinWindow;

        private SpriteRenderer _spriteRenderer;
        private Collider2D _collider;
        private bool _isOpen;

        public bool IsOpen => _isOpen;
        public int RequiredKeys => _requiredKeys;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _collider = GetComponent<Collider2D>();

            if (_collider != null)
                _collider.isTrigger = true;
        }

        private void Start()
        {
            if (WinWindow != null)
                WinWindow.SetActive(false);

            if (_spriteRenderer != null && _closedDoors != null)
                _spriteRenderer.sprite = _closedDoors;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Player player = other.GetComponent<Player>();
            if (player == null) return;

            KeyCollector keyCollector = player.GetComponent<KeyCollector>();
            if (keyCollector == null)
            {
                Debug.LogWarning("[DOOR] Player has no KeyCollector!");
                return;
            }

            Debug.Log($"[DOOR] Player entered. Keys = {keyCollector.CurrentKeys}/{_requiredKeys}");

            // Автовідкриття
            if (!_isOpen && _openAutomaticallyWhenEnoughKeys)
            {
                if (keyCollector.HasEnoughKeys(_requiredKeys))
                {
                    Debug.Log("[DOOR] Enough keys -> opening automatically");
                    Open();
                }
                else
                {
                    Debug.Log("[DOOR] Not enough keys yet");
                }
            }

            // Якщо двері вже відкриті — можна проходити
            if (_isOpen)
            {
                GoThroughDoor();
            }
        }

        public void TryOpen(Player player)
        {
            if (player == null) return;

            KeyCollector keyCollector = player.GetComponent<KeyCollector>();
            if (keyCollector == null)
            {
                Debug.LogWarning("[DOOR] Player has no KeyCollector!");
                return;
            }

            Debug.Log($"[DOOR] TryOpen called. Keys = {keyCollector.CurrentKeys}/{_requiredKeys}");

            if (keyCollector.HasEnoughKeys(_requiredKeys))
            {
                Open();
            }
            else
            {
                Debug.Log($"[DOOR] Not enough keys! Need {_requiredKeys}");
            }
        }

        public void Open()
        {
            if (_isOpen) return;

            _isOpen = true;
            Debug.Log("[DOOR] Doors opened!");

            if (_animator != null)
            {
                _animator.SetTrigger(_openTriggerName);
            }
            else if (_spriteRenderer != null && _openDoors != null)
            {
                _spriteRenderer.sprite = _openDoors;
            }
        }

        public void GoThroughDoor()
        {
            Debug.Log("[DOOR] Going through door");

            if (_isEnd)
            {
                Time.timeScale = 0f;

                if (WinWindow != null)
                    WinWindow.SetActive(true);

                return;
            }

            Time.timeScale = 1f;
            SceneManager.LoadScene(_nextSceneIndex);
        }
    }
}