using System;
using UnityEngine;

namespace Projects.OrbitGunner.Scripts
{

    public class OverdriveMeter : MonoBehaviour
    {
        public static OverdriveMeter Instance { get; private set; }

        public event Action<float> Changed;
        public event Action Triggered;

        [SerializeField] private float _maxCharge = 12f;

        public float Current { get; private set; }
        public float Normalized => _maxCharge > 0f ? Mathf.Clamp01(Current / _maxCharge) : 0f;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        private void Start()
        {
            Changed?.Invoke(Normalized);
        }

        public void AddCharge(float amount)
        {
            if (GameManager.Instance != null && GameManager.Instance.State != GameState.Playing)
                return;

            Current += amount;

            if (Current >= _maxCharge)
            {
                Current = 0f;
                Changed?.Invoke(0f);
                Triggered?.Invoke();

                if (NovaBurst.Instance != null)
                    NovaBurst.Instance.Trigger();
            }
            else
            {
                Changed?.Invoke(Normalized);
            }
        }

        private void OnDestroy()
        {
            if (Instance == this)
                Instance = null;
        }
    }
}
