using UnityEngine;

namespace RPG
{
    public class RPGEnemy : MonoBehaviour
    {
        [Header("Stats")]
        [SerializeField] private float _maxHealth = 50f;
        [SerializeField] private float _moveSpeed = 3f;
        [SerializeField] private float _attackDamage = 20f;
        [SerializeField] private float _attackRange = 1.5f;
        [SerializeField] private float _attackCooldown = 2f;

        [Header("Loot")]
        public GameObject coinPrefab;

        [Header("UI")]
        public Transform healthFillTransform;
        private Vector3 _originalHealthFillScale;

        private float _currentHealth;
        private float _attackTimer;
        private Player _targetPlayer;
        private bool _isDead = false;

        private Animator _animator;


        private bool _isLunging = false;
        private float _lungeTimer = 0f;
        private Vector3 _lungeStartPos;

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
            
            if (_animator != null && _animator.gameObject.GetComponent<AnimationEventReceiver>() == null)
            {
                _animator.gameObject.AddComponent<AnimationEventReceiver>();
            }

            _currentHealth = _maxHealth;
        }

        private void Start()
        {
            if (healthFillTransform != null)
            {
                _originalHealthFillScale = healthFillTransform.localScale;
                

                if (healthFillTransform.parent != null)
                {
                    var renderers = healthFillTransform.parent.GetComponentsInChildren<SpriteRenderer>();
                    if (renderers.Length > 0)
                    {
                        Texture2D tex = new Texture2D(1, 1);
                        tex.SetPixel(0, 0, Color.white);
                        tex.Apply();
                        Sprite square = Sprite.Create(tex, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f), 1f);
                        foreach (var sr in renderers)
                        {
                            sr.sprite = square;
                        }
                    }
                }
            }
            var playerObj = GameObject.Find("Human");
            if (playerObj != null)
            {
                _targetPlayer = playerObj.GetComponent<Player>();
            }
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (_isDead) return;

            if (_isLunging)
            {
                HandleLungeAnimation();
                return;
            }

            if (_targetPlayer != null)
            {
                float distance = Vector2.Distance(transform.position, _targetPlayer.transform.position);
                
                if (distance <= _attackRange)
                {
                    if (_animator != null) _animator.SetInteger("State", 0);

                    _attackTimer -= Time.deltaTime;
                    if (_attackTimer <= 0f)
                    {
                        PerformAttack();
                        _attackTimer = _attackCooldown;
                    }
                }
                else
                {
                    MoveTowardsPlayer();
                }
            }
        }

        private void MoveTowardsPlayer()
        {
            Vector3 direction = (_targetPlayer.transform.position - transform.position).normalized;

            Vector3 separation = Vector3.zero;
            var allEnemies = FindObjectsByType<RPGEnemy>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
            foreach (var other in allEnemies)
            {
                if (other != this && !other._isDead)
                {
                    float dist = Vector3.Distance(transform.position, other.transform.position);
                    if (dist < 1.5f && dist > 0.01f)
                    {
                        Vector3 pushDir = (transform.position - other.transform.position).normalized;
                        pushDir.x *= 0.5f;
                        pushDir.y *= 2.0f;
                        separation += pushDir * (1.5f - dist);
                    }
                }
            }

            direction = (direction + separation * 2f).normalized;

            Vector3 newPos = transform.position + direction * _moveSpeed * Time.deltaTime;
            newPos.y = Mathf.Clamp(newPos.y, -4.5f, -2.5f);
            transform.position = newPos;

            float faceDirection = _targetPlayer.transform.position.x - transform.position.x;
            if (faceDirection > 0)
                transform.rotation = Quaternion.Euler(0, 180, 0);
            else if (faceDirection < 0)
                transform.rotation = Quaternion.Euler(0, 0, 0);
                
            if (_animator != null)
            {
                _animator.SetInteger("State", 2);
            }
        }

        private void PerformAttack()
        {
            if (_animator != null)
            {
                _animator.SetTrigger("Attack");
                _animator.SetTrigger("Slash");
                _animator.SetInteger("State", 4);
            }
            else
            {
                _isLunging = true;
                _lungeTimer = 0f;
                _lungeStartPos = transform.position;
            }
            

            if (_targetPlayer != null)
            {
                _targetPlayer.TakeDamage(_attackDamage);
            }
        }

        private void HandleLungeAnimation()
        {
            _lungeTimer += Time.deltaTime;
            float lungeDuration = 0.3f;
            
            if (_lungeTimer <= lungeDuration)
            {
                float halfDuration = lungeDuration / 2f;
                Vector3 targetPos = _targetPlayer.transform.position;
                Vector3 direction = (targetPos - _lungeStartPos).normalized;
                
                if (_lungeTimer < halfDuration)
                {

                    transform.position = Vector3.Lerp(_lungeStartPos, _lungeStartPos + direction * 1f, _lungeTimer / halfDuration);
                }
                else
                {

                    transform.position = Vector3.Lerp(_lungeStartPos + direction * 1f, _lungeStartPos, (_lungeTimer - halfDuration) / halfDuration);
                }
            }
            else
            {
                transform.position = _lungeStartPos;
                _isLunging = false;
            }
        }

        public void TakeDamage(float amount)
        {
            if (_isDead) return;

            _currentHealth -= amount;
            
            if (healthFillTransform != null)
            {
                float pct = _currentHealth / _maxHealth;
                healthFillTransform.localScale = new Vector3(_originalHealthFillScale.x * pct, _originalHealthFillScale.y, _originalHealthFillScale.z);


                float widthLost = _originalHealthFillScale.x * (1f - pct);
                healthFillTransform.localPosition = new Vector3(-widthLost / 2f, healthFillTransform.localPosition.y, healthFillTransform.localPosition.z);
            }

            if (_currentHealth <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            if (_isDead) return;
            _isDead = true;

            if (_animator != null)
            {
                _animator.SetInteger("State", 9);
            }


            if (coinPrefab != null)
            {
                var coin = Instantiate(coinPrefab, transform.position, Quaternion.identity);
            }

            var spawner = FindFirstObjectByType<RPG.ProceduralSpawner>();
            if (spawner != null)
            {
                spawner.EnemyDefeated();
            }

            Destroy(gameObject, 2f);
        }
    }
}

