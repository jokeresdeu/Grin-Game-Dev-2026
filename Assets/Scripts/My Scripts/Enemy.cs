using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    private GameObject player_obj;

    public GameObject enemy;
    public GameObject death_menu;

    public int attack_damage = 20;
    public int enemy_hp = 100;

    public float speed = 5f;
    public float attack_radius = 0.3f;
    public float attack_cool_down = 1f;
    public float next_attack_time = 0f;

    public bool player_in_area;

    public Transform player_check;

    public LayerMask Player;

    void Start()
    {
        player_obj = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        Following();
        PlayerAttack();
    }

    void Following()
    {
        if (player_obj.transform.position.x > enemy.transform.position.x)
        {
            enemy.transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
        }
        else if (player_obj.transform.position.x < enemy.transform.position.x)
        {
            enemy.transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);
        }
    }
    void PlayerAttack()
    {
        player_in_area = Physics2D.OverlapCircle(player_check.position, attack_radius, Player);

        if (player_in_area && Time.time >= next_attack_time)
        {
            player_obj.GetComponent<Player>().hp -= attack_damage;

            if (player_obj.GetComponent<Player>().hp <= 0)
            {
                player_obj.GetComponent<Player>().anim.SetBool("death", true);

                StartCoroutine(LoadSceneWithDelay(2f));
            }

            IEnumerator LoadSceneWithDelay(float delay)
            {
                yield return new WaitForSeconds(delay);

                death_menu.SetActive(true);
            }

            next_attack_time = Time.time + attack_cool_down;
        }
    }
}
