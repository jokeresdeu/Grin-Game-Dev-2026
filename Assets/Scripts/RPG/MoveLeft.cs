using UnityEngine;

namespace RPG
{
    public class MoveLeft : MonoBehaviour
    {
        public float speed = 5f;
        

        private Camera _mainCamera;

        private void Start()
        {
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            if (RPGGameManager.Instance != null && RPGGameManager.Instance.IsArenaPaused)
            {
                return;
            }

            transform.position += Vector3.left * speed * Time.deltaTime;

            if (_mainCamera != null)
            {

                if (transform.position.x < _mainCamera.transform.position.x - 15f)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}


