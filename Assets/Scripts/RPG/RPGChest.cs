using UnityEngine;
using System.Collections;

namespace RPG
{
    public class RPGChest : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private int _coinCount = 15;
        [SerializeField] private GameObject _coinPrefab;

        private Animator _animator;
        private bool _isOpened = false;

        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        public void TakeDamage(float amount)
        {
            if (_isOpened) return;
            OpenChest();
        }

        private void OpenChest()
        {
            _isOpened = true;
            
            if (RPGGameManager.Instance != null)
            {
                RPGGameManager.Instance.StartVictorySequence();
            }

            if (_animator != null)
            {
                _animator.SetInteger("State", 9);
            }

            StartCoroutine(SpawnCoinsCoroutine());
        }

        private IEnumerator SpawnCoinsCoroutine()
        {
            if (_coinPrefab != null)
            {
                for (int i = 0; i < _coinCount; i++)
                {

                    Vector2 randomDir = Random.insideUnitCircle.normalized;
                    if (randomDir.y < 0) randomDir.y = -randomDir.y;
                    
                    Vector3 spawnPos = transform.position + new Vector3(randomDir.x, randomDir.y, 0f) * 0.5f;
                    
                    var coin = Instantiate(_coinPrefab, spawnPos, Quaternion.identity);
                    var moveLeft = coin.GetComponent<RPG.MoveLeft>();
                    if (moveLeft != null)
                    {
                        Destroy(moveLeft);
                    }

                    var rb = coin.GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        rb.AddForce(randomDir * Random.Range(3f, 6f), ForceMode2D.Impulse);
                        rb.AddTorque(Random.Range(-100f, 100f));
                    }

                    yield return new WaitForSeconds(0.02f);
                }
            }


            yield return new WaitForSeconds(1.5f);

            if (RPGGameManager.Instance != null)
            {
                RPGGameManager.Instance.GameWon();
            }
        }
    }
}

