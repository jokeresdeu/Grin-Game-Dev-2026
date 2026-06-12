using System;
using UnityEngine;

namespace ChickenHunt
{
    public class Chest : MonoBehaviour, IShootable
    {
        [Header("Налаштування бонусу")]
        [SerializeField] private int _pointsReward = 50; 

        public void OnShoot()
        {
            Debug.LogError("You shot chest");
            Destroy(gameObject);
        }

        // Взаємодія через Тригер
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
          
                ChickensManager manager = FindObjectOfType<ChickensManager>();
                if (manager != null)
                {
                    manager.AddScore(_pointsReward); 
                }
                
                Destroy(gameObject); 
            }
        }
    }
}