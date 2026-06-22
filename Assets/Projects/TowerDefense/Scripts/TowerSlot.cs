using UnityEngine;

namespace Projects.TowerDefense.Scripts
{
    /// <summary>
    /// A fixed buildable node beside the path. Clicking it (via BuildManager) opens the
    /// build/upgrade bar. Holds the tower placed on it; its dim marker hides once occupied.
    /// </summary>
    public class TowerSlot : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _renderer;
        [SerializeField] private float _pickRadius = 0.65f;

        public Tower Tower { get; private set; }
        public bool Occupied => Tower != null;
        public float PickRadius => _pickRadius;
        public Vector3 Position => transform.position;

        private void Awake()
        {
            if (_renderer == null)
                _renderer = GetComponent<SpriteRenderer>();
            UpdateVisual();
        }

        public void SetTower(Tower tower)
        {
            Tower = tower;
            UpdateVisual();
        }

        public void ClearTower()
        {
            Tower = null;
            UpdateVisual();
        }

        private void UpdateVisual()
        {
            if (_renderer != null)
                _renderer.enabled = !Occupied;
        }
    }
}
