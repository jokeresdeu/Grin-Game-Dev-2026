using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FireballMover : MonoBehaviour
{
    private void Start()
    {
        // «находимо гравц€ ≥ спавнимось пр€мо над ним
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Vector3 pos = transform.position;
            pos.x = player.transform.position.x;
            transform.position = pos;
        }
    }
}