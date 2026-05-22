using UnityEngine;
using System.Collections.Generic;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private List<GameObject> _hearts; 

    public void UpdateHealth(int currentHealth)
    {
        for (int i = 0; i < _hearts.Count; i++)
        {
            if (i < currentHealth)
            {
                _hearts[i].SetActive(true);
            }
            else
            {
                _hearts[i].SetActive(false);
            }
        }
    }
}