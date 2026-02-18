using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages a pool of reusable objects for better performance.
/// Use instead of Instantiate/Destroy for frequently spawned objects like projectiles.
/// </summary>
public class ObjectPool : MonoBehaviour
{
    [Header("Pool Settings")]
    [SerializeField] private GameObject prefab;
    [SerializeField] private int initialPoolSize = 10;
    [SerializeField] private bool expandable = true;
    
    [Header("Organization")]
    [SerializeField] private bool createContainer = true;
    
    private Queue<GameObject> pool = new Queue<GameObject>();
    private Transform container;
    
    public static ObjectPool Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        
        if (createContainer)
        {
            container = new GameObject($"Pool_{prefab.name}").transform;
            container.SetParent(transform);
        }
        
        InitializePool();
    }
    
    private void InitializePool()
    {
        for (int i = 0; i < initialPoolSize; i++)
        {
            CreateNewObject();
        }
    }
    
    private GameObject CreateNewObject()
    {
        GameObject obj = Instantiate(prefab);
        obj.SetActive(false);
        
        if (container != null)
        {
            obj.transform.SetParent(container);
        }
        
        pool.Enqueue(obj);
        return obj;
    }
    
    public GameObject GetObject()
    {
        if (pool.Count == 0)
        {
            if (expandable)
            {
                return CreateNewObject();
            }
            return null;
        }
        
        GameObject obj = pool.Dequeue();
        obj.SetActive(true);
        return obj;
    }
    
    public GameObject GetObject(Vector3 position, Quaternion rotation)
    {
        GameObject obj = GetObject();
        if (obj != null)
        {
            obj.transform.position = position;
            obj.transform.rotation = rotation;
        }
        return obj;
    }
    
    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        
        if (container != null)
        {
            obj.transform.SetParent(container);
        }
        
        pool.Enqueue(obj);
    }
}
