using UnityEngine;
using UnityEngine.SceneManagement;

public class PlaneController : MonoBehaviour
{
    public float jumpForce = 5f;
    public float forwardSpeed = 3f;
    public float maxY = 5.5f;

    public GameObject gameOverPanel;
    public GameObject getReadyScreen;

    public GameObject puffPrefab;
    public Transform exhaustPoint;

    private Rigidbody2D rb;
    private Animator anim;
    public bool isDead = false;
    public bool gameStarted = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        gameOverPanel.SetActive(false);
        getReadyScreen.SetActive(true);

        anim.enabled = false;

        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.linearVelocity = Vector2.zero;
    }

    void Update()
    {
        if (isDead) return;

        if (transform.position.y > maxY)
        {
            Die();
        }

        if (!gameStarted)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                gameStarted = true;
                getReadyScreen.SetActive(false);
                anim.enabled = true;
                rb.bodyType = RigidbodyType2D.Dynamic;
                rb.linearVelocity = Vector2.up * jumpForce;

                if (AudioManager.instance != null) AudioManager.instance.PlaySFX(AudioManager.instance.jumpSound);

                if (puffPrefab != null && exhaustPoint != null)
                {
                    Instantiate(puffPrefab, exhaustPoint.position, Quaternion.identity);
                }
            }
            return;
        }

        rb.linearVelocity = new Vector2(forwardSpeed, rb.linearVelocity.y);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.linearVelocity = new Vector2(forwardSpeed, jumpForce);

            if (AudioManager.instance != null) AudioManager.instance.PlaySFX(AudioManager.instance.jumpSound);

            if (puffPrefab != null && exhaustPoint != null)
            {
                Instantiate(puffPrefab, exhaustPoint.position, Quaternion.identity);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Die();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Coin"))
        {
            ScoreManager.instance.AddScore(1);
            if (AudioManager.instance != null) AudioManager.instance.PlaySFX(AudioManager.instance.coinSound);
            Destroy(collision.gameObject);
            return;
        }

        Die();
    }

    private void Die()
    {
        if (isDead) return;

        isDead = true;
        rb.linearVelocity = Vector2.zero;
        anim.enabled = false;
        if (AudioManager.instance != null) AudioManager.instance.PlaySFX(AudioManager.instance.gameOverSound);
        gameOverPanel.SetActive(true);
    }

    public void RestartGame()
    {
        if (AudioManager.instance != null) AudioManager.instance.PlaySFX(AudioManager.instance.menuSelectSound);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    {
        if (AudioManager.instance != null) AudioManager.instance.PlaySFX(AudioManager.instance.menuSelectSound);
        SceneManager.LoadScene("MainMenu");
    }
}