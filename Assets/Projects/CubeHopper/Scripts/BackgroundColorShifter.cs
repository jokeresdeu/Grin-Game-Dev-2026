using UnityEngine;

namespace Projects.CubeHopper.Scripts
{
    public class BackgroundColorShifter : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private Gradient _gradient;
        [SerializeField] private float _smoothing = 1.5f;

        private Color _current;

        private void Awake()
        {
            if (_camera == null)
                _camera = Camera.main;
            if (_camera != null)
                _current = _camera.backgroundColor;
        }

        private void Update()
        {
            if (_camera == null || _gradient == null)
                return;

            float t = WorldSpeed.Instance != null ? WorldSpeed.Instance.NormalizedSpeed : 0f;
            Color target = _gradient.Evaluate(t);
            _current = Color.Lerp(_current, target, Time.deltaTime * _smoothing);
            _camera.backgroundColor = _current;
        }
    }
}
