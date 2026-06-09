using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    public static CubeSpawner Instance { get; private set; }

    [Header("References")]
    public GameObject cubePrefab1;
    public GameObject cubePrefab2;
    public Transform  ninja;

    [Header("Spawn settings")]
    public float spawnYMin   = 4f;
    public float spawnYMax   = 6f;
    public float spawnXRange = 1.5f;
    [Min(0.3f)]
    public float minSpacing  = 1.0f;

    readonly List<TargetCube> activeCubes = new List<TargetCube>();

    void Awake() => Instance = this;

    public void BeginFirstWave() => SpawnWave();

    public void MoveCubeDown()
    {
        foreach (TargetCube cube in new List<TargetCube>(activeCubes))
            if (cube != null) cube.StepDown();
    }

    public void OnCubeDestroyed(TargetCube cube)
    {
        activeCubes.Remove(cube);
        if (activeCubes.Count == 0)
            Invoke(nameof(SpawnWave), 0.5f);
    }

    void SpawnWave()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver) return;

        int count    = Random.Range(1, 5); // від 1 до 4
        float ninjaX = ninja != null ? ninja.position.x : 0f;

        float step   = count > 1
            ? Mathf.Max(minSpacing, spawnXRange * 2f / (count - 1))
            : 0f;
        float startX = ninjaX - step * (count - 1) / 2f;

        for (int i = 0; i < count; i++)
        {
            int type = Random.Range(1, 3);
            GameObject prefab = type == 1 ? cubePrefab1 : cubePrefab2;
            if (prefab == null) { Debug.LogWarning("CubeSpawner: prefab not assigned!"); continue; }

            float x = startX + step * i;
            float y = Random.Range(spawnYMin, spawnYMax);

            GameObject go = Instantiate(prefab, new Vector3(x, y, 0f), Quaternion.identity);
            TargetCube cube = go.GetComponent<TargetCube>();
            cube.SetHp(type);
            cube.scoreValue = type;
            activeCubes.Add(cube);
        }
    }
}
