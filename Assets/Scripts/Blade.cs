using UnityEngine;

[RequireComponent(typeof(EdgeCollider2D))]
[RequireComponent(typeof(TrailRenderer))]
public class Blade : MonoBehaviour
{
    [SerializeField] private float minCutDistance = 0.1f;

    private Camera mainCamera;
    private EdgeCollider2D edgeCollider;
    private TrailRenderer trailRenderer;

    private Vector2 previousPosition;
    private bool isCutting;

    private void Awake()
    {
        mainCamera = Camera.main;
        edgeCollider = GetComponent<EdgeCollider2D>();
        trailRenderer = GetComponent<TrailRenderer>();

        edgeCollider.enabled = false;
        trailRenderer.enabled = false;
    }

    private void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver)
        {
            StopCutting();
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            StartCutting();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            StopCutting();
        }

        if (isCutting)
        {
            ContinueCutting();
        }
    }

    private void StartCutting()
    {
        isCutting = true;

        Vector2 mousePosition = GetMouseWorldPosition();
        previousPosition = mousePosition;
        transform.position = mousePosition;

        edgeCollider.enabled = true;
        trailRenderer.enabled = true;
        trailRenderer.Clear();
    }

    private void StopCutting()
    {
        isCutting = false;

        edgeCollider.enabled = false;
        trailRenderer.enabled = false;
    }

    private void ContinueCutting()
    {
        Vector2 currentPosition = GetMouseWorldPosition();
        transform.position = currentPosition;

        float distance = Vector2.Distance(previousPosition, currentPosition);

        if (distance < minCutDistance)
            return;

        Vector2 localPreviousPosition = transform.InverseTransformPoint(previousPosition);
        Vector2 localCurrentPosition = transform.InverseTransformPoint(currentPosition);

        edgeCollider.SetPoints(new System.Collections.Generic.List<Vector2>
        {
            localPreviousPosition,
            localCurrentPosition
        });

        previousPosition = currentPosition;
    }

    private Vector2 GetMouseWorldPosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = -mainCamera.transform.position.z;
        return mainCamera.ScreenToWorldPoint(mousePosition);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Fruit fruit = collision.GetComponent<Fruit>();

        if (fruit != null)
        {
            fruit.Slice();
        }
    }
}