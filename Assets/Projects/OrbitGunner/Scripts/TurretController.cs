using UnityEngine;

namespace Projects.OrbitGunner.Scripts
{
    [RequireComponent(typeof(Weapon))]
    public class TurretController : MonoBehaviour
    {
        [SerializeField] private Transform _barrel;
        [SerializeField] private SpriteRenderer _barrelRenderer;
        [SerializeField] private float _rotationSpeed = 110f;
        [SerializeField] private float _frenzyMultiplier = 1.8f;
        [SerializeField] private float _recoilKick = 0.12f;
        [SerializeField] private float _recoilRecover = 6f;
        [SerializeField] private Color _frenzyColor = new Color(1f, 0.55f, 0.55f);

        private Weapon _weapon;
        private Color _barrelBaseColor = Color.white;
        private float _angle;
        private float _directionSign = 1f;
        private float _recoil;
        private float _barrelBaseDistance = 0.6f;
        private bool _subscribed;

        private void Awake()
        {
            _weapon = GetComponent<Weapon>();

            if (_barrelRenderer != null)
                _barrelBaseColor = _barrelRenderer.color;

            if (_barrel != null)
                _barrelBaseDistance = _barrel.localPosition.x;
        }

        private void OnEnable()
        {
            TrySubscribe();
        }

        private void OnDisable()
        {
            if (_subscribed && DifficultyDirector.Instance != null)
            {
                DifficultyDirector.Instance.FrenzyChanged -= OnFrenzyChanged;
                _subscribed = false;
            }
        }

        private void Update()
        {
            if (!_subscribed)
                TrySubscribe();

            if (GameManager.Instance != null && GameManager.Instance.State != GameState.Playing)
                return;

            bool frenzy = DifficultyDirector.Instance != null && DifficultyDirector.Instance.IsFrenzy;
            float speed = _rotationSpeed * (frenzy ? _frenzyMultiplier : 1f);

            _angle += speed * _directionSign * Time.deltaTime;
            float radians = _angle * Mathf.Deg2Rad;
            Vector2 aim = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));

            transform.rotation = Quaternion.Euler(0f, 0f, _angle);

            if (InputReader.FireHeld && _weapon.TryFire(aim))
                _recoil = _recoilKick;

            UpdateRecoil();
        }

        private void UpdateRecoil()
        {
            if (_barrel == null)
                return;

            _recoil = Mathf.MoveTowards(_recoil, 0f, _recoilRecover * Time.deltaTime);
            _barrel.localPosition = new Vector3(_barrelBaseDistance - _recoil, 0f, 0f);
        }

        private void OnFrenzyChanged(bool active)
        {
            if (active)
                _directionSign *= -1f;

            if (_barrelRenderer != null)
                _barrelRenderer.color = active ? _frenzyColor : _barrelBaseColor;
        }

        private void TrySubscribe()
        {
            if (_subscribed || DifficultyDirector.Instance == null)
                return;

            DifficultyDirector.Instance.FrenzyChanged += OnFrenzyChanged;
            _subscribed = true;
        }
    }
}
