using System;
using UnityEngine;

namespace ChickenHunt
{
    public class Chest : MonoBehaviour, IShootable
    {
        [Header("Налаштування бонусу")]
        [SerializeField] private int _pointsReward = 50; 

        private Animator _animator;
        private bool _isOpen = false;

        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        public void OnShoot()
        {
            if (_isOpen) return;
            _isOpen = true;

            Debug.Log("You opened the chest!");

            ChickensManager manager = FindObjectOfType<ChickensManager>();
            if (manager != null)
            {
                manager.AddScore(_pointsReward);
            }

            if (_animator != null)
            {
                _animator.SetTrigger("Open");
            }

            Collider2D col = GetComponent<Collider2D>();
            if (col != null) col.enabled = false;

            Destroy(gameObject, 1.0f);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (_isOpen) return;

            if (collision.CompareTag("Player"))
            {
                _isOpen = true;
                
                ChickensManager manager = FindObjectOfType<ChickensManager>();
                if (manager != null)
                {
                    manager.AddScore(_pointsReward); 
                }
                
                if (_animator != null)
                {
                    _animator.SetTrigger("Open");
                }

                Collider2D col = GetComponent<Collider2D>();
                if (col != null) col.enabled = false;

                Destroy(gameObject, 1.0f); 
            }
        }
    }
}