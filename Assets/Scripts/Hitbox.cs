using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public GameObject wholeTarget;
    public GameObject slicedTarget;

    private bool isSliced = false;

    public void Slice()
    {
        if (isSliced == false)
        {
            isSliced = true;

            if (ScoreManager.instance != null)
            {
                ScoreManager.instance.AddPoints(10);
            }

            wholeTarget.SetActive(false);
            slicedTarget.SetActive(true);

            slicedTarget.transform.SetParent(null);

            Rigidbody2D[] parts = slicedTarget.GetComponentsInChildren<Rigidbody2D>();
            foreach (Rigidbody2D part in parts)
            {
                Vector2 randomForce = new Vector2(Random.Range(-3f, 3f), Random.Range(1f, 3f));
                part.AddForce(randomForce, ForceMode2D.Impulse);
            }

            Destroy(slicedTarget, 3f);

            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Border")
        {
            Destroy(this.gameObject);
        }
    }
}
