using UnityEngine;

public class DefenderHealth : MonoBehaviour
{
    public int hp = 50;

    public void TakeDamage(int damage)
    {
        hp -= damage;

        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }
}