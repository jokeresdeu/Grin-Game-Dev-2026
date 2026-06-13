using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class SetupLab3RPG_Fix
{
    [MenuItem("Tools/Fix Lab 3 RPG Bounds")]
    public static void FixScene()
    {
        GameObject human = GameObject.Find("Human");
        if (human == null)
        {
            Debug.LogError("Could not find 'Human' object!");
            return;
        }

        Vector3 center = human.transform.position;

        // 1. Relocate Coins and Hazards
        for (int i = 0; i < 5; i++)
        {
            GameObject coin = GameObject.Find($"Coin_{i}");
            if (coin != null)
                coin.transform.position = center + new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), 0);
        }

        for (int i = 0; i < 3; i++)
        {
            GameObject hazard = GameObject.Find($"Hazard_{i}");
            if (hazard != null)
                hazard.transform.position = center + new Vector3(Random.Range(-4f, 4f), Random.Range(-4f, 4f), 0);
        }

        // 2. Create Boundaries
        GameObject boundariesObj = GameObject.Find("Boundaries");
        if (boundariesObj == null)
            boundariesObj = new GameObject("Boundaries");

        // The layer must match the Player's _obstacleLayer. By default let's use the Default layer (0) or Ground.
        // Assuming Default works for the Physics2D.Raycast if the mask is set to Everything or Default.
        // Usually, tilemaps are on Default or Ground. We'll set it to Default.
        
        // Clear old bounds if any
        foreach (Transform child in boundariesObj.transform)
        {
            GameObject.DestroyImmediate(child.gameObject);
        }

        float width = 12f;
        float height = 10f;
        float thickness = 2f;

        // Top
        CreateWall(boundariesObj.transform, "Wall_Top", center + new Vector3(0, height/2, 0), new Vector2(width, thickness));
        // Bottom
        CreateWall(boundariesObj.transform, "Wall_Bottom", center + new Vector3(0, -height/2, 0), new Vector2(width, thickness));
        // Left
        CreateWall(boundariesObj.transform, "Wall_Left", center + new Vector3(-width/2, 0, 0), new Vector2(thickness, height));
        // Right
        CreateWall(boundariesObj.transform, "Wall_Right", center + new Vector3(width/2, 0, 0), new Vector2(thickness, height));

        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
        Debug.Log("Lab 3 RPG Bounds Fixed!");
    }

    private static void CreateWall(Transform parent, string name, Vector3 position, Vector2 size)
    {
        GameObject wall = new GameObject(name);
        wall.transform.SetParent(parent);
        wall.transform.position = position;
        var col = wall.AddComponent<BoxCollider2D>();
        col.size = size;
        // Don't make them triggers. The CanMove script uses Raycast which will hit standard colliders.
    }
}
