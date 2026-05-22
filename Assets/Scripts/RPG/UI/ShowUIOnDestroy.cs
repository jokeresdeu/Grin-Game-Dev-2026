using UnityEngine;

public class ShowUIOnDestroy : MonoBehaviour
{

    private GameObject uiElement;

    private void Start()
    {
        var ui = FindObjectOfType<Canvas>(true); // true = включає неактивні

        if (ui != null)
        {
            uiElement = ui.transform.Find("WinMenu")?.gameObject;
        }
    }

    private void OnDestroy()
    {
        if (gameObject.scene.isLoaded && uiElement != null)
        {
            uiElement.SetActive(true);
        }
    }
}