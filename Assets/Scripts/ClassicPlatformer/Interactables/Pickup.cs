using System.Collections;
using UnityEngine;

namespace ClassicPlatformer
{
    public enum PickupType
    {
        Coin,
        Health,
        Chest
    }

    public class Pickup : BaseInteractable
    {
        [Header("Type")]
        [SerializeField] private PickupType _type = PickupType.Coin;

        [Header("Value")]
        [SerializeField] private int _value = 1;

        [Header("Visual")]
        [SerializeField] private GameObject _collectEffect;

        [Header("Chest")]
        [SerializeField] private Sprite _closedSprite;
        [SerializeField] private Sprite _openSprite;
        [SerializeField] private float _destroyDelay = 1.5f;

        private SpriteRenderer _spriteRenderer;
        private bool _opened = false;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public override void Interact(Player player)
        {
            if (_opened) return;

            switch (_type)
            {
                case PickupType.Coin:
                    GameManager.Instance.AddCoins(_value);
                    Break();
                    break;

                case PickupType.Health:
                    player.Heal(_value);
                    Break();
                    break;

                case PickupType.Chest:
                    OpenChest(player);
                    break;
            }
        }

        private void Break()
        {
            Debug.Log("Pickup");
            if (_collectEffect != null)
                Instantiate(_collectEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        private void OpenChest(Player player)
        {
            _opened = true;

            // Міняємо спрайт на відкритий
            if (_spriteRenderer != null && _openSprite != null)
                _spriteRenderer.sprite = _openSprite;

            // Видаємо нагороду
            GameManager.Instance.AddCoins(_value);

            if (_collectEffect != null)
                Instantiate(_collectEffect, transform.position, Quaternion.identity);

            Debug.Log("Chest opened!");

            // Знищуємо через затримку
            StartCoroutine(DestroyAfterDelay());
        }

        private IEnumerator DestroyAfterDelay()
        {
            yield return new WaitForSeconds(_destroyDelay);
            Destroy(gameObject);
        }
    }
}