using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Projects.MegaSuperChallengeShot.Scripts
{
    public static class ChickenHuntScenePaths
    {
        public const string GameplayScenePath = "Assets/lab4.unity";
        public const string GameplaySceneName = "lab4";
        public const string MainMenuScenePath = "Assets/Projects/MegaSuperChallengeShot/Scenes/MainMenu.unity";
        public const string MainMenuSceneName = "MainMenu";
    }

    public static class ChickenHuntUiUtility
    {
        private static TMP_FontAsset _cachedFont;

        public static EventSystem EnsureEventSystem()
        {
            EventSystem eventSystem = Object.FindFirstObjectByType<EventSystem>();
            if (eventSystem != null)
            {
                return eventSystem;
            }

            GameObject eventSystemObject = new("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
            return eventSystemObject.GetComponent<EventSystem>();
        }

        public static Canvas EnsureCanvas(string name, int sortingOrder = 0)
        {
            Canvas canvas = Object.FindFirstObjectByType<Canvas>();
            if (canvas == null)
            {
                GameObject canvasObject = new(name, typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
                canvas = canvasObject.GetComponent<Canvas>();
            }

            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = sortingOrder;

            CanvasScaler scaler = canvas.GetComponent<CanvasScaler>();
            if (scaler == null)
            {
                scaler = canvas.gameObject.AddComponent<CanvasScaler>();
            }

            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 0.5f;

            if (canvas.GetComponent<GraphicRaycaster>() == null)
            {
                canvas.gameObject.AddComponent<GraphicRaycaster>();
            }

            return canvas;
        }

        public static RectTransform CreatePanel(
            string name,
            Transform parent,
            Color color,
            Vector2 anchorMin,
            Vector2 anchorMax,
            Vector2 anchoredPosition,
            Vector2 sizeDelta,
            Vector2 pivot)
        {
            GameObject panelObject = new(name, typeof(RectTransform), typeof(Image));
            RectTransform rectTransform = panelObject.GetComponent<RectTransform>();
            rectTransform.SetParent(parent, false);
            SetRect(rectTransform, anchorMin, anchorMax, anchoredPosition, sizeDelta, pivot);

            Image image = panelObject.GetComponent<Image>();
            image.color = color;

            return rectTransform;
        }

        public static TextMeshProUGUI CreateText(
            string name,
            Transform parent,
            string content,
            float fontSize,
            TextAlignmentOptions alignment,
            Vector2 anchorMin,
            Vector2 anchorMax,
            Vector2 anchoredPosition,
            Vector2 sizeDelta,
            Vector2 pivot,
            FontStyles fontStyle = FontStyles.Normal,
            Color? color = null)
        {
            GameObject textObject = new(name, typeof(RectTransform));
            RectTransform rectTransform = textObject.GetComponent<RectTransform>();
            rectTransform.SetParent(parent, false);
            SetRect(rectTransform, anchorMin, anchorMax, anchoredPosition, sizeDelta, pivot);

            TextMeshProUGUI text = textObject.AddComponent<TextMeshProUGUI>();
            text.font = GetFontAsset();
            text.text = content;
            text.fontSize = fontSize;
            text.alignment = alignment;
            text.fontStyle = fontStyle;
            text.color = color ?? Color.white;
            text.raycastTarget = false;
            text.enableWordWrapping = true;
            text.overflowMode = TextOverflowModes.Overflow;

            return text;
        }

        public static Button CreateButton(
            string name,
            Transform parent,
            string label,
            Vector2 anchoredPosition,
            Vector2 sizeDelta)
        {
            Color normalColor = new(0.2f, 0.27f, 0.19f, 0.96f);
            RectTransform buttonRect = CreatePanel(
                name,
                parent,
                normalColor,
                new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f),
                anchoredPosition,
                sizeDelta,
                new Vector2(0.5f, 0.5f));

            Button button = buttonRect.gameObject.AddComponent<Button>();
            button.targetGraphic = buttonRect.GetComponent<Image>();
            ColorBlock colors = button.colors;
            colors.normalColor = normalColor;
            colors.highlightedColor = new Color(0.27f, 0.36f, 0.24f, 1f);
            colors.pressedColor = new Color(0.16f, 0.22f, 0.15f, 1f);
            colors.selectedColor = colors.highlightedColor;
            button.colors = colors;

            CreateText(
                "Label",
                buttonRect,
                label,
                34f,
                TextAlignmentOptions.Center,
                Vector2.zero,
                Vector2.one,
                Vector2.zero,
                new Vector2(-24f, -12f),
                new Vector2(0.5f, 0.5f),
                FontStyles.Bold);

            return button;
        }

        public static void SetRect(
            RectTransform rectTransform,
            Vector2 anchorMin,
            Vector2 anchorMax,
            Vector2 anchoredPosition,
            Vector2 sizeDelta,
            Vector2 pivot)
        {
            rectTransform.anchorMin = anchorMin;
            rectTransform.anchorMax = anchorMax;
            rectTransform.anchoredPosition = anchoredPosition;
            rectTransform.sizeDelta = sizeDelta;
            rectTransform.pivot = pivot;
            rectTransform.localScale = Vector3.one;
        }

        private static TMP_FontAsset GetFontAsset()
        {
            if (_cachedFont != null)
            {
                return _cachedFont;
            }

            _cachedFont = TMP_Settings.defaultFontAsset;
            if (_cachedFont == null)
            {
                _cachedFont = Resources.Load<TMP_FontAsset>("Fonts & Materials/LiberationSans SDF");
            }

            return _cachedFont;
        }
    }
}
