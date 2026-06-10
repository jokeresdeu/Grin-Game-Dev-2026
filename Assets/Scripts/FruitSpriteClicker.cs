using UnityEngine;

public class FruitSpriteClicker : MonoBehaviour
{
    [Header("Scale")]
    [SerializeField] private float pressedScaleMultiplier = 0.85f;
    [SerializeField] private float scaleSpeed = 20f;

    private Camera mainCamera;
    private Collider2D fruitCollider;
    private Vector3 normalScale;
    private Vector3 targetScale;

    private void Awake()
    {
        mainCamera = Camera.main;
        fruitCollider = GetComponent<Collider2D>();

        normalScale = transform.localScale;
        targetScale = normalScale;
    }

    private void Update()
    {
        transform.localScale = Vector3.Lerp(
            transform.localScale,
            targetScale,
            Time.deltaTime * scaleSpeed
        );

        if (Input.GetMouseButtonDown(0))
        {
            if (IsMouseOnFruit())
            {
                targetScale = normalScale * pressedScaleMultiplier;

                if (GameManager.Instance != null)
                {
                    GameManager.Instance.Click();
                }
                else
                {
                    Debug.LogError("GameManager.Instance is null.");
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            targetScale = normalScale;
        }
    }

    private bool IsMouseOnFruit()
    {
        if (mainCamera == null)
        {
            Debug.LogError("Main Camera was not found. The camera must have the MainCamera tag.");
            return false;
        }

        if (fruitCollider == null)
        {
            Debug.LogError("Fruit does not have a Collider2D component.");
            return false;
        }

        Vector3 mouseScreenPosition = Input.mousePosition;
        mouseScreenPosition.z = Mathf.Abs(mainCamera.transform.position.z);

        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(mouseScreenPosition);
        Vector2 mouseWorldPosition2D = new Vector2(mouseWorldPosition.x, mouseWorldPosition.y);

        return fruitCollider.OverlapPoint(mouseWorldPosition2D);
    }
}