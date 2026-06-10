using UnityEngine;

namespace Projects.CubeHopper.Scripts
{
    public class ParallaxLayer : MonoBehaviour
    {
        [SerializeField] private float _speedFactor = 0.3f;
        [SerializeField] private Transform[] _tiles;
        [SerializeField] private float _tileWidth = 20f;
        [SerializeField] private float _recycleThreshold = -20f;

        private void Update()
        {
            if (GameManager.Instance != null && GameManager.Instance.State != GameState.Playing)
                return;

            float worldSpeed = WorldSpeed.Instance != null ? WorldSpeed.Instance.Current : 6f;
            float step = worldSpeed * _speedFactor * Time.deltaTime;

            if (_tiles == null || _tiles.Length == 0)
            {
                transform.position += Vector3.left * step;
                if (transform.position.x <= _recycleThreshold)
                    transform.position += Vector3.right * _tileWidth;
                return;
            }

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
