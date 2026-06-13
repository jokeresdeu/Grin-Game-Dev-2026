using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    private GameObject player_obj;

    public GameObject enemy;

    public int attack_damage = 20;
    public int enemy_hp = 100;

    public float speed = 5f;
    public float attack_radius = 0.3f;
    public float attack_cool_down = 1f;
    public float next_attack_time = 0f;

    public bool player_in_area;

    public Transform player_check;
    public LayerMask Player;

    // Анімації
    private Animator anim;
    public bool is_dead = false;

    void Start()
    {
        player_obj = GameObject.FindWithTag("Player");
        // Шукаємо Animator на дочірньому об'єкті або на самому ворогу
        anim = enemy.GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (is_dead) return;

        Following();
        PlayerAttack();
        UpdateAnimation();
    }

    void UpdateAnimation()
    {
        if (anim == null) return;

        // Визначаємо чи ворог рухається
        float distToPlayer = Mathf.Abs(
            player_obj.transform.position.x - enemy.transform.position.x
        );

        // AnimState: 0 = idle, 1 = run
        bool isMoving = distToPlayer > 0.1f;
        anim.SetInteger("AnimState", isMoving ? 1 : 0);

        // AirSpeedY для jump/fall анімації
        Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
        if (rb != null)
            anim.SetFloat("AirSpeedY", rb.linearVelocity.y);
    }

    void Following()
    {
        if (player_obj == null) return;

        float dir = player_obj.transform.position.x > enemy.transform.position.x ? 1f : -1f;
        enemy.transform.position += new Vector3(dir * speed * Time.deltaTime, 0, 0);

        // Фліп спрайту
        Vector3 scale = enemy.transform.localScale;
        scale.x = dir > 0 
            ? Mathf.Abs(scale.x) 
            : -Mathf.Abs(scale.x);
        enemy.transform.localScale = scale;
    }

    void PlayerAttack()
    {
        player_in_area = Physics2D.OverlapCircle(
            player_check.position, attack_radius, Player
        );
        if (player_in_area && Time.time >= next_attack_time)
        {
            if (anim != null)
                anim.SetTrigger("Attack1");

            var playerScript = player_obj.GetComponent<Player>();
            if (playerScript != null)
                playerScript.TakeDamage(attack_damage); // ← просто це

            next_attack_time = Time.time + attack_cool_down;
        }
    }

    // Викликається коли гравець б'є ворога
    public void TakeDamage(int damage)
    {
        if (is_dead) return;

        enemy_hp -= damage;

        if (anim != null)
            anim.SetTrigger("Hurt");

        if (enemy_hp <= 0)
            Die();
    }

    void Die()
    {
        is_dead = true;

        if (anim != null)
            anim.SetTrigger("Death");

        // Вимикаємо колайдер щоб гравець не взаємодіяв з мертвим ворогом
        var col = enemy.GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        StartCoroutine(DestroyAfterDeath(1.5f));
    }

    IEnumerator DestroyAfterDeath(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(enemy);
    }

    IEnumerator LoadSceneWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
    }
}
