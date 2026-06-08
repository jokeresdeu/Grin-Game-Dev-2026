using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("Налаштування руху")]
    public float speed = 7f;             // Початкову швидкість збільшено для кращого відгуку
    public float tileSize = 1.5f;

    [Header("Коллайдер гравця")]
    public Collider playerCollider;

    [Header("Звукові ефекти")]
    public AudioClip coinSound;        

    private bool movingRight = false;
    private bool isDead = false;
    private bool isRolling = false;
    private bool canChangeDirection = true; // Запобігає скасуванню повороту через подвійний клік

    private Vector3 targetGridPosition;
    private Rigidbody rb;
    private GameManager gameManager;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gameManager = FindObjectOfType<GameManager>();
        rb.isKinematic = true;
        rb.useGravity = false;
        transform.position = new Vector3(0, 0.5f, 0);
        transform.rotation = Quaternion.identity;
        targetGridPosition = transform.position;
    }

    void Update()
    {
        if (isDead) return;
        // Фіксація кліку
        if (Time.timeScale > 0 && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)))
        {
            if (canChangeDirection)
            {
                movingRight = !movingRight;
                canChangeDirection = false; // Блокуємо повторні кліки, поки куб не докотиться
            }
        }
        // Автоматичний рух по сітці
        if (!isRolling)
        {
            Vector3 nextTarget = GetNextGridPosition();
            if (CheckNextPositionGround(nextTarget))
            {
                StartCoroutine(RollCubeLerp(nextTarget));
            }
            else
            {
                isDead = true;
                StartCoroutine(RollToDeath(nextTarget));
            }
        }
    }

    Vector3 GetNextGridPosition()
    {
        Vector3 offset = movingRight ? new Vector3(tileSize, 0, 0) : new Vector3(0, 0, tileSize);
        return targetGridPosition + offset;
    }

    bool CheckNextPositionGround(Vector3 positionToCheck)
    {
        Vector3 checkCenter = positionToCheck;
        checkCenter.y = 0f;
        Vector3 boxSize = new Vector3(tileSize * 0.45f, 0.2f, tileSize * 0.45f);
        Collider[] colliders = Physics.OverlapBox(checkCenter, boxSize, Quaternion.identity);
        foreach (Collider c in colliders)
        {
            if (c != playerCollider && !c.CompareTag("Coin"))
            {
                return true;
            }
        }
        return false;
    }

    IEnumerator RollCubeLerp(Vector3 targetCenter)
    {
        isRolling = true;
        Vector3 startCenter = transform.position;
        Quaternion startRot = transform.rotation;
        Quaternion targetRot = movingRight ? Quaternion.Euler(0, 0, -90) * startRot : Quaternion.Euler(90, 0, 0) * startRot;
        float elapsed = 0f;
        float duration = tileSize / speed;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            Vector3 currentPos = Vector3.Lerp(startCenter, targetCenter, t);

            float bumpHeight = transform.localScale.y * 0.207f;
            currentPos.y += Mathf.Sin(t * Mathf.PI) * bumpHeight;
            transform.position = currentPos;
            transform.rotation = Quaternion.Slerp(startRot, targetRot, t);
            yield return null;
        }

        // Жорстка фіксація координат в кінці кроку
        transform.position = targetCenter;
        Vector3 rot = transform.rotation.eulerAngles;
        rot.x = Mathf.Round(rot.x / 90f) * 90f;
        rot.y = Mathf.Round(rot.y / 90f) * 90f;
        rot.z = Mathf.Round(rot.z / 90f) * 90f;
        transform.rotation = Quaternion.Euler(rot);
        targetGridPosition = targetCenter;

        isRolling = false;
        canChangeDirection = true;
    }

    IEnumerator RollToDeath(Vector3 targetCenter)
    {
        // Прискорюємо рух у прірву
        float normalSpeed = speed;
        speed *= 1.6f;
        yield return StartCoroutine(RollCubeLerp(targetCenter));
        // Вмикаємо швидке падіння вниз
        rb.isKinematic = false;
        rb.useGravity = true;
        rb.linearVelocity = new Vector3(0, -18f, 0);
        // Миттєва пауза чисто символічно на рух камери вниз і відразу Game Over
        yield return new WaitForSeconds(0.2f);
        if (gameManager != null)
        {
            gameManager.GameOver();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            if (gameManager != null)
            {
                gameManager.AddScore(1);
            }
            // Граємо звук у точці підбору перед видаленням монетки
            if (coinSound != null)
            {
                AudioSource.PlayClipAtPoint(coinSound, other.transform.position);
            }
            Destroy(other.gameObject);
        }
    }
}