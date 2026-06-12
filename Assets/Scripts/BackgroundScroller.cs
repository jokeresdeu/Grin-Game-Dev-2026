using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    public float scrollSpeed = 0.5f;
    private Renderer bgRenderer;

    void Awake()
    {
        bgRenderer = GetComponent<Renderer>();
    }

    void Update()
    {
        float offset = Time.time * scrollSpeed;
        bgRenderer.material.mainTextureOffset = new Vector2(offset, 0);
    }
}