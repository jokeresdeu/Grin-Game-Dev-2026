using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{
    public GameObject enemyHealthBar;
    public GameObject enemy;

    public int gettingExp = 10;
    public float health = 100f;

    public Player player_info;

    private float _maxHealthBarWidth;

    void Start()
    {
        _maxHealthBarWidth = enemyHealthBar.transform.localScale.x;
    }

    void Update()
    {
        if (enemyHealthBar.transform.localScale.x <= 0)
        {
            Destroy(enemy);
            if (player_info.expProgress < 100)
            {
                player_info.expProgress += gettingExp;
                player_info.score += 100;
                player_info.scoreText.text = Convert.ToString(player_info.score);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Attack" && enemyHealthBar.transform.localScale.x > 0)
        {
            // Розраховуємо пропорційний урон від damage гравця
            float damageRatio = (_maxHealthBarWidth * player_info.damage) / health;
            
            enemyHealthBar.transform.localScale = new Vector3(
                Mathf.Max(0, enemyHealthBar.transform.localScale.x - damageRatio),
                enemyHealthBar.transform.localScale.y,
                enemyHealthBar.transform.localScale.z
            );
        }
    }
}
