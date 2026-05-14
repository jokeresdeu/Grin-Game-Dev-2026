using System;
using UnityEngine;

public class BirdMover : MonoBehaviour
{
    [SerializeField] private float _speed;
    
    void Start()
    {
        Debug.Log("Hello world");
    }

    // Update is called once per frame
    void Update() 
    {
        transform.position += Vector3.right * _speed;
    }

    private void FixedUpdate()
    {
        
    }
}
