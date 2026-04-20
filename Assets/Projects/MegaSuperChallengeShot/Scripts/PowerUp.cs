using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PowerUp : MonoBehaviour
{
    public enum PowerUpType
    {
        Reload,
        ExtraLife
    }

    [SerializeField] private PowerUpType type = PowerUpType.Reload;
    [SerializeField] private float lifetime = 5f;
    [SerializeField] private float bobSpeed = 2f;
    [SerializeField] private float bobAmplitude = 0.2f;

    private Vector3 _startPos;
    private float _timer;

    private void Start()
    {
        _startPos = transform.position;
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        Vector3 pos = _startPos;
        pos.y += Mathf.Sin(_timer * bobSpeed) * bobAmplitude;
        transform.position = pos;
    }

    public void Collect()
    {
        if (GameManager.Instance == null) return;

        switch (type)
        {
            case PowerUpType.Reload:
                GameManager.Instance.Reload();
                break;
            case PowerUpType.ExtraLife:
                GameManager.Instance.Reload();
                break;
        }

        Destroy(gameObject);
    }
}
