using System;
using UnityEngine;

namespace ChickenHunt
{
    public class Chest : MonoBehaviour, IShootable
    {
        public event System.Action ChestShot;
        public void OnShoot()
        {
            ChestShot?.Invoke();
            Destroy(gameObject);
        }
    }
}