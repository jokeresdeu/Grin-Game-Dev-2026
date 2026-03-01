using UnityEngine;

/// <summary>
/// Бонусний об'єкт, що з'являється випадково.
/// Виявляється за допомогою Physics2D.OverlapBox (кожні N секунд
/// перевіряємо область навколо курсора). При «зборі» відновлює патрони.
///
/// Це — додатковий приклад використання Physics2D.Overlap*.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class PowerUp : MonoBehaviour
{
    public enum PowerUpType
    {
        Reload,       // перезарядка
        ExtraLife     // додаткове життя
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
        // Невелика анімація «покачування»
        _timer += Time.deltaTime;
        Vector3 pos = _startPos;
        pos.y += Mathf.Sin(_timer * bobSpeed) * bobAmplitude;
        transform.position = pos;
    }

    /// <summary>
    /// Викликається OverlapBox-перевіркою з PowerUpCollector.
    /// </summary>
    public void Collect()
    {
        if (GameManager.Instance == null) return;

        switch (type)
        {
            case PowerUpType.Reload:
                GameManager.Instance.Reload();
                break;
            case PowerUpType.ExtraLife:
                // Не додаємо понад максимум — просто перезавантажуємо патрони
                GameManager.Instance.Reload();
                break;
        }

        Destroy(gameObject);
    }
}
