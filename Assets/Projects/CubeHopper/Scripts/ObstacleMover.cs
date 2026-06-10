using UnityEngine;

namespace Projects.CubeHopper.Scripts
{
    public class ObstacleMover : MonoBehaviour
    {
        [SerializeField] private float _despawnX = -15f;
        [SerializeField] private bool _hasBeenCleared;

        private void Update()
        {
            if (GameManager.Instance != null && GameManager.Instance.State != GameState.Playing)
                return;

            float speed = WorldSpeed.Instance != null ? WorldSpeed.Instance.Current : 6f;
            transform.position += Vector3.left * speed * Time.deltaTime;

            if (!_hasBeenCleared && transform.position.x < 0f)
            {
                _hasBeenCleared = true;
                if (ScoreManager.Instance != null)
                    ScoreManager.Instance.RegisterObstacleCleared();
            }

            if (transform.position.x < _despawnX)
                Destroy(gameObject);
        }
    }
}
