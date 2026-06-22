using System.IO;
using Projects.TowerDefense.Scripts;
using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Projects.TowerDefense.EditorTools
{
    /// <summary>
    /// Edit-time tool that builds the Tower Defense main-menu and game scenes with a fully
    /// inspectable GameObject hierarchy, builds the enemy/tower prefabs, wires every
    /// [SerializeField] reference, registers the scenes in Build Settings, and saves them.
    /// This is NOT runtime code: it runs once from the Unity menu. Mirrors the proven
    /// OrbitGunnerSceneBuilder workflow.
    ///
    /// Menu: TowerDefense ▸ Build Scenes
    /// </summary>
    public static class TowerDefenseSceneBuilder
    {
        private const string SceneDir = "Assets/Projects/TowerDefense/Scenes";
        private const string PrefabDir = "Assets/Projects/TowerDefense/Prefabs";
        private const string MatDir = "Assets/Projects/TowerDefense/Materials";
        private const string SpriteDir = "Assets/Projects/TowerDefense/Sprites";
        private const string MenuScenePath = SceneDir + "/TowerDefense_MainMenu.unity";
        private const string GameScenePath = SceneDir + "/TowerDefense_Game.unity";

        private static readonly Color Background = new Color(0.07f, 0.09f, 0.12f);
        private static readonly Color PanelDark = new Color(0.03f, 0.04f, 0.07f, 0.90f);
        private static readonly Color Dim = new Color(1f, 1f, 1f, 0.55f);
        private static readonly Color BarBg = new Color(0.12f, 0.12f, 0.16f, 0.92f);
        private static readonly Color Gold = new Color(1f, 0.85f, 0.4f);
        private static readonly Color Green = new Color(0.2f, 0.7f, 0.45f);
        private static readonly Color Blue = new Color(0.25f, 0.5f, 0.8f);
        private static readonly Color Red = new Color(0.72f, 0.32f, 0.34f);
        private static readonly Color Grey = new Color(0.4f, 0.4f, 0.5f);

        [MenuItem("TowerDefense/Build Scenes")]
        public static void BuildAll()
        {
            BuildMenuScene();
            BuildGameScene();

            EditorBuildSettings.scenes = new[]
            {
                new EditorBuildSettingsScene(MenuScenePath, true),
                new EditorBuildSettingsScene(GameScenePath, true)
            };

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorSceneManager.OpenScene(MenuScenePath);
            Debug.Log("TowerDefense: both scenes built, prefabs saved, and added to Build Settings.");
        }

        [MenuItem("TowerDefense/Build Main Menu Scene")]
        public static void BuildMenuScene()
        {
            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            CreateCamera();
            CreateEventSystem();
            Canvas canvas = CreateCanvas("MenuCanvas");

            Text(canvas.transform, "Title", "ЗАХИСТ ЦИТАДЕЛІ", 104f, TextAlignmentOptions.Center,
                C(0.5f, 0.5f), C(0.5f, 0.5f), C(0.5f, 0.5f), C(0f, 400f), C(1700f, 170f), new Color(0.6f, 0.85f, 1f));

            Text(canvas.transform, "Subtitle",
                "Оберіть расу та захистіть цитадель від трьох рівнів ворогів.",
                30f, TextAlignmentOptions.Center,
                C(0.5f, 0.5f), C(0.5f, 0.5f), C(0.5f, 0.5f), C(0f, 295f), C(1400f, 60f), new Color(1f, 1f, 1f, 0.85f));

            Text(canvas.transform, "ChooseLabel", "ОБЕРІТЬ РАСУ", 36f, TextAlignmentOptions.Center,
                C(0.5f, 0.5f), C(0.5f, 0.5f), C(0.5f, 0.5f), C(0f, 215f), C(700f, 50f), Dim);

            var raceButtons = new Button[RaceConfig.All.Length];
            float[] xs = { -470f, 0f, 470f };
            for (int i = 0; i < RaceConfig.All.Length; i++)
            {
                RaceConfig rc = RaceConfig.For(RaceConfig.All[i]);
                raceButtons[i] = Btn(canvas.transform, "Race_" + rc.Id, rc.DisplayName,
                    C(xs[i], 110f), C(380f, 130f), rc.Color, 42f);
            }

            TMP_Text raceName = Text(canvas.transform, "RaceName", "", 44f, TextAlignmentOptions.Center,
                C(0.5f, 0.5f), C(0.5f, 0.5f), C(0.5f, 0.5f), C(0f, -10f), C(900f, 56f), Color.white);
            TMP_Text blurb = Text(canvas.transform, "Blurb", "", 28f, TextAlignmentOptions.Center,
                C(0.5f, 0.5f), C(0.5f, 0.5f), C(0.5f, 0.5f), C(0f, -95f), C(1250f, 110f), new Color(1f, 1f, 1f, 0.8f));

            Button play = Btn(canvas.transform, "PlayButton", "ГРАТИ", C(0f, -245f), C(440f, 95f), Green, 40f);
            Button quit = Btn(canvas.transform, "QuitButton", "Вийти", C(0f, -365f), C(360f, 72f), Red, 32f);

            Text(canvas.transform, "Controls",
                "Керування: миша — будувати й покращувати вежі   ·   Esc — пауза",
                24f, TextAlignmentOptions.Center,
                C(0.5f, 0f), C(0.5f, 0f), C(0.5f, 0f), C(0f, 60f), C(1400f, 40f), new Color(1f, 1f, 1f, 0.45f));

            var controller = new GameObject("MainMenuController").AddComponent<MainMenuController>();
            WireArray(controller, "_raceButtons", raceButtons);
            Wire(controller, "_raceNameText", raceName);
            Wire(controller, "_blurbText", blurb);
            Wire(controller, "_playButton", play);
            Wire(controller, "_quitButton", quit);

            SaveScene(scene, MenuScenePath);
        }

        [MenuItem("TowerDefense/Build Game Scene")]
        public static void BuildGameScene()
        {
            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            CreateCamera();
            CreateEventSystem();
            Material lineMat = EnsureLineMaterial();

            // --- Systems ---
            var systems = new GameObject("Systems");
            systems.AddComponent<GameManager>();
            systems.AddComponent<ResourceManager>();
            LevelManager levelManager = systems.AddComponent<LevelManager>();
            EnemySpawner spawner = systems.AddComponent<EnemySpawner>();
            BuildManager build = systems.AddComponent<BuildManager>();

            // --- Base (the Citadel): empty root (scale 1) with sized children, so child
            //     scales don't multiply with a scaled parent. ---
            var baseGo = new GameObject("Base");
            baseGo.transform.position = LevelLibrary.BasePosition;
            GameObject baseBody = SizedCircle("Body", new Color(0.88f, 0.8f, 0.55f), LevelLibrary.BasePosition, 1.0f, 1, baseGo.transform);
            SizedCircle("Keep", new Color(1f, 0.96f, 0.78f), LevelLibrary.BasePosition, 0.44f, 2, baseGo.transform);
            BaseHealth baseHealth = baseGo.AddComponent<BaseHealth>();
            Wire(baseHealth, "_renderer", baseBody.GetComponent<SpriteRenderer>());

            // --- Containers ---
            var enemies = new GameObject("Enemies");
            var towers = new GameObject("Towers");

            // --- Levels (paths + slots), only level 1 active ---
            LevelConfig[] levels = LevelLibrary.Build();
            var levelContainers = new GameObject[levels.Length];
            var paths = new EnemyPath[levels.Length];
            for (int i = 0; i < levels.Length; i++)
            {
                var container = new GameObject("Level" + (i + 1));
                paths[i] = BuildPath(container.transform, levels[i].Waypoints, lineMat);
                foreach (Vector3 slotPos in levels[i].SlotPositions)
                    BuildSlot(container.transform, slotPos);
                container.SetActive(i == 0);
                levelContainers[i] = container;
            }

            // --- Selection indicators (sized at runtime, hidden initially) ---
            SpriteRenderer rangeRing = MakeSprite("RangeRing", Circle(), new Color(0.4f, 0.8f, 1f, 0.12f), Vector3.zero, 0, null);
            rangeRing.enabled = false;
            SpriteRenderer slotHighlight = MakeSprite("SlotHighlight", Circle(), new Color(1f, 0.95f, 0.5f, 0.30f), Vector3.zero, 1, null);
            slotHighlight.enabled = false;

            // --- Prefabs (commit GUIDs before referencing) ---
            Enemy enemyPrefab = BuildEnemyPrefab();
            Tower towerPrefab = BuildTowerPrefab();
            AssetDatabase.SaveAssets();

            // --- Wire systems ---
            Wire(spawner, "_enemyPrefab", enemyPrefab);
            Wire(spawner, "_container", enemies.transform);
            WireArray(levelManager, "_levelContainers", levelContainers);
            WireArray(levelManager, "_paths", paths);
            Wire(build, "_towerContainer", towers.transform);
            Wire(build, "_towerPrefab", towerPrefab);
            Wire(build, "_rangeRing", rangeRing);
            Wire(build, "_slotHighlight", slotHighlight);

            // --- HUD canvas ---
            Canvas canvas = CreateCanvas("HUDCanvas");

            TMP_Text goldText = Text(canvas.transform, "Gold", "Золото: 100", 42f, TextAlignmentOptions.TopLeft,
                C(0f, 1f), C(0f, 1f), C(0f, 1f), C(40f, -28f), C(560f, 60f), Gold);
            TMP_Text levelText = Text(canvas.transform, "Level", "Рівень 1/3 · Хвиля 1/3", 36f, TextAlignmentOptions.TopRight,
                C(1f, 1f), C(1f, 1f), C(1f, 1f), C(-40f, -28f), C(760f, 60f), Color.white);

            Text(canvas.transform, "HpLabel", "ЦИТАДЕЛЬ", 26f, TextAlignmentOptions.Center,
                C(0.5f, 1f), C(0.5f, 1f), C(0.5f, 1f), C(0f, -26f), C(360f, 36f), Dim);
            Image hpFill = Bar(canvas.transform, "HpBar", C(0.5f, 1f), C(0.5f, 1f), C(0.5f, 1f),
                C(0f, -76f), C(520f, 34f), BarBg, new Color(1f, 0.35f, 0.4f));
            TMP_Text hpText = Text(canvas.transform, "HpText", "20/20", 22f, TextAlignmentOptions.Center,
                C(0.5f, 1f), C(0.5f, 1f), C(0.5f, 1f), C(0f, -76f), C(520f, 34f), Color.white);

            TMP_Text banner = Text(canvas.transform, "Banner", "", 110f, TextAlignmentOptions.Center,
                C(0.5f, 0.5f), C(0.5f, 0.5f), C(0.5f, 0.5f), C(0f, 70f), C(1500f, 180f), new Color(1f, 0.95f, 0.7f, 0f));

            Text(canvas.transform, "Hint",
                "Клікніть по світлому майданчику, щоб поставити чи покращити вежу   ·   Esc — пауза",
                24f, TextAlignmentOptions.BottomLeft,
                C(0f, 0f), C(0f, 0f), C(0f, 0f), C(40f, 210f), C(1100f, 40f), Dim);

            HUDController hud = canvas.gameObject.AddComponent<HUDController>();
            Wire(hud, "_goldText", goldText);
            Wire(hud, "_hpText", hpText);
            Wire(hud, "_hpFill", hpFill);
            Wire(hud, "_levelText", levelText);
            Wire(hud, "_bannerText", banner);

            // --- Build bar (bottom) ---
            Image bar = Box(canvas.transform, "BuildBar", C(0.5f, 0f), C(0.5f, 0f), C(0.5f, 0f),
                C(0f, 28f), C(1180f, 160f), new Color(0.06f, 0.07f, 0.10f, 0.94f));

            RectTransform buildGroup = Rect(bar.transform, "BuildGroup", C(0.5f, 0.5f), C(0.5f, 0.5f), C(0.5f, 0.5f),
                C(0f, 0f), C(1180f, 160f));
            var buildButtons = new Button[TowerConfig.Roles.Length];
            var buildLabels = new TMP_Text[TowerConfig.Roles.Length];
            float[] bx = { -390f, 0f, 390f };
            for (int i = 0; i < TowerConfig.Roles.Length; i++)
            {
                buildButtons[i] = Btn(buildGroup, "BuildBtn_" + i, "Вежа", C(bx[i], 0f), C(360f, 120f), Blue, 28f);
                buildLabels[i] = buildButtons[i].GetComponentInChildren<TMP_Text>();
            }

            RectTransform upgradeGroup = Rect(bar.transform, "UpgradeGroup", C(0.5f, 0.5f), C(0.5f, 0.5f), C(0.5f, 0.5f),
                C(0f, 0f), C(1180f, 160f));
            TMP_Text towerInfo = Text(upgradeGroup, "TowerInfo", "", 32f, TextAlignmentOptions.Center,
                C(0.5f, 0.5f), C(0.5f, 0.5f), C(0.5f, 0.5f), C(0f, 45f), C(1000f, 44f), Color.white);
            Button upgradeButton = Btn(upgradeGroup, "UpgradeButton", "Покращити", C(-270f, -28f), C(440f, 86f), Green, 28f);
            TMP_Text upgradeLabel = upgradeButton.GetComponentInChildren<TMP_Text>();
            Button sellButton = Btn(upgradeGroup, "SellButton", "Продати", C(270f, -28f), C(440f, 86f), Red, 28f);
            TMP_Text sellLabel = sellButton.GetComponentInChildren<TMP_Text>();
            bar.gameObject.SetActive(false);

            Wire(build, "_buildBar", bar.gameObject);
            Wire(build, "_buildGroup", buildGroup.gameObject);
            Wire(build, "_upgradeGroup", upgradeGroup.gameObject);
            WireArray(build, "_buildButtons", buildButtons);
            WireArray(build, "_buildLabels", buildLabels);
            Wire(build, "_towerInfo", towerInfo);
            Wire(build, "_upgradeButton", upgradeButton);
            Wire(build, "_upgradeLabel", upgradeLabel);
            Wire(build, "_sellButton", sellButton);
            Wire(build, "_sellLabel", sellLabel);

            // --- Pause panel ---
            Transform pause = Panel(canvas.transform, "PausePanel", PanelDark);
            Text(pause, "Title", "ПАУЗА", 86f, TextAlignmentOptions.Center,
                C(0.5f, 0.5f), C(0.5f, 0.5f), C(0.5f, 0.5f), C(0f, 220f), C(800f, 120f), Color.white);
            Button resume = Btn(pause, "ResumeButton", "Продовжити", C(0f, 95f), C(400f, 72f), Green);
            Button pRestart = Btn(pause, "RestartButton", "Почати рівень спочатку", C(0f, 12f), C(400f, 72f), Blue);
            Button pMenu = Btn(pause, "MenuButton", "Головне меню", C(0f, -71f), C(400f, 72f), Grey);
            Button pQuit = Btn(pause, "QuitButton", "Вийти", C(0f, -154f), C(400f, 72f), Red);
            pause.gameObject.SetActive(false);

            PauseMenuController pauseCtrl = canvas.gameObject.AddComponent<PauseMenuController>();
            Wire(pauseCtrl, "_panel", pause.gameObject);
            Wire(pauseCtrl, "_resumeButton", resume);
            Wire(pauseCtrl, "_restartButton", pRestart);
            Wire(pauseCtrl, "_menuButton", pMenu);
            Wire(pauseCtrl, "_quitButton", pQuit);

            // --- Win panel ---
            Transform win = Panel(canvas.transform, "WinPanel", new Color(0.04f, 0.08f, 0.05f, 0.92f));
            Text(win, "Title", "ПЕРЕМОГА!", 92f, TextAlignmentOptions.Center,
                C(0.5f, 0.5f), C(0.5f, 0.5f), C(0.5f, 0.5f), C(0f, 230f), C(1000f, 130f), new Color(0.6f, 1f, 0.7f));
            Text(win, "Subtitle", "Цитадель вистояла усі три рівні. Чудова оборона!", 36f, TextAlignmentOptions.Center,
                C(0.5f, 0.5f), C(0.5f, 0.5f), C(0.5f, 0.5f), C(0f, 110f), C(1200f, 60f), Color.white);
            Button winRestart = Btn(win, "WinRestartButton", "Грати ще", C(0f, -30f), C(420f, 80f), Green);
            Button winMenu = Btn(win, "WinMenuButton", "Головне меню", C(0f, -125f), C(420f, 72f), Grey);
            win.gameObject.SetActive(false);

            // --- Lose panel ---
            Transform lose = Panel(canvas.transform, "LosePanel", new Color(0.10f, 0.03f, 0.04f, 0.92f));
            Text(lose, "Title", "ПОРАЗКА", 92f, TextAlignmentOptions.Center,
                C(0.5f, 0.5f), C(0.5f, 0.5f), C(0.5f, 0.5f), C(0f, 230f), C(1000f, 130f), new Color(1f, 0.5f, 0.5f));
            TMP_Text loseInfo = Text(lose, "LoseInfo", "Цитадель впала.", 36f, TextAlignmentOptions.Center,
                C(0.5f, 0.5f), C(0.5f, 0.5f), C(0.5f, 0.5f), C(0f, 110f), C(1200f, 60f), Color.white);
            Button loseRestart = Btn(lose, "LoseRestartButton", "Спробувати ще", C(0f, -30f), C(420f, 80f), Blue);
            Button loseMenu = Btn(lose, "LoseMenuButton", "Головне меню", C(0f, -125f), C(420f, 72f), Grey);
            lose.gameObject.SetActive(false);

            EndScreenController endCtrl = canvas.gameObject.AddComponent<EndScreenController>();
            Wire(endCtrl, "_winPanel", win.gameObject);
            Wire(endCtrl, "_losePanel", lose.gameObject);
            Wire(endCtrl, "_loseInfo", loseInfo);
            Wire(endCtrl, "_winRestartButton", winRestart);
            Wire(endCtrl, "_winMenuButton", winMenu);
            Wire(endCtrl, "_loseRestartButton", loseRestart);
            Wire(endCtrl, "_loseMenuButton", loseMenu);

            SaveScene(scene, GameScenePath);
        }

        // ---------- prefab builders ----------

        private static Enemy BuildEnemyPrefab()
        {
            var go = new GameObject("Enemy");
            var sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = Circle();
            sr.color = Color.white;
            sr.sortingOrder = 3;
            go.AddComponent<Enemy>();

            Directory.CreateDirectory(PrefabDir);
            GameObject asset = PrefabUtility.SaveAsPrefabAsset(go, PrefabDir + "/Enemy.prefab");
            Object.DestroyImmediate(go);
            return asset.GetComponent<Enemy>();
        }

        private static Tower BuildTowerPrefab()
        {
            var go = new GameObject("Tower");
            var sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = Circle();
            sr.color = Color.white;
            sr.sortingOrder = 4;
            go.AddComponent<Tower>();

            Directory.CreateDirectory(PrefabDir);
            GameObject asset = PrefabUtility.SaveAsPrefabAsset(go, PrefabDir + "/Tower.prefab");
            Object.DestroyImmediate(go);
            return asset.GetComponent<Tower>();
        }

        // ---------- path / slot ----------

        private static EnemyPath BuildPath(Transform parent, Vector3[] waypoints, Material lineMat)
        {
            var go = new GameObject("Path");
            go.transform.SetParent(parent, false);
            var lr = go.AddComponent<LineRenderer>();
            lr.useWorldSpace = true;
            lr.sharedMaterial = lineMat;
            lr.positionCount = waypoints.Length;
            lr.SetPositions(waypoints);
            lr.startWidth = 0.38f;
            lr.endWidth = 0.38f;
            lr.numCapVertices = 4;
            lr.numCornerVertices = 4;
            lr.startColor = new Color(0.55f, 0.56f, 0.64f, 0.55f);
            lr.endColor = new Color(0.55f, 0.56f, 0.64f, 0.55f);
            lr.sortingOrder = -1;
            return go.AddComponent<EnemyPath>();
        }

        private static void BuildSlot(Transform parent, Vector3 pos)
        {
            GameObject go = SizedCircle("Slot", new Color(0.55f, 0.85f, 0.6f, 0.32f), pos, 0.7f, 1, parent);
            var slot = go.AddComponent<TowerSlot>();
            Wire(slot, "_renderer", go.GetComponent<SpriteRenderer>());
        }

        // ---------- primitives ----------

        private static void CreateCamera()
        {
            var go = new GameObject("Main Camera");
            go.tag = "MainCamera";
            go.transform.position = new Vector3(0f, 0f, -10f);
            var cam = go.AddComponent<Camera>();
            cam.orthographic = true;
            cam.orthographicSize = 5.5f;
            cam.clearFlags = CameraClearFlags.SolidColor;
            cam.backgroundColor = Background;
            go.AddComponent<AudioListener>();
        }

        private static void CreateEventSystem()
        {
            var go = new GameObject("EventSystem");
            go.AddComponent<EventSystem>();
            go.AddComponent<StandaloneInputModule>();
        }

        private static Canvas CreateCanvas(string name)
        {
            var go = new GameObject(name);
            var canvas = go.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            var scaler = go.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.matchWidthOrHeight = 0.5f;
            go.AddComponent<GraphicRaycaster>();
            return canvas;
        }

        private static SpriteRenderer MakeSprite(string name, Sprite sprite, Color color, Vector3 pos, int order, Transform parent)
        {
            var go = new GameObject(name);
            if (parent != null)
                go.transform.SetParent(parent, false);
            go.transform.position = pos;
            var sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = sprite;
            sr.color = color;
            sr.sortingOrder = order;
            return sr;
        }

        private static GameObject SizedCircle(string name, Color color, Vector3 pos, float diameter, int order, Transform parent)
        {
            SpriteRenderer sr = MakeSprite(name, Circle(), color, pos, order, parent);
            float native = Circle().bounds.size.x;
            float s = native > 0.0001f ? diameter / native : diameter;
            sr.transform.localScale = new Vector3(s, s, 1f);
            return sr.gameObject;
        }

        private static TMP_Text Text(Transform parent, string name, string text, float size,
            TextAlignmentOptions align, Vector2 aMin, Vector2 aMax, Vector2 pivot, Vector2 pos, Vector2 sd, Color color)
        {
            var go = new GameObject(name, typeof(RectTransform));
            go.transform.SetParent(parent, false);
            var rt = (RectTransform)go.transform;
            rt.anchorMin = aMin; rt.anchorMax = aMax; rt.pivot = pivot; rt.anchoredPosition = pos; rt.sizeDelta = sd;
            var t = go.AddComponent<TextMeshProUGUI>();
            TMP_FontAsset font = TMP_Settings.defaultFontAsset;
            if (font == null)
                font = Resources.Load<TMP_FontAsset>("Fonts & Materials/LiberationSans SDF");
            if (font != null)
                t.font = font;
            t.fontSize = size; t.alignment = align; t.color = color; t.text = text; t.raycastTarget = false;
            return t;
        }

        private static Button Btn(Transform parent, string name, string label, Vector2 pos, Vector2 size, Color color, float labelSize = 30f)
        {
            var go = new GameObject(name, typeof(RectTransform));
            go.transform.SetParent(parent, false);
            var rt = (RectTransform)go.transform;
            rt.anchorMin = rt.anchorMax = rt.pivot = new Vector2(0.5f, 0.5f);
            rt.anchoredPosition = pos; rt.sizeDelta = size;
            var img = go.AddComponent<Image>();
            img.sprite = UiSprite(); img.type = Image.Type.Sliced; img.color = color;
            var button = go.AddComponent<Button>();
            button.targetGraphic = img;
            Text(go.transform, "Label", label, labelSize, TextAlignmentOptions.Center,
                Vector2.zero, Vector2.one, new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero, Color.white);
            return button;
        }

        private static Image Bar(Transform parent, string name, Vector2 aMin, Vector2 aMax, Vector2 pivot,
            Vector2 pos, Vector2 size, Color bg, Color fill)
        {
            var bgGo = new GameObject(name, typeof(RectTransform));
            bgGo.transform.SetParent(parent, false);
            var brt = (RectTransform)bgGo.transform;
            brt.anchorMin = aMin; brt.anchorMax = aMax; brt.pivot = pivot; brt.anchoredPosition = pos; brt.sizeDelta = size;
            var bgImg = bgGo.AddComponent<Image>();
            bgImg.sprite = UiSprite(); bgImg.type = Image.Type.Sliced; bgImg.color = bg;
            bgImg.raycastTarget = false;

            var fGo = new GameObject(name + "_Fill", typeof(RectTransform));
            fGo.transform.SetParent(bgGo.transform, false);
            var frt = (RectTransform)fGo.transform;
            frt.anchorMin = Vector2.zero; frt.anchorMax = Vector2.one; frt.pivot = new Vector2(0f, 0.5f);
            frt.offsetMin = new Vector2(3f, 3f); frt.offsetMax = new Vector2(-3f, -3f);
            var fImg = fGo.AddComponent<Image>();
            fImg.sprite = UiSprite(); fImg.type = Image.Type.Filled;
            fImg.fillMethod = Image.FillMethod.Horizontal;
            fImg.fillOrigin = (int)Image.OriginHorizontal.Left;
            fImg.fillAmount = 1f; fImg.color = fill; fImg.raycastTarget = false;
            return fImg;
        }

        private static Image Box(Transform parent, string name, Vector2 aMin, Vector2 aMax, Vector2 pivot,
            Vector2 pos, Vector2 size, Color color)
        {
            var go = new GameObject(name, typeof(RectTransform));
            go.transform.SetParent(parent, false);
            var rt = (RectTransform)go.transform;
            rt.anchorMin = aMin; rt.anchorMax = aMax; rt.pivot = pivot; rt.anchoredPosition = pos; rt.sizeDelta = size;
            var img = go.AddComponent<Image>();
            img.sprite = UiSprite(); img.type = Image.Type.Sliced; img.color = color;
            return img;
        }

        private static RectTransform Rect(Transform parent, string name, Vector2 aMin, Vector2 aMax, Vector2 pivot,
            Vector2 pos, Vector2 size)
        {
            var go = new GameObject(name, typeof(RectTransform));
            go.transform.SetParent(parent, false);
            var rt = (RectTransform)go.transform;
            rt.anchorMin = aMin; rt.anchorMax = aMax; rt.pivot = pivot; rt.anchoredPosition = pos; rt.sizeDelta = size;
            return rt;
        }

        private static Transform Panel(Transform parent, string name, Color color)
        {
            var go = new GameObject(name, typeof(RectTransform));
            go.transform.SetParent(parent, false);
            var rt = (RectTransform)go.transform;
            rt.anchorMin = Vector2.zero; rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.zero; rt.offsetMax = Vector2.zero;
            var img = go.AddComponent<Image>();
            img.sprite = UiSprite(); img.type = Image.Type.Sliced; img.color = color;
            return go.transform;
        }

        // ---------- helpers ----------

        private static Material EnsureLineMaterial()
        {
            Directory.CreateDirectory(MatDir);
            string path = MatDir + "/PathLine.mat";
            Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (mat == null)
            {
                mat = new Material(Shader.Find("Sprites/Default"));
                AssetDatabase.CreateAsset(mat, path);
            }
            return mat;
        }

        private static Sprite UiSprite() => AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");

        private static Sprite _circleSprite;

        // A crisp, high-resolution circle generated once as a project asset. Used for the base,
        // towers, enemies, slots and indicators so nothing is the blurry upscaled built-in Knob.
        // Pixels-per-unit == texture size, so the sprite is exactly 1 world unit (bounds-based
        // sizing keeps every object's diameter correct).
        private static Sprite Circle()
        {
            if (_circleSprite == null)
                _circleSprite = EnsureCircleSprite();
            return _circleSprite != null
                ? _circleSprite
                : AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Knob.psd");
        }

        private static Sprite EnsureCircleSprite()
        {
            Directory.CreateDirectory(SpriteDir);
            string path = SpriteDir + "/Circle.png";
            Sprite existing = AssetDatabase.LoadAssetAtPath<Sprite>(path);
            if (existing != null)
                return existing;

            const int size = 256;
            var tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
            float radius = size * 0.5f;
            const float edge = 1.5f; // anti-aliased rim width in pixels
            var pixels = new Color32[size * size];
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    float dx = x + 0.5f - radius;
                    float dy = y + 0.5f - radius;
                    float dist = Mathf.Sqrt(dx * dx + dy * dy);
                    float a = Mathf.Clamp01((radius - dist) / edge);
                    pixels[y * size + x] = new Color32(255, 255, 255, (byte)(a * 255f));
                }
            }
            tex.SetPixels32(pixels);
            tex.Apply();

            File.WriteAllBytes(path, tex.EncodeToPNG());
            Object.DestroyImmediate(tex);
            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceSynchronousImport);

            var importer = (TextureImporter)AssetImporter.GetAtPath(path);
            if (importer != null)
            {
                importer.textureType = TextureImporterType.Sprite;
                importer.spriteImportMode = SpriteImportMode.Single;
                importer.spritePixelsPerUnit = size;
                importer.filterMode = FilterMode.Bilinear;
                importer.alphaIsTransparency = true;
                importer.mipmapEnabled = false;
                importer.SaveAndReimport();
            }

            return AssetDatabase.LoadAssetAtPath<Sprite>(path);
        }

        private static Vector2 C(float x, float y) => new Vector2(x, y);

        private static void Wire(Object target, string field, Object value)
        {
            var so = new SerializedObject(target);
            SerializedProperty p = so.FindProperty(field);
            if (p == null)
            {
                Debug.LogWarning($"TowerDefense builder: field '{field}' not found on {target.GetType().Name}");
                return;
            }
            p.objectReferenceValue = value;
            so.ApplyModifiedPropertiesWithoutUndo();
        }

        private static void WireArray(Object target, string field, Object[] values)
        {
            var so = new SerializedObject(target);
            SerializedProperty p = so.FindProperty(field);
            if (p == null)
            {
                Debug.LogWarning($"TowerDefense builder: array field '{field}' not found on {target.GetType().Name}");
                return;
            }
            p.arraySize = values.Length;
            for (int i = 0; i < values.Length; i++)
                p.GetArrayElementAtIndex(i).objectReferenceValue = values[i];
            so.ApplyModifiedPropertiesWithoutUndo();
        }

        private static void SaveScene(Scene scene, string path)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            EditorSceneManager.SaveScene(scene, path);
        }
    }
}
