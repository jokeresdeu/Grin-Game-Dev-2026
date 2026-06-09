using Projects.MegaSuperChallengeShot.Scripts;
using TMPro;
using UnityEngine;

public class CrosshairController : MonoBehaviour
{
    [SerializeField] private int _maxShots = 10;
    [SerializeField] private TMP_Text _shotsText;
    [SerializeField] private LayerMask _target;
    [SerializeField] private float _shotgunRadius = 1.5f;
    [SerializeField] private CrosshairAnimator _animator;

    private Camera _main;
    private int _currentShots;

    private void Start()
    {
        _main = Camera.main;

        if (_animator == null)
            _animator = GetComponent<CrosshairAnimator>();

        Reload();
    }

    private void Update()
    {
        FollowMouse();

        if (GameManager.Instance != null && GameManager.Instance.State != GameState.Playing)
            return;

        if (Input.GetKeyDown(KeyCode.Mouse0))
            FireSingle();
        else if (Input.GetKeyDown(KeyCode.Mouse1))
            FireShotgun();
        else if (Input.GetKeyDown(KeyCode.R))
            Reload();
    }

    private void FollowMouse()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = -_main.transform.position.z;
        Vector3 worldPosition = _main.ScreenToWorldPoint(mousePosition);
        worldPosition.z = 0f;
        transform.position = worldPosition;
    }

    private void FireSingle()
    {
        if (_currentShots <= 0)
            return;

        _currentShots--;
        UpdateShotsUI();

        if (_animator != null)
            _animator.PlayShoot();

        Ray ray = _main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity, _target);

        if (hit.collider != null)
            KillBird(hit.collider.gameObject);
    }

    private void FireShotgun()
    {
        if (_currentShots <= 0)
            return;

        _currentShots--;
        UpdateShotsUI();

        if (_animator != null)
            _animator.PlayShoot();

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, _shotgunRadius, _target);
        for (int i = 0; i < hits.Length; i++)
            KillBird(hits[i].gameObject);
    }

    private void KillBird(GameObject bird)
    {
        if (ScoreManager.Instance != null)
            ScoreManager.Instance.AddScore(bird.transform.position);

        Destroy(bird);
    }

    private void Reload()
    {
        _currentShots = _maxShots;
        UpdateShotsUI();

        if (_animator != null)
            _animator.PlayReload();
    }

    private void UpdateShotsUI()
    {
        if (_shotsText != null)
            _shotsText.text = $"{_currentShots}/{_maxShots}";
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _shotgunRadius);
    }
}
