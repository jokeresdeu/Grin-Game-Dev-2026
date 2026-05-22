using UnityEngine;

public class ShowUIOnDestroy : MonoBehaviour
{
    [SerializeField] private string uiObjectName = "DeadMenu";
    private GameObject uiElement;

    private void Awake()
    {
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.name == uiObjectName)
            {
                uiElement = obj;
                break;
            }
        }

        if (uiElement == null)
        {
            Debug.LogWarning($"UI object '{uiObjectName}' not found!");
        }
    }

    private void OnDestroy()
    {
        if (uiElement != null)
        {
            uiElement.SetActive(true);
            Time.timeScale = 0f;
        }
    }
}