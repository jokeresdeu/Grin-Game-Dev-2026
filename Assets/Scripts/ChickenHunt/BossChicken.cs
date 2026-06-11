using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace ChickenHunt
{
    public class BossChicken : MonoBehaviour, IShootable
    {
        [Header("HP")]
        [SerializeField] private int _maxHP = 20;
        [SerializeField] private int _secondStageHP = 10;

        [Header("Movement")]
        [SerializeField] private float _moveSpeed = 4f;
        [SerializeField] private float _stopDistance = 0.05f;

        [Header("Stage 2 Shields")]
        [SerializeField] private BossShield _shieldPrefab;
        [SerializeField] private Transform[] _shieldSpawnPoints;
        [SerializeField] private int _shieldCount = 3;
        [SerializeField] private float _shieldRadius = 2f;

        private readonly List<BossShield> _activeShields = new List<BossShield>();

        private Vector3 _targetPosition;
        private int _currentHP;
        private bool _isMoving;
        private bool _isReady;
        private bool _stageTwoStarted;
        private TextMeshProUGUI _hpText;

        public event Action<BossChicken> OnBossDefeated;

        public void Initialize(Vector3 targetPosition, TextMeshProUGUI hpText)
        {
            _targetPosition = targetPosition;
            _hpText = hpText;

            _currentHP = _maxHP;
            _isMoving = true;
            _isReady = false;
            _stageTwoStarted = false;

            UpdateHPUI();
        }

        private void Update()
        {
            if (!_isMoving) return;

            transform.position = Vector3.MoveTowards(
                transform.position,
                _targetPosition,
                _moveSpeed * Time.deltaTime
            );

            if (Vector3.Distance(transform.position, _targetPosition) <= _stopDistance)
            {
                transform.position = _targetPosition;
                _isMoving = false;
                _isReady = true;
            }
        }

        public void OnShoot()
        {
            if (!_isReady) return;

            if (_stageTwoStarted && HasActiveShields())
            {
                if (ChickensManager.Instance != null)
                    ChickensManager.Instance.ShowMessage("\r\nBreak all the shields first!", 1.2f);

                return;
            }

            TakeDamage(1);
        }

        private void TakeDamage(int damage)
        {
            _currentHP = Mathf.Max(0, _currentHP - damage);
            UpdateHPUI();

            if (!_stageTwoStarted && _currentHP <= _secondStageHP && _currentHP > 0)
            {
                StartSecondStage();
                return;
            }

            if (_currentHP <= 0)
            {
                Die();
            }
        }

        private void StartSecondStage()
        {
            _stageTwoStarted = true;

            if (ChickensManager.Instance != null)
                ChickensManager.Instance.ShowMessage(
                    "2 Stage!\n\r\nBreak the shields, then hit the boss again.",
                    4f
                );

            SpawnShields();
        }

        private void SpawnShields()
        {
            ClearNullShields();

            if (_shieldPrefab == null)
                return;

            for (int i = 0; i < _shieldCount; i++)
            {
                Vector3 spawnPosition = GetShieldPosition(i);

                BossShield shield = Instantiate(
                    _shieldPrefab,
                    spawnPosition,
                    Quaternion.identity
                );

                shield.Initialize(this);
                _activeShields.Add(shield);
            }
        }

        private Vector3 GetShieldPosition(int index)
        {
            if (_shieldSpawnPoints != null &&
                index < _shieldSpawnPoints.Length &&
                _shieldSpawnPoints[index] != null)
            {
                return _shieldSpawnPoints[index].position;
            }

            float angle = (360f / Mathf.Max(1, _shieldCount)) * index * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * _shieldRadius;

            return transform.position + offset;
        }

        public void RemoveShield(BossShield shield)
        {
            if (_activeShields.Contains(shield))
                _activeShields.Remove(shield);
        }

        private bool HasActiveShields()
        {
            ClearNullShields();
            return _activeShields.Count > 0;
        }

        private void ClearNullShields()
        {
            for (int i = _activeShields.Count - 1; i >= 0; i--)
            {
                if (_activeShields[i] == null)
                    _activeShields.RemoveAt(i);
            }
        }

        private void UpdateHPUI()
        {
            if (_hpText == null) return;

            _hpText.gameObject.SetActive(true);
            _hpText.text = $"Boss HP: {_currentHP}/{_maxHP}";
        }

        private void Die()
        {
            OnBossDefeated?.Invoke(this);
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            for (int i = _activeShields.Count - 1; i >= 0; i--)
            {
                if (_activeShields[i] != null)
                    Destroy(_activeShields[i].gameObject);
            }

            _activeShields.Clear();
        }
    }
}