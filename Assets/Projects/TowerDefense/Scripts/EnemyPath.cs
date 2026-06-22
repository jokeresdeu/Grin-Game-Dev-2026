using System.Collections.Generic;
using UnityEngine;

namespace Projects.TowerDefense.Scripts
{
    /// <summary>
    /// The waypoint polyline enemies follow toward the base. Points are read from the
    /// object's LineRenderer (which also draws the visible "road"), so the path data and
    /// its visual are one and the same. The last point is the base.
    /// </summary>
    [RequireComponent(typeof(LineRenderer))]
    public class EnemyPath : MonoBehaviour
    {
        private readonly List<Vector3> _points = new List<Vector3>();
        private bool _collected;

        private void Awake() => Collect();
        private void OnEnable() => Collect();

        private void Collect()
        {
            if (_collected && _points.Count > 0)
                return;

            var line = GetComponent<LineRenderer>();
            _points.Clear();
            if (line != null)
            {
                for (int i = 0; i < line.positionCount; i++)
                    _points.Add(line.GetPosition(i));
            }
            _collected = _points.Count > 0;
        }

        public int Count
        {
            get
            {
                Collect();
                return _points.Count;
            }
        }

        public Vector3 GetPoint(int index)
        {
            Collect();
            return _points[Mathf.Clamp(index, 0, _points.Count - 1)];
        }

        public Vector3 Start => GetPoint(0);
        public Vector3 End => GetPoint(Count - 1);
    }
}
