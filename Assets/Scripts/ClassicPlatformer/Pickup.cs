using UnityEngine;

namespace ClassicPlatformer
{
    public enum PickupType
    {
        Coin,
        Health
    }

    public class Pickup : MonoBehaviour
    {
        [Header("Type")]
        [SerializeField] private PickupType _type = PickupType.Coin;

        [Header("Value")]
        [SerializeField] private int _value = 1;

        [Header("Visual")]
        [SerializeField] private GameObject _collectEffect;

        private void OnTriggerEnter2D(Collider2D other)
        {
            var player = other.GetComponent<Player>();
            if (player == null) return;

            switch (_type)
            {
                case PickupType.Coin:
                    GameManager.Instance.AddCoins(_value);
                    break;
                case PickupType.Health:
                    var health = other.GetComponent<Health>();
                    if (health != null && health.CurrentHealth < health.MaxHealth)
                        health.Heal(_value);
                    else
                        return;
                    break;
            }

            if (_collectEffect != null)
                Instantiate(_collectEffect, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }
}
