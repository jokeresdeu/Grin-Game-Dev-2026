using UnityEngine;
using TMPro;

namespace ChickenHunt
{
    public class Weapon : MonoBehaviour
    {
        [Header("Ammo")]
        [SerializeField] private int _maxAmmo = 5;
        [SerializeField] private float _reloadTime = 1.5f;

        [Header("Crosshair")]
        [SerializeField] private Transform _crosshair;
        [SerializeField] private float _smoothTime = 0.05f;

        [Header("UI")]
        [SerializeField] private TextMeshProUGUI _ammoText;
        [SerializeField] private GameObject _reloadingIndicator;

        [Header("Audio")]
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _shootClip;
        [SerializeField] private AudioClip _reloadClip;

        private Camera _camera;
        private int _currentAmmo;
        private float _reloadTimer;
        private bool _isReloading;
        private Vector3 _velocity;

        private void Awake()
        {
            _camera = Camera.main;
            _currentAmmo = _maxAmmo;
        }

        private void Start()
        {
            UpdateAmmoUI();

            if (_reloadingIndicator != null)
                _reloadingIndicator.SetActive(false);

            if (_crosshair != null && _camera != null)
            {
                Vector3 mouseWorld = _camera.ScreenToWorldPoint(Input.mousePosition);
                mouseWorld.z = 0f;
                _crosshair.position = mouseWorld;
            }
        }

        private void Update()
        {
            UpdateCrosshair();
            HandleInput();
            UpdateReload();
        }

        private void UpdateCrosshair()
        {
            if (_crosshair == null || _camera == null) return;

            Vector3 mouseWorld = _camera.ScreenToWorldPoint(Input.mousePosition);
            mouseWorld.z = 0f;

            _crosshair.position = Vector3.SmoothDamp(
                _crosshair.position,
                mouseWorld,
                ref _velocity,
                _smoothTime
            );
        }

        private void HandleInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                TryShoot();
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                TryReload();
            }
        }

        private void UpdateReload()
        {
            if (!_isReloading) return;

            _reloadTimer -= Time.deltaTime;

            if (_reloadTimer <= 0f)
            {
                FinishReload();
            }
        }

        private void TryShoot()
        {
            if (_isReloading || _currentAmmo <= 0) return;

            _currentAmmo--;
            UpdateAmmoUI();

            PlayShootSound();

            Vector3 mouseWorld = _camera.ScreenToWorldPoint(Input.mousePosition);
            mouseWorld.z = 0f;

            RaycastHit2D hit = Physics2D.Raycast(mouseWorld, Vector2.zero);

            if (hit.collider != null && hit.collider.TryGetComponent(out IShootable target))
            {
                target.OnShoot(1);
            }

            if (_currentAmmo <= 0)
            {
                StartReload();
            }
        }

        private void TryReload()
        {
            if (_isReloading || _currentAmmo == _maxAmmo) return;

            StartReload();
        }

        private void StartReload()
        {
            _isReloading = true;
            _reloadTimer = _reloadTime;

            if (_reloadingIndicator != null)
                _reloadingIndicator.SetActive(true);

            PlayReloadSound();
        }

        private void FinishReload()
        {
            _isReloading = false;
            _currentAmmo = _maxAmmo;

            UpdateAmmoUI();

            if (_reloadingIndicator != null)
                _reloadingIndicator.SetActive(false);
        }

        private void UpdateAmmoUI()
        {
            if (_ammoText != null)
                _ammoText.text = $"{_currentAmmo}/{_maxAmmo}";
        }

        private void PlayShootSound()
        {
            if (_audioSource == null || _shootClip == null) return;

            _audioSource.pitch = Random.Range(0.97f, 1.03f);
            _audioSource.PlayOneShot(_shootClip);
        }

        private void PlayReloadSound()
        {
            if (_audioSource == null || _reloadClip == null) return;

            _audioSource.pitch = 1f;
            _audioSource.PlayOneShot(_reloadClip);
        }

        public void AddMaxAmmo(int amount)
        {
            if (amount <= 0) return;

            _maxAmmo += amount;
            _currentAmmo = Mathf.Min(_currentAmmo + amount, _maxAmmo);
            UpdateAmmoUI();
        }

        public void RefillAmmo()
        {
            _currentAmmo = _maxAmmo;
            UpdateAmmoUI();
        }
    }
}