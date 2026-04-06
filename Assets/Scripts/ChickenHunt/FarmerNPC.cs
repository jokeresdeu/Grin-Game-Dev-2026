using UnityEngine;

namespace ChickenHunt
{
    public class FarmerNPC : MonoBehaviour
    {
        private Animator _animator;
        private float _timer;

        private int _currentAction;
        private int _walkDirection = 1;

        [Header("Settings")]
        [SerializeField] private float _minActionTime = 2f;
        [SerializeField] private float _maxActionTime = 5f;
        [SerializeField] private float _walkSpeed = 2f;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            PerformRandomAction();
        }

        private void Update()
        {
            _timer -= Time.deltaTime;

            if (_timer <= 0f)
            {
                PerformRandomAction();
                _timer = Random.Range(_minActionTime, _maxActionTime);
            }

            if (_currentAction == 0)
            {
                transform.position += new Vector3(_walkDirection * _walkSpeed * Time.deltaTime, 0, 0);
            }
        }

        private void PerformRandomAction()
        {
            if (_animator == null) return;

            _currentAction = Random.Range(0, 3);

            switch (_currentAction)
            {
                case 0:
                    _animator.Play("Farmer_Walk");

                    _walkDirection = Random.value > 0.5f ? 1 : -1;

                    transform.localScale = new Vector3(_walkDirection, 1, 1);
                    break;

                case 1:
                    _animator.Play("Farmer_Crouch");
                    break;

                case 2:
                    _animator.Play("Farmer_Jump");
                    break;
            }
        }
    }
}