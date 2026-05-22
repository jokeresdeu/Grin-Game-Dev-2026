using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InteractionManager : MonoBehaviour
{
    [Header("Налаштування гри")]
    public int score = 0;
    public float hp = 100f;

    [Header("Зв'язок з інтерфейсом (UI)")]
    public TextMeshProUGUI scoreText;
    public Slider healthSlider;

    [Header("Налаштування спауну (Рандом)")]
    public GameObject birdPrefab;
    public float minX = -7f;
    public float maxX = 7f;
    public float minY = -3f;
    public float maxY = 4f;

    void Start()
    {
        UpdateUI();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

        RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

        if (hit.collider != null && hit.collider.CompareTag("Enemy"))
        {
            Debug.Log("ВЛУЧАННЯ! Вбито: " + hit.collider.gameObject.name);

            Destroy(hit.collider.gameObject); 
            score += 1;                     

            SpawnNewBird();                  
        }
        else
        {
            Debug.Log("ПРОМАХ!");
            hp -= 10f;                       
        }

        UpdateUI(); 
    }

    void SpawnNewBird()
    {
        if (birdPrefab != null)
        {
            float randomX = Random.Range(minX, maxX);
            float randomY = Random.Range(minY, maxY);
            Vector3 randomPos = new Vector3(randomX, randomY, 0);

            Instantiate(birdPrefab, randomPos, Quaternion.identity);

            Debug.Log("НОВА ПТАШКА з'явилася в: " + randomPos);
        }
    }

    void UpdateUI()
    {
        if (scoreText != null) scoreText.text = "Score: " + score;
        if (healthSlider != null) healthSlider.value = hp;

        if (hp <= 0)
        {
            Debug.Log("КІНЕЦЬ ГРИ! Перезапуск...");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}