using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    public GameObject deathParticlesPrefab;
    public GameObject coinBurstPrefab;

    private float baseGravity;
    private float currentDirection = 1f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        baseGravity = Mathf.Abs(rb.gravityScale);

        currentDirection = Mathf.Sign(rb.gravityScale);

        SpriteRenderer render = GetComponent<SpriteRenderer>();
        ParticleSystem trail = GetComponentInChildren<ParticleSystem>();

        string selectedSkin = PlayerPrefs.GetString("SelectedSkin", "Default");

        if (selectedSkin == "Green")
        {
            if (render != null) render.color = Color.green;
            if (trail != null) { var main = trail.main; main.startColor = Color.green; }
        }
        else if (selectedSkin == "Gold")
        {
            Color goldColor = new Color(1f, 0.84f, 0f);
            if (render != null) render.color = goldColor;
            if (trail != null) { var main = trail.main; main.startColor = goldColor; }
        }
        else
        {
            if (render != null) render.color = Color.white;
            if (trail != null) { var main = trail.main; main.startColor = Color.white; }
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            currentDirection *= -1f;
        }

        if (GameManager.instance != null)
        {
            rb.gravityScale = currentDirection * baseGravity * GameManager.instance.globalSpeedMultiplier;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name.Contains("Obstacle"))
        {
            if (AudioManager.instance != null)
            {
                AudioManager.instance.PlaySFX(AudioManager.instance.deathSound);
            }

            if (deathParticlesPrefab != null)
            {
                Instantiate(deathParticlesPrefab, transform.position, Quaternion.identity);
            }

            gameObject.SetActive(false);
            if (CameraShake.instance != null) CameraShake.instance.TriggerShake();
            GameManager.instance.GameOver();
        }
        else if (other.CompareTag("Coin"))
        {
            if (coinBurstPrefab != null)
            {
                Instantiate(coinBurstPrefab, other.transform.position, Quaternion.identity);
            }

            GameManager.instance.AddCoin();
            Destroy(other.gameObject);
        }
    }
}