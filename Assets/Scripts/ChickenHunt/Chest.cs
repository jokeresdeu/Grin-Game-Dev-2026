using System.Collections;
using UnityEngine;

namespace ChickenHunt
{
    public class Chest : MonoBehaviour, IShootable
    {
        [SerializeField] private Animator _animator;

        public void OnShoot()
        {
            if (_animator != null)
            {
                _animator.SetTrigger("Open");
            }

            StartCoroutine(ExplosionRoutine());
        }

        private IEnumerator ExplosionRoutine()
        {
            yield return new WaitForSeconds(0.45f);

            Chicken[] allChickens = Object.FindObjectsByType<Chicken>(FindObjectsSortMode.None);

            foreach (var chicken in allChickens)
            {
                if (chicken != null)
                {
                    chicken.OnShoot();
                }
            }

            yield return new WaitForSeconds(0.35f);
            Destroy(gameObject);
        }
    }
}