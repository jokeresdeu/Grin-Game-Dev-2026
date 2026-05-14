using UnityEngine;

public class BirdSpawner : MonoBehaviour
{
    [SerializeField] private BirdMover _birdPrefab;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instantiate(_birdPrefab, transform.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
