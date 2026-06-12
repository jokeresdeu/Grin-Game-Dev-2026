using System;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemy1;
    public GameObject enemy2;
    public GameObject enemy3;

    public Transform spawnPoint_1;
    public Transform spawnPoint_2;
    public Transform spawnPoint_3;
    public Transform spawnPoint_4;
    public Transform spawnPoint_5;
    public Transform spawnPoint_6;
    public Transform spawnPoint_7;
    public Transform spawnPoint_8;
    public Transform spawnPoint_9;
    public Transform spawnPoint_10;
    public Transform spawnPoint_11;
    public Transform spawnPoint_12;

    private int enemy_spawned_count = 2;

    void Spawn()
    {
        GameObject[] enemies = { enemy1, enemy2, enemy3 };
        Transform[] spawners = { 
            spawnPoint_1, spawnPoint_2, spawnPoint_3, 
            spawnPoint_4, spawnPoint_5, spawnPoint_6,
            spawnPoint_7, spawnPoint_8, spawnPoint_9, 
            spawnPoint_10, spawnPoint_11, spawnPoint_12 
        };
        System.Random random = new System.Random();
        GameObject enemy = enemies[random.Next(enemies.Length)];
        Transform spawnPoint = spawners[random.Next(spawners.Length)];
        Instantiate(enemy, spawnPoint.position, Quaternion.identity, null);
    }

    void Start()
    {
        Spawn();
    }

    void Update()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length - 3 == 0)
        {
            for (int i = 0; i < enemy_spawned_count; i++)
                Spawn();
            enemy_spawned_count += 1;
        }
    }
}
