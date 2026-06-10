using UnityEngine;

public class ShopPanelController : MonoBehaviour
{
    [Header("Shop")]
    [SerializeField] private GameObject shopPanel;

    private void Start()
    {
        CloseShop();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseShop();
        }
    }

    public void OpenShop()
    {
        shopPanel.SetActive(true);
    }

    public void CloseShop()
    {
        shopPanel.SetActive(false);
    }

    public void ToggleShop()
    {
        shopPanel.SetActive(!shopPanel.activeSelf);
    }
}