using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using RPG;
using System.IO;

public class SetupLab3RPG_Procedural
{
    [MenuItem("Tools/Setup Procedural RPG")]
    public static void Setup()
    {
        string prefabsDir = "Assets/Prefabs";
        if (!Directory.Exists(prefabsDir))
        {
            Directory.CreateDirectory(prefabsDir);
            AssetDatabase.Refresh();
        }

        // 1. Create Prefabs from existing Coin_0 and Hazard_0
        GameObject coin = GameObject.Find("Coin_0");
        GameObject hazard = GameObject.Find("Hazard_0");

        GameObject coinPrefab = null;
        GameObject hazardPrefab = null;

        if (coin != null)
        {
            if (coin.GetComponent<MoveLeft>() == null)
                coin.AddComponent<MoveLeft>();
            
            string path = prefabsDir + "/Coin.prefab";
            coinPrefab = PrefabUtility.SaveAsPrefabAsset(coin, path);
        }

        if (hazard != null)
        {
            if (hazard.GetComponent<MoveLeft>() == null)
                hazard.AddComponent<MoveLeft>();
            
            string path = prefabsDir + "/Hazard.prefab";
            hazardPrefab = PrefabUtility.SaveAsPrefabAsset(hazard, path);
        }

        // 2. Delete ALL existing static coins and hazards
        for (int i = 0; i < 20; i++) // Just brute force loop
        {
            GameObject c = GameObject.Find($"Coin_{i}");
            if (c != null) GameObject.DestroyImmediate(c);

            GameObject h = GameObject.Find($"Hazard_{i}");
            if (h != null) GameObject.DestroyImmediate(h);
        }

        // 3. Attach Spawner to GameManager
        var gm = GameObject.Find("RPGGameManager");
        if (gm != null)
        {
            var spawner = gm.GetComponent<ProceduralSpawner>();
            if (spawner == null)
                spawner = gm.AddComponent<ProceduralSpawner>();
            
            // If we successfully made prefabs (or already had them), assign them
            if (coinPrefab == null) coinPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Coin.prefab");
            if (hazardPrefab == null) hazardPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Hazard.prefab");

            // spawner.SetPrefabs(coinPrefab, hazardPrefab);
            
            // Also ensure the SerializedProperty is set, because SetPrefabs only sets private fields and might get overwritten on serialization
            var so = new SerializedObject(spawner);
            // so.FindProperty("_coinPrefab").objectReferenceValue = coinPrefab;
            // so.FindProperty("_hazardPrefab").objectReferenceValue = hazardPrefab;
            so.ApplyModifiedProperties();
        }
        else
        {
            Debug.LogError("RPGGameManager not found!");
        }

        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
        Debug.Log("Procedural setup completed!");
    }
}
