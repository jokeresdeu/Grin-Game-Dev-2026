using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Shoots projectiles in the direction the object is facing or toward the mouse.
/// Perfect for weapons, turrets, or any shooting mechanic.
/// </summary>
public class ProjectileShooter : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float projectileSpeed = 10f;
    
    [Header("Input")]
    [SerializeField] private KeyCode fireKey = KeyCode.Space;
    [SerializeField] private bool useMouseButton = false;
    [SerializeField] private int mouseButton = 0;
    
    [Header("Direction")]
    [SerializeField] private ShootDirection shootDirection = ShootDirection.FacingDirection;
    
    [Header("Fire Rate")]
    [SerializeField] private float fireRate = 0.2f;
    [SerializeField] private bool autoFire = false;
    
    [Header("Events")]
    [SerializeField] private UnityEvent onShoot;
    
    private float lastFireTime;
    private Camera mainCamera;
    
    public enum ShootDirection
    {
        FacingDirection,
        TowardMouse,
        Right,
        Left,
        Up,
        Down
    }
    
    private void Awake()
    {
        mainCamera = Camera.main;
        
        if (firePoint == null)
        {
            firePoint = transform;
        }
    }
    
    private void Update()
    {
        bool fireInput;
        
        if (useMouseButton)
        {
            fireInput = autoFire ? Input.GetMouseButton(mouseButton) : Input.GetMouseButtonDown(mouseButton);
        }
        else
        {
            fireInput = autoFire ? Input.GetKey(fireKey) : Input.GetKeyDown(fireKey);
        }
        
        if (fireInput && Time.time >= lastFireTime + fireRate)
        {
            Shoot();
        }
    }
    
    public void Shoot()
    {
        if (projectilePrefab == null) return;
        
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        
        Vector2 direction = GetShootDirection();
        
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        projectile.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = direction * projectileSpeed;
        }
        else
        {
            AutoMover mover = projectile.GetComponent<AutoMover>();
            if (mover != null)
            {
                mover.SetDirection(direction);
                mover.SetSpeed(projectileSpeed);
            }
        }
        
        lastFireTime = Time.time;
        onShoot?.Invoke();
    }
    
    private Vector2 GetShootDirection()
    {
        switch (shootDirection)
        {
            case ShootDirection.TowardMouse:
                Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                return ((Vector2)(mousePos - firePoint.position)).normalized;
                
            case ShootDirection.Right:
                return Vector2.right;
                
            case ShootDirection.Left:
                return Vector2.left;
                
            case ShootDirection.Up:
                return Vector2.up;
                
            case ShootDirection.Down:
                return Vector2.down;
                
            case ShootDirection.FacingDirection:
            default:
                return transform.right;
        }
    }
}
