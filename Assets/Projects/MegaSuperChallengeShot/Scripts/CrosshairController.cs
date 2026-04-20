using Projects.MegaSuperChallengeShot.Scripts;
using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class CrosshairController : MonoBehaviour
{
    [SerializeField] private int _maxShots = 6;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private LayerMask _target;
    [SerializeField] private float _hitRadius = 0.8f;
    [SerializeField] private float _minimumHitRadius = 0.25f;
    [SerializeField] private bool _scaleHitRadiusWithCrosshair = false;

    [Header("Line Renderer (shot tracer)")]
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private float _lineDisplayDuration = 0.05f;

    private Camera _main;
    private int _currentShots;
    private CrosshairAnimator _crosshairAnimator;
    private float _lineTimer;
    private float _configuredScaleFactor = 1f;

    private void Start()
    {
        _main = Camera.main;
        _crosshairAnimator = GetComponent<CrosshairAnimator>();
        _configuredScaleFactor = GetScaleFactor(transform.localScale);
        Cursor.visible = false;

        SetMaxShots();

        if (_lineRenderer != null)
        {
            _lineRenderer.enabled = false;
            _lineRenderer.startWidth = 0.05f;
            _lineRenderer.endWidth = 0.05f;
        }
    }

    private void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 0f;
        Vector3 worldPosition = _main.ScreenToWorldPoint(mousePosition);
        worldPosition.z = 0f;
        transform.position = worldPosition;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (_currentShots > 0)
            {
                Shoot(worldPosition);
            }
            else
            {
                if (_crosshairAnimator != null)
                {
                    _crosshairAnimator.PlayEmptyAnimation();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Reload();
        }

        if (_lineRenderer != null && _lineRenderer.enabled)
        {
            _lineTimer -= Time.deltaTime;
            if (_lineTimer <= 0f)
            {
                _lineRenderer.enabled = false;
            }
        }
    }

    private void Shoot(Vector3 worldPosition)
    {
        _currentShots--;
        _text.text = $"{_currentShots}/{_maxShots}";

        if (_crosshairAnimator != null)
        {
            _crosshairAnimator.PlayShootAnimation();
        }

        float effectiveHitRadius = GetEffectiveHitRadius();
        List<Component> hitTargets = CollectHitTargets(worldPosition, effectiveHitRadius);

        for (int i = 0; i < hitTargets.Count; i++)
        {
            BirdMover birdMover = hitTargets[i] as BirdMover;
            if (birdMover != null)
            {
                ScoreManager.Instance.AddScore();
                birdMover.OnShot();
            }
            else
            {
                ChestAnimationController chest = hitTargets[i] as ChestAnimationController;
                if (chest != null)
                {
                    chest.ToggleChest();
                }
                else
                {
                    ScoreManager.Instance.AddScore();
                    Destroy(hitTargets[i].gameObject);
                }
            }
        }
    }

    private void Reload()
    {
        SetMaxShots();

        if (_crosshairAnimator != null)
        {
            _crosshairAnimator.PlayReloadAnimation();
        }
    }

    private void SetMaxShots()
    {
        _currentShots = _maxShots;
        _text.text = $"{_currentShots}/{_maxShots}";
    }

    private void OnDestroy()
    {
        Cursor.visible = true;
    }

    private float GetEffectiveHitRadius()
    {
        if (!_scaleHitRadiusWithCrosshair)
        {
            return _hitRadius;
        }

        return Mathf.Max(_minimumHitRadius, _hitRadius * _configuredScaleFactor);
    }

    private static float GetScaleFactor(Vector3 scale)
    {
        return (Mathf.Abs(scale.x) + Mathf.Abs(scale.y)) * 0.5f;
    }

    private List<Component> CollectHitTargets(Vector3 worldPosition, float effectiveHitRadius)
    {
        var hits = new List<Component>();
        var seen = new HashSet<int>();
        Vector2 shotPoint = worldPosition;

        BirdMover[] birds = FindObjectsByType<BirdMover>(FindObjectsSortMode.None);
        for (int i = 0; i < birds.Length; i++)
        {
            BirdMover bird = birds[i];
            Collider2D collider = bird.GetComponent<Collider2D>();
            Vector2 closestPoint = collider != null ? collider.ClosestPoint(shotPoint) : (Vector2)bird.transform.position;
            float distance = Vector2.Distance(shotPoint, closestPoint);

            if (distance <= effectiveHitRadius && seen.Add(bird.gameObject.GetInstanceID()))
            {
                hits.Add(bird);
            }
        }

        Collider2D[] overlapHits = Physics2D.OverlapCircleAll(shotPoint, effectiveHitRadius);
        for (int i = 0; i < overlapHits.Length; i++)
        {
            ChestAnimationController chest = overlapHits[i].GetComponent<ChestAnimationController>();
            if (chest != null && seen.Add(chest.gameObject.GetInstanceID()))
            {
                hits.Add(chest);
            }
        }

        return hits;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        float effectiveHitRadius;

        if (_scaleHitRadiusWithCrosshair)
        {
            float scaleFactor = Application.isPlaying ? _configuredScaleFactor : GetScaleFactor(transform.localScale);
            effectiveHitRadius = Mathf.Max(_minimumHitRadius, _hitRadius * scaleFactor);
        }
        else
        {
            effectiveHitRadius = _hitRadius;
        }

        Gizmos.DrawWireSphere(transform.position, effectiveHitRadius);
    }
}
