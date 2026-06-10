using UnityEngine;

namespace Projects.CubeHopper.Scripts
{
    public class GroundScroller : MonoBehaviour
    {
        [SerializeField] private Transform[] _tiles;
        [SerializeField] private float _tileWidth = 8f;
        [SerializeField] private float _recycleThreshold = -8f;

        private void Update()
        {
            if (GameManager.Instance != null && GameManager.Instance.State != GameState.Playing)
                return;

            float speed = WorldSpeed.Instance != null ? WorldSpeed.Instance.Current : 6f;
            float step = speed * Time.deltaTime;

            for (int i = 0; i < _tiles.Length; i++)
            {
                Transform tile = _tiles[i];
                tile.position += Vector3.left * step;

                if (tile.position.x <= _recycleThreshold)
                    tile.position += Vector3.right * (_tileWidth * _tiles.Length);
            }
        }
    }
}
