using System.Collections;
using UnityEngine;

namespace ClassicPlatformer
{
    // Колайдер: IsTrigger = true
    [RequireComponent(typeof(Animator))]
    public class CoinCollectible : MonoBehaviour
    {
        [SerializeField] private int _value = 1;
        [SerializeField] private float _collectAnimDuration = 0.3f;

        private Animator _animator;
        private bool _collected;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_collected) return;
            if (!other.TryGetComponent(out PlayerHealth _)) return;

            _collected = true;
            GameManager.Instance.AddScore(_value);
            _animator.SetTrigger("Collect");
            StartCoroutine(DestroyAfterAnim());
        }

        private IEnumerator DestroyAfterAnim()
        {
            yield return new WaitForSeconds(_collectAnimDuration);
            Destroy(gameObject);
        }
    }
}
