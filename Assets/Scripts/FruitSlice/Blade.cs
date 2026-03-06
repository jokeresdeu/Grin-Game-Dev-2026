using UnityEngine;

namespace FruitSlice
{
    public class Blade : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private LayerMask _sliceableLayer;
        [SerializeField] private float _minSliceDistance = 0.5f;

        [Header("Trail")]
        [SerializeField] private TrailRenderer _trailRenderer;

        private Camera _camera;
        private Vector3 _lastMouseWorld;
        private bool _isDragging;

        private void Awake()
        {
            _camera = Camera.main;

            if (_trailRenderer != null)
                _trailRenderer.enabled = false;
        }

        private void Update()
        {
            HandleInput();
            UpdateTrail();
        }

        private void HandleInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                StartDrag();
            }
            else if (Input.GetMouseButton(0) && _isDragging)
            {
                ContinueDrag();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                EndDrag();
            }
        }

        private void StartDrag()
        {
            _isDragging = true;
            _lastMouseWorld = GetMouseWorld();

            if (_trailRenderer != null)
            {
                _trailRenderer.Clear();
                _trailRenderer.enabled = true;
            }
        }

        private void ContinueDrag()
        {
            Vector3 currentMouse = GetMouseWorld();

            if (Vector3.Distance(currentMouse, _lastMouseWorld) >= _minSliceDistance)
            {
                CheckSlice(_lastMouseWorld, currentMouse);
                _lastMouseWorld = currentMouse;
            }
        }

        private void EndDrag()
        {
            _isDragging = false;

            if (_trailRenderer != null)
                _trailRenderer.enabled = false;
        }

        private void UpdateTrail()
        {
            if (_trailRenderer == null || !_isDragging) 
                return;

            _trailRenderer.transform.position = GetMouseWorld();
        }

        private void CheckSlice(Vector3 start, Vector3 end)
        {
            RaycastHit2D[] hits = Physics2D.LinecastAll(start, end, _sliceableLayer);

            foreach (var hit in hits)
            {
                var fruit = hit.collider.GetComponent<Fruit>();
                if (fruit != null && !fruit.IsSliced)
                {
                    Vector2 sliceDirection = (end - start).normalized;
                    fruit.Slice(sliceDirection);
                }
            }
        }

        private Vector3 GetMouseWorld()
        {
            Vector3 mouseWorld = _camera.ScreenToWorldPoint(Input.mousePosition);
            mouseWorld.z = 0f;
            return mouseWorld;
        }
    }
}
