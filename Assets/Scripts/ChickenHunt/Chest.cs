using System;
using UnityEngine;

namespace ChickenHunt
{
    public class Chest : MonoBehaviour, IShootable
    {
        public void OnShoot(int damage)
        {
            Debug.LogError("You shot chest");
            Destroy(gameObject);
        }
    }
}