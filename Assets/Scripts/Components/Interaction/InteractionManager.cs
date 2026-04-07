using UnityEngine;
using TMPro;

public class InteractionManager : MonoBehaviour
{
    [Header("Settings")]
    public GameObject birdPrefab;
    public GameObject crosshair;
    public int score = 0;
    public int shotsMax = 5;
    private int shotsCurrent;

    [Header("UI")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI shotsText;
    public GameObject winMenuUI; 

    void Start()
    {
        shotsCurrent = shotsMax;
        Cursor.visible = false;
        Time.timeScale = 1f;
        UpdateUI();
        SpawnNewBird();
    }

    void Update()
    {
        if (crosshair != null)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
            crosshair.transform.position = mousePos;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (shotsCurrent > 0)
        {
            shotsCurrent--;

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            Collider2D hit = Physics2D.OverlapPoint(mousePos2D);

            if (hit != null && hit.CompareTag("Enemy"))
            {
                GameObject bird = hit.gameObject;

                Animator anim = bird.GetComponent<Animator>();
                if (anim != null)
                {
                    anim.SetTrigger("Die");
                }

                score++;

                if (score >= 5)
                {
                    WinGame();
                }

                Destroy(bird, 0.2f);

                if (score < 5)
                {
                    SpawnNewBird();
                }
            }
        }
        UpdateUI();
    }

    void WinGame()
    {
        if (winMenuUI != null)
        {
            winMenuUI.SetActive(true); 
            Time.timeScale = 0f;     
            Cursor.visible = true;     
        }
    }

    void SpawnNewBird()
    {
        if (birdPrefab != null)
        {
            Vector3 randomPos = new Vector3(Random.Range(-7f, 7f), Random.Range(-3f, 3f), 0);
            Instantiate(birdPrefab, randomPos, Quaternion.identity);
        }
    }

    void UpdateUI()
    {
        if (scoreText != null) scoreText.text = "Score: " + score;
        if (shotsText != null) shotsText.text = shotsCurrent + "/" + shotsMax;
    }
}