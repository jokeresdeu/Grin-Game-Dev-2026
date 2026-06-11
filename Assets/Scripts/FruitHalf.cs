using UnityEngine;

public class FruitHalf : MonoBehaviour
{
    [SerializeField] private float destroyTime = 3f;

    private void Start()
    {
        Destroy(gameObject, destroyTime);
    }
}