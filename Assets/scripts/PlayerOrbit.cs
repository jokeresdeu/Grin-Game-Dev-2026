using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerOrbit : MonoBehaviour
{
    [Header("Orbit Settings")]
    [SerializeField] private Transform orbitCenter;
    [SerializeField] private float radius = 2.5f;
    [SerializeField] private float angularSpeed = 160f;
    [SerializeField] private float speedIncreasePerSecond = 3f;

    private float angle;
    private int direction = 1;

    private void Start()
    {
        if (orbitCenter == null)
        {
            GameObject centerObject = GameObject.Find("OrbitCenter");
            if (centerObject != null)
                orbitCenter = centerObject.transform;
        }

        Vector2 offset = transform.position - orbitCenter.position;

        if (offset.magnitude > 0.1f)
        {
            radius = offset.magnitude;
            angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
        }
    }

    private void Update()
    {
        if (GameManager.Instance == null) return;
        if (GameManager.Instance.IsGameOver || GameManager.Instance.IsPaused) return;

        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                return;

            direction *= -1;
        }

        angularSpeed += speedIncreasePerSecond * Time.deltaTime;
        angle += direction * angularSpeed * Time.deltaTime;

        float radians = angle * Mathf.Deg2Rad;

        Vector3 newPosition = orbitCenter.position + new Vector3(
            Mathf.Cos(radians),
            Mathf.Sin(radians),
            0f
        ) * radius;

        transform.position = newPosition;

        Vector3 directionFromCenter = transform.position - orbitCenter.position;
        transform.up = directionFromCenter.normalized;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Obstacle>() != null)
        {
            GameManager.Instance.GameOver();
        }

        if (collision.GetComponent<Collectible>() != null)
        {
            GameManager.Instance.AddCrystalScore();
            Destroy(collision.gameObject);
        }
    }
}