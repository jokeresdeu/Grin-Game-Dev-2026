using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class WorldModeManager : MonoBehaviour
{
    public static WorldModeManager Instance { get; private set; }

    [Header("Mode")]
    [SerializeField] private WorldMode currentMode = WorldMode.Light;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI modeText;

    [Header("Camera")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Color lightBackground = new Color(0.85f, 0.9f, 1f);
    [SerializeField] private Color darkBackground = new Color(0.08f, 0.05f, 0.18f);

    public WorldMode CurrentMode => currentMode;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        ApplyMode();
    }

    private void Update()
    {
        if (GameManager.Instance != null && !GameManager.Instance.IsGameRunning)
        {
            return;
        }

        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        bool touchPressed = Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began;

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0) || touchPressed)
        {
            ToggleMode();
        }
    }

    private void ToggleMode()
    {
        currentMode = currentMode == WorldMode.Light ? WorldMode.Dark : WorldMode.Light;
        ApplyMode();
    }

    public void RefreshMode()
    {
        ApplyMode();
    }

    private void ApplyMode()
    {
        ModeObject[] modeObjects = FindObjectsByType<ModeObject>(FindObjectsSortMode.None);

        foreach (ModeObject modeObject in modeObjects)
        {
            if (modeObject != null)
            {
                modeObject.ApplyMode(currentMode);
            }
        }

        if (modeText != null)
        {
            modeText.text = currentMode == WorldMode.Light ? "MODE: LIGHT" : "MODE: DARK";
        }

        if (mainCamera != null)
        {
            mainCamera.backgroundColor = currentMode == WorldMode.Light ? lightBackground : darkBackground;
        }
    }
}