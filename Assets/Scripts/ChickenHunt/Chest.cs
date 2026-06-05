using UnityEngine;

namespace ChickenHunt
{
    public class Chest : MonoBehaviour, IShootable
    {
        public void OnShoot()
        {
            Debug.Log("Chest destroyed, clearing screen");

            Chicken[] allChickens = Object.FindObjectsByType<Chicken>(FindObjectsSortMode.None);

            foreach (var chicken in allChickens)
            {
                if (chicken != null)
                {
                    chicken.OnShoot();
                }
            }

            Destroy(gameObject);
        }
    }
}