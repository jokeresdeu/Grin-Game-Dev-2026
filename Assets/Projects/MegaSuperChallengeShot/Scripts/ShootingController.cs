using UnityEngine;

/// <summary>
/// Замінює стару CrosshairController. Керує прицілом (crosshair),
/// стрільбою через OverlapCircle та додатковим Raycast-промінем.
///
/// Використані методи фізики:
///   - Physics2D.OverlapCircleAll  — основна перевірка влучання (область навколо курсора)
///   - Physics2D.Raycast           — додаткова лінійна перевірка (промінь від нижнього краю)
/// </summary>
public class ShootingController : MonoBehaviour
{
    [Header("Crosshair")]
    [SerializeField] private float crosshairZ = 0f;

    [Header("Overlap Shooting")]
    [SerializeField] private float shotRadius = 0.5f;
    [SerializeField] private LayerMask targetLayer;

    [Header("Raycast Scanning")]
    [SerializeField] private float raycastDistance = 20f;
    [SerializeField] private LayerMask raycastLayer;
    [SerializeField] private Transform raycastOrigin;

    [Header("Visual")]
    [SerializeField] private LineRenderer laserLine;
    [SerializeField] private float laserDuration = 0.1f;

    private Camera _cam;
    private float _laserTimer;

    private void Start()
    {
        _cam = Camera.main;
        Cursor.visible = false;

        if (laserLine != null)
            laserLine.enabled = false;
    }

    private void Update()
    {
        if (GameManager.Instance == null) return;
        if (GameManager.Instance.State != GameState.Playing) return;

        MoveCrosshair();
        HandleShooting();
        HandleReload();
        HandleRaycastScan();
        UpdateLaser();
    }

    // =========================================================================
    // Crosshair follows mouse
    // =========================================================================
    private void MoveCrosshair()
    {
        Vector3 mouseScreen = Input.mousePosition;
        mouseScreen.z = Mathf.Abs(_cam.transform.position.z);
        Vector3 world = _cam.ScreenToWorldPoint(mouseScreen);
        world.z = crosshairZ;
        transform.position = world;
    }

    // =========================================================================
    // Overlap shooting — Physics2D.OverlapCircleAll
    // =========================================================================
    private void HandleShooting()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        if (!GameManager.Instance.TryShoot()) return;

        // --- Physics2D.OverlapCircleAll ---
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            transform.position, shotRadius, targetLayer);

        foreach (Collider2D hit in hits)
        {
            Bird bird = hit.GetComponent<Bird>();
            if (bird != null)
            {
                bird.Die();
            }
        }

        if (hits.Length == 0)
        {
            // Промах — не потрапили
        }
    }

    // =========================================================================
    // Reload (right-click)
    // =========================================================================
    private void HandleReload()
    {
        if (Input.GetMouseButtonDown(1))
        {
            GameManager.Instance.Reload();
        }
    }

    // =========================================================================
    // Raycast scanning — Physics2D.Raycast
    // Постійний промінь знизу вгору: якщо птах потрапляє на лінію,
    // показується лазерна лінія (візуальний індикатор).
    // При натисканні Space — автоматичний постріл по Raycast-цілі.
    // =========================================================================
    private void HandleRaycastScan()
    {
        if (raycastOrigin == null) return;

        Vector2 origin = raycastOrigin.position;
        Vector2 direction = Vector2.up;

        // --- Physics2D.Raycast ---
        RaycastHit2D hit = Physics2D.Raycast(origin, direction, raycastDistance, raycastLayer);

        if (hit.collider != null)
        {
            ShowLaser(origin, hit.point);

            // Space — автоматичний постріл по Raycast-цілі
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (GameManager.Instance.TryShoot())
                {
                    Bird bird = hit.collider.GetComponent<Bird>();
                    if (bird != null)
                    {
                        bird.Die();
                    }
                }
            }
        }

        // Для відладки — відображення променя в Scene-вікні
        Debug.DrawRay(origin, direction * raycastDistance, Color.red);
    }

    // =========================================================================
    // Laser visual (LineRenderer)
    // =========================================================================
    private void ShowLaser(Vector2 from, Vector2 to)
    {
        if (laserLine == null) return;

        laserLine.enabled = true;
        laserLine.SetPosition(0, new Vector3(from.x, from.y, 0f));
        laserLine.SetPosition(1, new Vector3(to.x, to.y, 0f));
        _laserTimer = laserDuration;
    }

    private void UpdateLaser()
    {
        if (laserLine == null || !laserLine.enabled) return;

        _laserTimer -= Time.deltaTime;
        if (_laserTimer <= 0f)
        {
            laserLine.enabled = false;
        }
    }

    // =========================================================================
    // Editor gizmos
    // =========================================================================
    private void OnDrawGizmosSelected()
    {
        // Overlap radius
        Gizmos.color = new Color(1f, 0f, 0f, 0.3f);
        Gizmos.DrawWireSphere(transform.position, shotRadius);

        // Raycast line
        if (raycastOrigin != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(raycastOrigin.position,
                (Vector2)raycastOrigin.position + Vector2.up * raycastDistance);
        }
    }
}
