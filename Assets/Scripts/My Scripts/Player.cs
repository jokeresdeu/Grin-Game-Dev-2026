using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private GameObject player_obj;
    private float start_scale_x;
    public Rigidbody2D rb;
    public Animator anim;
    public float speed = 1f;
    public float jump_force = 5f;
    public float check_radius = 0.2f;
    public bool on_ground;
    public Transform ground_check;
    public Transform enemy_check;
    public LayerMask Ground;
    public LayerMask Enemy;
    public int hp = 100;
    public int damage = 50;
    public int killed_enemy = 0;
    public Text HPScore;
    public Text KEScore;

    private bool is_dead = false;

    void Start()
    {
        player_obj = GameObject.FindWithTag("Player");
        start_scale_x = player_obj.transform.localScale.x;
    }

    void Update()
    {
        if (is_dead) return;

        CheckingGround();
        SetScore();

        if (Input.GetKeyDown(KeyCode.Space) && on_ground)
            Jump();

        if (Input.GetMouseButtonDown(0))
        {
            anim.SetTrigger("Attack");
            Attack();
        }

        player_obj.transform.Translate(Move(0, 0) * speed * Time.deltaTime);
    }

    Vector2 Move(float moveX, float moveY)
    {
        if (Input.GetKey(KeyCode.A))
            moveX = -1f;
        else if (Input.GetKey(KeyCode.D))
            moveX = 1f;

        anim.SetFloat("Speed", Mathf.Abs(moveX));

        if (moveX != 0) Rotation(moveX);
        return new Vector2(moveX, moveY).normalized;
    }

    void Rotation(float moveX)
    {
        player_obj.transform.localScale = new Vector3(
            start_scale_x * moveX,
            player_obj.transform.localScale.y,
            player_obj.transform.localScale.z
        );
    }

    void Jump()
    {
        rb.AddForce(Vector2.up * jump_force, ForceMode2D.Impulse);
        anim.SetBool("isGrounded", false);
    }

    void CheckingGround()
    {
        on_ground = Physics2D.OverlapCircle(ground_check.position, check_radius, Ground);
        anim.SetBool("isGrounded", on_ground);
    }

    void Attack()
    {
        Collider2D hit = Physics2D.OverlapBox(
            enemy_check.position, new Vector2(2f, 1f), 0f, Enemy
        );
        if (hit != null)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                if (enemy.is_dead)
                {
                    killed_enemy += 1;
                    if (killed_enemy == 1)
                        StartCoroutine(RestartAfterDelay(2f));
                }
            }
        }
    }

    public void TakeDamage(int dmg)
    {
        if (is_dead) return;

        hp -= dmg;
        if (hp <= 0)
            Die();
    }

    void Die()
    {
        is_dead = true;
        anim.SetTrigger("Death");
        StartCoroutine(RestartAfterDelay(2f));
    }

    System.Collections.IEnumerator RestartAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    void SetScore()
    {
        HPScore.text = Convert.ToString(hp);
        KEScore.text = Convert.ToString(killed_enemy);
    }
}