using UnityEngine;

namespace ClassicPlatformer
{
    // Колайдер: IsTrigger = true
    public class CoinCollectible : MonoBehaviour
    {
        [SerializeField] private int _value = 1;
        [SerializeField] private GameObject _collectEffect;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.TryGetComponent(out PlayerHealth _)) return;

            GameManager.Instance.AddScore(_value);

            if (_collectEffect != null)
                Instantiate(_collectEffect, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }
}
