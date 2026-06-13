using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using RPG;

public class SetupLab3RPG
{
    [MenuItem("Tools/Setup Lab 3 RPG")]
    public static void SetupScene()
    {
        string scenePath = "Assets/Scenes/RPG.unity";
        EditorSceneManager.OpenScene(scenePath);

        if (GameObject.Find("RPGGameManager") != null) 
        {
            Debug.Log("Already setup!");
            return;
        }

        GameObject gmObj = new GameObject("RPGGameManager");
        var gm = gmObj.AddComponent<RPGGameManager>();

        GameObject canvasObj = new GameObject("Canvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasObj.AddComponent<CanvasScaler>();
        canvasObj.AddComponent<GraphicRaycaster>();

        GameObject scoreObj = new GameObject("ScoreText");
        scoreObj.transform.SetParent(canvasObj.transform, false);
        var scoreText = scoreObj.AddComponent<TextMeshProUGUI>();
        scoreText.text = "Score: 0";
        scoreText.fontSize = 36;
        scoreText.color = Color.white;
        RectTransform scoreRect = scoreObj.GetComponent<RectTransform>();
        scoreRect.anchorMin = new Vector2(0, 1);
        scoreRect.anchorMax = new Vector2(0, 1);
        scoreRect.pivot = new Vector2(0, 1);
        scoreRect.anchoredPosition = new Vector2(20, -20);
        scoreRect.sizeDelta = new Vector2(300, 50);

        GameObject sliderObj = DefaultControls.CreateSlider(new DefaultControls.Resources());
        sliderObj.name = "HealthSlider";
        sliderObj.transform.SetParent(canvasObj.transform, false);
        RectTransform sliderRect = sliderObj.GetComponent<RectTransform>();
        sliderRect.anchorMin = new Vector2(0.5f, 0);
        sliderRect.anchorMax = new Vector2(0.5f, 0);
        sliderRect.pivot = new Vector2(0.5f, 0);
        sliderRect.anchoredPosition = new Vector2(0, 30);
        sliderRect.sizeDelta = new Vector2(400, 30);
        Slider slider = sliderObj.GetComponent<Slider>();
        slider.value = 1f;
        slider.interactable = false;
        var fillArea = sliderObj.transform.Find("Fill Area/Fill").GetComponent<Image>();
        fillArea.color = Color.red;

        GameObject panelObj = new GameObject("GameOverPanel");
        panelObj.transform.SetParent(canvasObj.transform, false);
        var panelImage = panelObj.AddComponent<Image>();
        panelImage.color = new Color(0, 0, 0, 0.8f);
        RectTransform panelRect = panelObj.GetComponent<RectTransform>();
        panelRect.anchorMin = Vector2.zero;
        panelRect.anchorMax = Vector2.one;
        panelRect.sizeDelta = Vector2.zero;
        panelRect.anchoredPosition = Vector2.zero;

        GameObject goTextObj = new GameObject("GameOverText");
        goTextObj.transform.SetParent(panelObj.transform, false);
        var goText = goTextObj.AddComponent<TextMeshProUGUI>();
        goText.text = "GAME OVER";
        goText.fontSize = 72;
        goText.color = Color.red;
        goText.alignment = TextAlignmentOptions.Center;
        RectTransform goTextRect = goTextObj.GetComponent<RectTransform>();
        goTextRect.anchorMin = new Vector2(0.5f, 0.5f);
        goTextRect.anchorMax = new Vector2(0.5f, 0.5f);
        goTextRect.anchoredPosition = new Vector2(0, 50);
        goTextRect.sizeDelta = new Vector2(500, 100);

        GameObject btnObj = DefaultControls.CreateButton(new DefaultControls.Resources());
        btnObj.name = "RestartButton";
        btnObj.transform.SetParent(panelObj.transform, false);
        RectTransform btnRect = btnObj.GetComponent<RectTransform>();
        btnRect.anchorMin = new Vector2(0.5f, 0.5f);
        btnRect.anchorMax = new Vector2(0.5f, 0.5f);
        btnRect.anchoredPosition = new Vector2(0, -50);
        btnRect.sizeDelta = new Vector2(200, 60);

        var btnText = btnObj.GetComponentInChildren<Text>();
        btnText.text = "Restart";
        btnText.fontSize = 24;

        Button btn = btnObj.GetComponent<Button>();
        UnityEditor.Events.UnityEventTools.AddPersistentListener(btn.onClick, gm.RestartGame);

        panelObj.SetActive(false);

        SerializedObject so = new SerializedObject(gm);
        so.FindProperty("_scoreText").objectReferenceValue = scoreText;
        so.FindProperty("_healthSlider").objectReferenceValue = slider;
        so.FindProperty("_gameOverPanel").objectReferenceValue = panelObj;
        so.ApplyModifiedProperties();

        GameObject player = GameObject.Find("Player");
        Vector3 playerPos = player != null ? player.transform.position : Vector3.zero;

        for (int i = 0; i < 5; i++)
        {
            GameObject coin = new GameObject($"Coin_{i}");
            coin.transform.position = playerPos + new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), 0);
            var sr = coin.AddComponent<SpriteRenderer>();
            sr.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Knob.psd");
            sr.color = Color.yellow;
            var col = coin.AddComponent<BoxCollider2D>();
            col.isTrigger = true;
            coin.AddComponent<Collectible>();
        }

        for (int i = 0; i < 3; i++)
        {
            GameObject hazard = new GameObject($"Hazard_{i}");
            hazard.transform.position = playerPos + new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), 0);
            var sr = hazard.AddComponent<SpriteRenderer>();
            sr.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
            sr.color = new Color(1f, 0.5f, 0f, 0.7f);
            hazard.transform.localScale = new Vector3(3, 3, 1);
            var col = hazard.AddComponent<BoxCollider2D>();
            col.isTrigger = true;
            hazard.AddComponent<Hazard>();
        }

        if (player != null)
        {
            var rb = player.GetComponent<Rigidbody2D>();
            if (rb == null)
            {
                rb = player.AddComponent<Rigidbody2D>();
                rb.bodyType = RigidbodyType2D.Kinematic;
            }
        }

        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
        Debug.Log("Lab 3 RPG Setup Complete!");
    }
}
