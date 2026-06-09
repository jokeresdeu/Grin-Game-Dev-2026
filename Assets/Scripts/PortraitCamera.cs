using UnityEngine;

[RequireComponent(typeof(Camera))]
public class PortraitCamera : MonoBehaviour
{
    [SerializeField] float targetWorldHeight = 20f;

    Camera cam;

    void Awake()
    {
        cam = GetComponent<Camera>();
        ApplyPortraitViewport();
        SetOrthographicSize();
    }

    void ApplyPortraitViewport()
    {
        float targetAspect = 9f / 16f;
        float windowAspect = (float)Screen.width / Screen.height;
        float scaleWidth   = windowAspect / targetAspect;

        Rect rect;
        if (scaleWidth < 1f)
        {
            float offsetX = (1f - scaleWidth) * 0.5f;
            rect = new Rect(offsetX, 0f, scaleWidth, 1f);
        }
        else
        {
            float scaleHeight = 1f / scaleWidth;
            float offsetY     = (1f - scaleHeight) * 0.5f;
            rect = new Rect(0f, offsetY, 1f, scaleHeight);
        }

        cam.rect = rect;
    }

    void SetOrthographicSize()
    {
        cam.orthographic     = true;
        cam.orthographicSize = targetWorldHeight * 0.5f;
    }
}
