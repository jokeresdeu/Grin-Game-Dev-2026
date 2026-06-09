using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class KunaiOrbitController : MonoBehaviour
{
    public static KunaiOrbitController Instance { get; private set; }

    [Header("Orbit settings")]
    public GameObject kunaiPrefab;
    public int   startKunaiCount = 4;
    public float orbitRadius     = 1.3f;
    [Tooltip("Degrees/second. Positive = counter-clockwise.")]
    public float rotationSpeed   = 180f;

    [Header("Highlight")]
    public Color activeColor  = Color.yellow;   
    public Color normalColor  = Color.white;   

    readonly List<Kunai>          orbitKunai  = new List<Kunai>();
    readonly List<float>          orbitAngles = new List<float>();
    readonly List<SpriteRenderer> orbitSprites = new List<SpriteRenderer>();

    int flyingKunaiCount;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        for (int i = 0; i < startKunaiCount; i++)
            AddKunaiToOrbit(i * (360f / startKunaiCount));

        GameManager.Instance?.SetInitialKunaiCount(startKunaiCount);
    }

    void Update()
    {
        if (GameManager.Instance != null && (!GameManager.Instance.HasStarted || GameManager.Instance.IsGameOver || GameManager.Instance.IsPaused)) return;

        float delta = rotationSpeed * Time.deltaTime;

        for (int i = 0; i < orbitKunai.Count; i++)
        {
            orbitAngles[i] += delta;
            float rad = orbitAngles[i] * Mathf.Deg2Rad;
            Vector2 pos = (Vector2)transform.position
                        + new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * orbitRadius;
            orbitKunai[i].transform.position = pos;

            float offset = orbitKunai[i].spriteAngleOffset;
            orbitKunai[i].transform.rotation = Quaternion.Euler(0f, 0f, orbitAngles[i] + 90f + offset);

            orbitSprites[i].color = (i == 0) ? activeColor : normalColor;
        }

        bool tapped = (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
                   || (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame);
        if (tapped && orbitKunai.Count > 0)
            ThrowFirst();
    }

    void ThrowFirst()
    {
        Kunai thrownKunai = orbitKunai[0];

        float rad = orbitAngles[0] * Mathf.Deg2Rad;
        Vector2 throwDir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));

        orbitSprites[0].color = normalColor;
        orbitKunai.RemoveAt(0);
        orbitAngles.RemoveAt(0);
        orbitSprites.RemoveAt(0);

        flyingKunaiCount++;
        GameManager.Instance?.LoseKunai();
        thrownKunai.Throw(throwDir);
        CubeSpawner.Instance?.MoveCubeDown();
    }

    void AddKunaiToOrbit(float angleDeg)
    {
        GameObject go = Instantiate(kunaiPrefab, transform.position, Quaternion.identity);
        Kunai k = go.GetComponent<Kunai>();
        k.Init(this, transform);
        orbitKunai.Add(k);
        orbitAngles.Add(angleDeg);
        orbitSprites.Add(go.GetComponent<SpriteRenderer>());
    }

    public void AddNewKunai()
    {
        if (orbitKunai.Count >= startKunaiCount) return;

        float spawnAngle = orbitAngles.Count > 0
            ? orbitAngles[orbitAngles.Count - 1] + 120f
            : 180f;
        AddKunaiToOrbit(spawnAngle);
        GameManager.Instance?.GainKunai();
    }

    public void NotifyKunaiLost(Kunai k)
    {
        flyingKunaiCount = Mathf.Max(0, flyingKunaiCount - 1);
        CheckAllKunaiGone();
    }

    public void NotifyKunaiStuck(Kunai k)
    {
        flyingKunaiCount = Mathf.Max(0, flyingKunaiCount - 1);
        CheckAllKunaiGone();
    }

    void CheckAllKunaiGone()
    {
        if (orbitKunai.Count == 0 && flyingKunaiCount <= 0)
            GameManager.Instance?.NotifyAllKunaiGone();
    }
}
