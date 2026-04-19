using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ClassicPlatformer
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Doors : BaseInteractable
    {
        [SerializeField] private Sprite _openDoors;
        [SerializeField] private GameObject WinWindow;
        private SpriteRenderer _spriteRenderer;

        private bool _isOpen;
        
        private void Awake()
        { 
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
        
        protected override void OnTriggerEnter2D(Collider2D other)
        {
            if(!_isOpen)
                return;
            
            base.OnTriggerEnter2D(other);
        }
        private void Start()
        {
            if (WinWindow!=null)
            {
                WinWindow.SetActive(false);
            }            
        }
        public void Open()
        {
            _spriteRenderer.sprite = _openDoors;
            _isOpen = true;
        }

        public override void Interact(Player player)
        {
            Time.timeScale = 0f;
            WinWindow.SetActive(true);
            //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}