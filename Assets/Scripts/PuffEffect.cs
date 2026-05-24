using UnityEngine;

public class PuffEffect : MonoBehaviour
{
    void Update()
    {
        transform.localScale -= Vector3.one * Time.deltaTime * 3f;
        if (transform.localScale.x <= 0)
        {
            Destroy(gameObject);
        }
    }
}