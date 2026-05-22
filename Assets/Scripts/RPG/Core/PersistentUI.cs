using UnityEngine;

public class PersistentUI : MonoBehaviour
{
    private static PersistentUI _instance;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;

        DontDestroyOnLoad(gameObject);
    }
}