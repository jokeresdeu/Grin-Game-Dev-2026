using UnityEngine;

public class BirdMover : MonoBehaviour
{
    [SerializeField] private float _speed = 2f;
    [SerializeField] private Vector2 _direction = Vector2.right;

    private bool _isMoving = true;

    public float CurrentSpeed => _isMoving ? _speed : 0f;

    private void Update()
    {
        if (!_isMoving)
            return;

        transform.position += (Vector3)(_direction.normalized * (_speed * Time.deltaTime));
    }

    public void StopMoving()
    {
        _isMoving = false;
    }
}
