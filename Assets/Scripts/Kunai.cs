using UnityEngine;

public class Kunai : MonoBehaviour
{
    public enum KunaiState { Orbiting, Flying, Stuck }

    public KunaiState CurrentState { get; private set; } = KunaiState.Orbiting;

    [SerializeField] float flySpeed = 14f;

    public float spriteAngleOffset = 0f;

    KunaiOrbitController orbitController;
    Transform            ninjaTransform;
    Vector2              flyDirection;

    public void Init(KunaiOrbitController controller, Transform ninja)
    {
        orbitController = controller;
        ninjaTransform  = ninja;
        CurrentState    = KunaiState.Orbiting;
    }

    public void Throw(Vector2 direction)
    {
        CurrentState = KunaiState.Flying;
        flyDirection = direction.normalized;
    }

    void Update()
    {
        if (CurrentState != KunaiState.Flying) return;

        transform.position += (Vector3)(flyDirection * flySpeed * Time.deltaTime);

        Vector3 vp = Camera.main.WorldToViewportPoint(transform.position);
        if (vp.x < -0.1f || vp.x > 1.1f || vp.y < -0.1f || vp.y > 1.1f)
        {
            orbitController.NotifyKunaiLost(this);
            gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (CurrentState != KunaiState.Flying) return;

        TargetCube cube = other.GetComponent<TargetCube>();
        if (cube != null)
        {
            CurrentState = KunaiState.Stuck;
            transform.SetParent(other.transform);
            cube.Hit();
            orbitController.NotifyKunaiStuck(this);
        }
    }
}
