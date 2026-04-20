using UnityEngine;

public class BirdSpawner : MonoBehaviour
{
    [SerializeField] private BirdMover _birdPrefab;
    
    void Start()
    {
        Instantiate(_birdPrefab, transform.position, Quaternion.identity);
    }

    void Update()
    {
        
    }
}
