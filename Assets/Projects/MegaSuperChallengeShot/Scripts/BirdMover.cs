using UnityEngine;

public class BirdMover : MonoBehaviour
{
    [SerializeField] private float _speed = 2f;
    [SerializeField] private Vector2 _direction = Vector2.right;

    private void Update()
    {
        transform.position += (Vector3)(_direction.normalized * (_speed * Time.deltaTime));
    }
}
