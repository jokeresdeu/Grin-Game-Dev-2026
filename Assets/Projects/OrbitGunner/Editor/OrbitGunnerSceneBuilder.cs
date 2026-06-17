using System.IO;
using Projects.OrbitGunner.Scripts;
using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Projects.OrbitGunner.EditorTools
{
    /// <summary>
    /// Edit-time tool that builds OrbitGunner's two scenes with a conventional,
    /// fully-inspectable GameObject hierarchy and saves them. This is NOT runtime
    /// code: it runs once from the Unity menu, then the saved .unity scenes contain
    /// real, hand-editable objects (camera, systems, core, turret, canvas/HUD/menus,
    /// enemy & bullet prefabs) with all serialized references wired.
    ///
    /// Menu: OrbitGunner ▸ Build Scenes (Menu + Game)
    /// </summary>
    public static class OrbitGunnerSceneBuilder
    {
        private const string SceneDir = "Assets/Projects/OrbitGunner/Scenes";
        private const string PrefabDir = "Assets/Projects/OrbitGunner/Prefabs";
        private const string MenuScenePath = SceneDir + "/OrbitGunner_MainMenu.unity";
        private const string GameScenePath = SceneDir + "/OrbitGunner_Game.unity";

        private static readonly Color Background = new Color(0.05f, 0.06f, 0.10f);
        private static readonly Color Dim = new Color(1f, 1f, 1f, 0.55f);
        private static readonly Color BarBg = new Color(0.12f, 0.12f, 0.16f, 0.85f);

        [MenuItem("OrbitGunner/Build Scenes (Menu + Game)")]
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
            Debug.Log("OrbitGunner: both scenes built and added to Build Settings.");
        }

        [MenuItem("OrbitGunner/Build Main Menu Scene")]
        public static void BuildMenuScene()
        {
            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            CreateCamera(false);
            CreateEventSystem();
            Canvas canvas = CreateCanvas("MenuCanvas");

            Text(canvas.transform, "Title", "ORBITGUNNER", 120f, TextAlignmentOptions.Center,
                C(0.5f, 0.5f), C(0.5f, 0.5f), C(0.5f, 0.5f), C(0f, 300f), C(1400f, 160f), new Color(0.5f, 0.85f, 1f));

            Text(canvas.transform, "Tagline",
                "Гармата в центрі сама обертається — стріляй єдиною кнопкою й не пускай ворогів до ядра.",
                30f, TextAlignmentOptions.Center,
                C(0.5f, 0.5f), C(0.5f, 0.5f), C(0.5f, 0.5f), C(0f, 190f), C(1200f, 80f), new Color(1f, 1f, 1f, 0.8f));

            TMP_Text best = Text(canvas.transform, "Best", "Рекорд: 0", 40f, TextAlignmentOptions.Center,
                C(0.5f, 0.5f), C(0.5f, 0.5f), C(0.5f, 0.5f), C(0f, 110f), C(900f, 56f), Color.white);

            Button play = Btn(canvas.transform, "PlayButton", "ГРАТИ", C(0f, 0f), C(420f, 78f), new Color(0.2f, 0.7f, 0.45f));
            Button quit = Btn(canvas.transform, "QuitButton", "Вийти", C(0f, -96f), C(420f, 64f), new Color(0.5f, 0.3f, 0.34f));

            TMP_Text prompt = Text(canvas.transform, "Prompt", "натисни будь-де, щоб почати", 30f,
                TextAlignmentOptions.Center,
                C(0.5f, 0f), C(0.5f, 0f), C(0.5f, 0f), C(0f, 150f), C(900f, 44f), new Color(1f, 1f, 1f, 0.7f));

            Text(canvas.transform, "Controls",
                "Керування: утримуй ЛКМ / тап / Пробіл — вогонь   ·   Esc — пауза",
                24f, TextAlignmentOptions.Center,
                C(0.5f, 0f), C(0.5f, 0f), C(0.5f, 0f), C(0f, 60f), C(1300f, 40f), new Color(1f, 1f, 1f, 0.45f));

            var controller = new GameObject("MainMenuController").AddComponent<MainMenuController>();
            Wire(controller, "_bestText", best);
            Wire(controller, "_prompt", prompt);
            Wire(controller, "_playButton", play);
            Wire(controller, "_quitButton", quit);

            SaveScene(scene, MenuScenePath);
        }

        [MenuItem("OrbitGunner/Build Game Scene")]
        public static void BuildGameScene()
        {
            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            CreateCamera(true);
            CreateEventSystem();

            // --- Systems ---
            var systems = new GameObject("Systems");
            systems.AddComponent<GameManager>();
            systems.AddComponent<ScoreManager>();
            systems.AddComponent<OverdriveMeter>();
            systems.AddComponent<DifficultyDirector>();
            NovaBurst nova = systems.AddComponent<NovaBurst>();
            BulletPool pool = systems.AddComponent<BulletPool>();
            EnemySpawner spawner = systems.AddComponent<EnemySpawner>();

            // --- Core ---
            GameObject core = SizedCircle("Core", new Color(0.5f, 0.85f, 1f), Vector3.zero, 1.75f, 1, null);
            CoreHealth coreHealth = core.AddComponent<CoreHealth>();
            Wire(coreHealth, "_renderer", core.GetComponent<SpriteRenderer>());
            WireFloat(coreHealth, "_radius", 0.88f);
            Sprite3D("Nucleus", Circle(), new Color(0.85f, 0.97f, 1f), Vector3.zero, V(0.5f), 2, core.transform);

            // --- Turret ---
            var turret = new GameObject("Turret");
            turret.AddComponent<Weapon>();
            TurretController tc = turret.AddComponent<TurretController>();
            GameObject barrel = Sprite3D("Barrel", Square(), new Color(0.95f, 0.95f, 1f),
                Vector3.zero, Vector3.one, 3, turret.transform);
            Vector2 barrelSprite = Square().bounds.size;
            barrel.transform.localScale = new Vector3(1.6f / barrelSprite.x, 0.48f / barrelSprite.y, 1f);
            barrel.transform.localPosition = new Vector3(0.8f, 0f, 0f);
            Wire(tc, "_barrel", barrel.transform);
            Wire(tc, "_barrelRenderer", barrel.GetComponent<SpriteRenderer>());

            // --- Containers ---
            var enemies = new GameObject("Enemies");
            var bullets = new GameObject("Bullets");
            Wire(spawner, "_container", enemies.transform);
            Wire(pool, "_container", bullets.transform);

            // --- Prefabs ---
            Enemy enemyPrefab = BuildEnemyPrefab();
            Bullet bulletPrefab = BuildBulletPrefab();
            AssetDatabase.SaveAssets(); // commit prefab GUIDs before they are referenced by the scene
            Wire(spawner, "_enemyPrefab", enemyPrefab);
            Wire(spawner, "_circleSprite", Circle());
            Wire(spawner, "_triangleSprite", Square());
            Wire(pool, "_bulletPrefab", bulletPrefab);
            Wire(nova, "_ringSprite", Circle());

            // --- HUD canvas ---
            Canvas canvas = CreateCanvas("HUDCanvas");
            TMP_Text score = Text(canvas.transform, "Score", "0", 52f, TextAlignmentOptions.TopLeft,
                C(0f, 1f), C(0f, 1f), C(0f, 1f), C(40f, -28f), C(700f, 70f), Color.white);
            TMP_Text best = Text(canvas.transform, "Best", "Рекорд 0", 30f, TextAlignmentOptions.TopLeft,
                C(0f, 1f), C(0f, 1f), C(0f, 1f), C(42f, -96f), C(700f, 44f), Dim);
            TMP_Text wave = Text(canvas.transform, "Wave", "Хвиля 1 · 0с", 32f, TextAlignmentOptions.TopRight,
                C(1f, 1f), C(1f, 1f), C(1f, 1f), C(-40f, -30f), C(700f, 50f), Color.white);
            TMP_Text combo = Text(canvas.transform, "Combo", "", 46f, TextAlignmentOptions.Top,
                C(0.5f, 1f), C(0.5f, 1f), C(0.5f, 1f), C(0f, -150f), C(400f, 60f), new Color(1f, 0.85f, 0.3f));

            Text(canvas.transform, "HpLabel", "ЯДРО", 24f, TextAlignmentOptions.Right,
                C(0.5f, 1f), C(0.5f, 1f), C(1f, 1f), C(-280f, -40f), C(160f, 34f), Dim);
            Image hp = Bar(canvas.transform, "HpBar", C(0.5f, 1f), C(0.5f, 1f), C(0f, 1f),
                C(-260f, -40f), C(520f, 30f), BarBg, new Color(1f, 0.35f, 0.4f));

            Text(canvas.transform, "OdLabel", "ОВЕРДРАЙВ", 24f, TextAlignmentOptions.Bottom,
                C(0.5f, 0f), C(0.5f, 0f), C(0.5f, 0f), C(0f, 78f), C(320f, 30f), Dim);
            Image od = Bar(canvas.transform, "OverdriveBar", C(0.5f, 0f), C(0.5f, 0f), C(0.5f, 0f),
                C(0f, 44f), C(520f, 26f), BarBg, new Color(0.45f, 0.95f, 1f));
            od.fillAmount = 0f;

            HUDController hud = canvas.gameObject.AddComponent<HUDController>();
            Wire(hud, "_scoreText", score);
            Wire(hud, "_bestText", best);
            Wire(hud, "_comboText", combo);
            Wire(hud, "_waveText", wave);
            Wire(hud, "_hpFill", hp);
            Wire(hud, "_overdriveFill", od);

            // --- Pause panel ---
            Transform pause = Panel(canvas.transform, "PausePanel", new Color(0.03f, 0.04f, 0.08f, 0.82f));
            Text(pause, "Title", "ПАУЗА", 88f, TextAlignmentOptions.Center,
                C(0.5f, 0.5f), C(0.5f, 0.5f), C(0.5f, 0.5f), C(0f, 220f), C(800f, 120f), Color.white);
            Button resume = Btn(pause, "ResumeButton", "Продовжити", C(0f, 90f), C(380f, 70f), new Color(0.2f, 0.7f, 0.45f));
            Button restart = Btn(pause, "RestartButton", "Почати спочатку", C(0f, 8f), C(380f, 70f), new Color(0.25f, 0.45f, 0.8f));
            Button menu = Btn(pause, "MenuButton", "Головне меню", C(0f, -74f), C(380f, 70f), new Color(0.4f, 0.4f, 0.5f));
            Button quitBtn = Btn(pause, "QuitButton", "Вийти", C(0f, -156f), C(380f, 70f), new Color(0.7f, 0.3f, 0.32f));
            pause.gameObject.SetActive(false);

            PauseMenuController pauseCtrl = canvas.gameObject.AddComponent<PauseMenuController>();
            Wire(pauseCtrl, "_panel", pause.gameObject);
            Wire(pauseCtrl, "_resumeButton", resume);
            Wire(pauseCtrl, "_restartButton", restart);
            Wire(pauseCtrl, "_menuButton", menu);
            Wire(pauseCtrl, "_quitButton", quitBtn);

            // --- Game over panel ---
            Transform over = Panel(canvas.transform, "GameOverPanel", new Color(0.03f, 0.04f, 0.08f, 0.86f));
            Text(over, "Title", "ГРУ ЗАВЕРШЕНО", 80f, TextAlignmentOptions.Center,
                C(0.5f, 0.5f), C(0.5f, 0.5f), C(0.5f, 0.5f), C(0f, 260f), C(1000f, 120f), Color.white);
            TMP_Text goScore = Text(over, "FinalScore", "Рахунок: 0", 56f, TextAlignmentOptions.Center,
                C(0.5f, 0.5f), C(0.5f, 0.5f), C(0.5f, 0.5f), C(0f, 150f), C(900f, 70f), Color.white);
            TMP_Text goBest = Text(over, "BestScore", "Рекорд: 0", 38f, TextAlignmentOptions.Center,
                C(0.5f, 0.5f), C(0.5f, 0.5f), C(0.5f, 0.5f), C(0f, 90f), C(900f, 50f), new Color(1f, 1f, 1f, 0.7f));
            TMP_Text badge = Text(over, "NewBest", "НОВИЙ РЕКОРД!", 40f, TextAlignmentOptions.Center,
                C(0.5f, 0.5f), C(0.5f, 0.5f), C(0.5f, 0.5f), C(0f, 30f), C(900f, 50f), new Color(1f, 0.85f, 0.3f));
            Button goRestart = Btn(over, "RestartButton", "Спробувати ще", C(0f, -70f), C(380f, 70f), new Color(0.25f, 0.55f, 0.85f));
            Button goMenu = Btn(over, "MenuButton", "Головне меню", C(0f, -154f), C(380f, 70f), new Color(0.4f, 0.4f, 0.5f));
            TMP_Text goPrompt = Text(over, "Prompt", "натисни, щоб спробувати ще", 30f, TextAlignmentOptions.Center,
                C(0.5f, 0f), C(0.5f, 0f), C(0.5f, 0f), C(0f, 70f), C(900f, 44f), new Color(1f, 1f, 1f, 0.7f));
            over.gameObject.SetActive(false);

            GameOverUI overCtrl = canvas.gameObject.AddComponent<GameOverUI>();
            Wire(overCtrl, "_panel", over.gameObject);
            Wire(overCtrl, "_scoreText", goScore);
            Wire(overCtrl, "_bestText", goBest);
            Wire(overCtrl, "_badge", badge);
            Wire(overCtrl, "_prompt", goPrompt);
            Wire(overCtrl, "_restartButton", goRestart);
            Wire(overCtrl, "_menuButton", goMenu);

            SaveScene(scene, GameScenePath);
        }

        // ---------- prefab builders ----------

        private static Enemy BuildEnemyPrefab()
        {
            var go = new GameObject("Enemy");
            var sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = Circle();
            sr.color = Color.white;
            sr.sortingOrder = 0;
            go.AddComponent<Enemy>();

            Directory.CreateDirectory(PrefabDir);
            GameObject asset = PrefabUtility.SaveAsPrefabAsset(go, PrefabDir + "/Enemy.prefab");
            Object.DestroyImmediate(go);
            return asset.GetComponent<Enemy>();
        }

        private static Bullet BuildBulletPrefab()
        {
            var go = new GameObject("Bullet");
            var sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = Square();
            sr.color = new Color(1f, 0.95f, 0.5f);
            sr.sortingOrder = 5;
            float bulletNative = Square().bounds.size.x;
            float bulletScale = bulletNative > 0.0001f ? 0.34f / bulletNative : 0.34f;
            go.transform.localScale = new Vector3(bulletScale, bulletScale, 1f);
            go.AddComponent<Bullet>();

            Directory.CreateDirectory(PrefabDir);
            GameObject asset = PrefabUtility.SaveAsPrefabAsset(go, PrefabDir + "/Bullet.prefab");
            Object.DestroyImmediate(go);
            return asset.GetComponent<Bullet>();
        }

        // ---------- primitives ----------

        private static void CreateCamera(bool withShake)
        {
            var go = new GameObject("Main Camera");
            go.tag = "MainCamera";
            go.transform.position = new Vector3(0f, 0f, -10f);
            var cam = go.AddComponent<Camera>();
            cam.orthographic = true;
            cam.orthographicSize = 6f;
            cam.clearFlags = CameraClearFlags.SolidColor;
            cam.backgroundColor = Background;
            go.AddComponent<AudioListener>();
            if (withShake)
                go.AddComponent<CameraShake>();
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

        private static GameObject Sprite3D(string name, Sprite sprite, Color color, Vector3 pos, Vector3 scale, int order, Transform parent)
        {
            var go = new GameObject(name);
            if (parent != null)
                go.transform.SetParent(parent, false);
            go.transform.position = pos;
            go.transform.localScale = scale;
            var sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = sprite;
            sr.color = color;
            sr.sortingOrder = order;
            return go;
        }

        private static GameObject SizedCircle(string name, Color color, Vector3 pos, float diameter, int order, Transform parent)
        {
            Sprite sprite = Circle();
            var go = new GameObject(name);
            if (parent != null)
                go.transform.SetParent(parent, false);
            var sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = sprite;
            sr.color = color;
            sr.sortingOrder = order;
            float native = sprite.bounds.size.x;
            float s = native > 0.0001f ? diameter / native : diameter;
            go.transform.localScale = new Vector3(s, s, 1f);
            go.transform.position = pos;
            return go;
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

        private static Button Btn(Transform parent, string name, string label, Vector2 pos, Vector2 size, Color color)
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
            Text(go.transform, "Label", label, 30f, TextAlignmentOptions.Center,
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

            var fGo = new GameObject(name + "_Fill", typeof(RectTransform));
            fGo.transform.SetParent(bgGo.transform, false);
            var frt = (RectTransform)fGo.transform;
            frt.anchorMin = Vector2.zero; frt.anchorMax = Vector2.one; frt.pivot = new Vector2(0f, 0.5f);
            frt.offsetMin = new Vector2(3f, 3f); frt.offsetMax = new Vector2(-3f, -3f);
            var fImg = fGo.AddComponent<Image>();
            fImg.sprite = UiSprite(); fImg.type = Image.Type.Filled;
            fImg.fillMethod = Image.FillMethod.Horizontal;
            fImg.fillOrigin = (int)Image.OriginHorizontal.Left;
            fImg.fillAmount = 1f; fImg.color = fill;
            return fImg;
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

        private static Sprite UiSprite() => AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
        private static Sprite Circle() => AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Knob.psd");
        private static Sprite Square() => AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Background.psd");

        private static Vector2 C(float x, float y) => new Vector2(x, y);
        private static Vector3 V(float s) => new Vector3(s, s, s);

        private static void Wire(Object target, string field, Object value)
        {
            var so = new SerializedObject(target);
            SerializedProperty p = so.FindProperty(field);
            if (p == null)
            {
                Debug.LogWarning($"OrbitGunner builder: field '{field}' not found on {target.GetType().Name}");
                return;
            }
            p.objectReferenceValue = value;
            so.ApplyModifiedPropertiesWithoutUndo();
        }

        private static void WireFloat(Object target, string field, float value)
        {
            var so = new SerializedObject(target);
            SerializedProperty p = so.FindProperty(field);
            if (p == null)
                return;
            p.floatValue = value;
            so.ApplyModifiedPropertiesWithoutUndo();
        }

        private static void SaveScene(Scene scene, string path)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            EditorSceneManager.SaveScene(scene, path);
        }
    }
}
