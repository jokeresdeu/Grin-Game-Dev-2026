using System;
using UnityEngine;

public class Hitbox : MonoBehaviour
{

    public void Slice()
    {
        if (ScoreManager.instance != null)
        {
            ScoreManager.instance.AddPoints(10);
        }

        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Border")
        {
            Destroy(this.gameObject);
        }
    }
}
