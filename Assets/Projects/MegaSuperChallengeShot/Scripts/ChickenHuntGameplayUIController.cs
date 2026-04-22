using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Projects.MegaSuperChallengeShot.Scripts
{
    public class ChickenHuntGameplayUIController : MonoBehaviour
    {
        public static bool IsPaused { get; private set; }

        private CrosshairController _crosshairController;
        private GameObject _pauseMenuRoot;
        private Button _resumeButton;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Bootstrap()
        {
            Scene activeScene = SceneManager.GetActiveScene();
            if (activeScene.path != ChickenHuntScenePaths.GameplayScenePath)
            {
                return;
            }

            if (Object.FindFirstObjectByType<ChickenHuntGameplayUIController>() != null)
            {
                return;
            }

            GameObject controllerObject = new("ChickenHuntGameplayUIController");
            controllerObject.AddComponent<ChickenHuntGameplayUIController>();
        }

        private void Awake()
        {
            IsPaused = false;
            Time.timeScale = 1f;
            Cursor.visible = false;
        }

        private void Start()
        {
            ChickenHuntUiUtility.EnsureEventSystem();
            Canvas canvas = ChickenHuntUiUtility.EnsureCanvas("Canvas");

            _crosshairController = Object.FindFirstObjectByType<CrosshairController>();

            ConfigureHud(canvas.transform);
            BuildPauseMenu(canvas.transform);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SetPaused(!IsPaused);
            }
        }

        private void OnDestroy()
        {
            ResumeTime();
            Cursor.visible = true;
        }

        private void ConfigureHud(Transform canvasTransform)
        {
            ScoreManager scoreManager = Object.FindFirstObjectByType<ScoreManager>();
            if (scoreManager != null && scoreManager.ScoreText != null)
            {
                ConfigureText(
                    scoreManager.ScoreText,
                    new Vector2(0f, 1f),
                    new Vector2(0f, 1f),
                    new Vector2(32f, -32f),
                    new Vector2(320f, 60f),
                    new Vector2(0f, 1f),
                    32f,
                    TextAlignmentOptions.Left);
            }

            if (_crosshairController != null && _crosshairController.AmmoText != null)
            {
                ConfigureText(
                    _crosshairController.AmmoText,
                    new Vector2(1f, 1f),
                    new Vector2(1f, 1f),
                    new Vector2(-32f, -32f),
                    new Vector2(320f, 60f),
                    new Vector2(1f, 1f),
                    32f,
                    TextAlignmentOptions.Right);

                _crosshairController.RefreshAmmoText();
            }

            ChickenHuntUiUtility.CreateText(
                "ControlsHint",
                canvasTransform,
                "LMB Shoot    RMB Reload    Esc Pause",
                24f,
                TextAlignmentOptions.Left,
                new Vector2(0f, 0f),
                new Vector2(0f, 0f),
                new Vector2(32f, 32f),
                new Vector2(520f, 40f),
                new Vector2(0f, 0f),
                FontStyles.Normal,
                new Color(1f, 1f, 1f, 0.9f));
        }

        private void BuildPauseMenu(Transform canvasTransform)
        {
            RectTransform overlay = ChickenHuntUiUtility.CreatePanel(
                "PauseMenu",
                canvasTransform,
                new Color(0f, 0f, 0f, 0.62f),
                Vector2.zero,
                Vector2.one,
                Vector2.zero,
                Vector2.zero,
                new Vector2(0.5f, 0.5f));

            RectTransform panel = ChickenHuntUiUtility.CreatePanel(
                "PausePanel",
                overlay,
                new Color(0.11f, 0.15f, 0.11f, 0.96f),
                new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f),
                Vector2.zero,
                new Vector2(560f, 520f),
                new Vector2(0.5f, 0.5f));

            ChickenHuntUiUtility.CreateText(
                "Title",
                panel,
                "Paused",
                60f,
                TextAlignmentOptions.Center,
                new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f),
                new Vector2(0f, 180f),
                new Vector2(420f, 80f),
                new Vector2(0.5f, 0.5f),
                FontStyles.Bold);

            _resumeButton = ChickenHuntUiUtility.CreateButton("ResumeButton", panel, "Resume", new Vector2(0f, 70f), new Vector2(280f, 64f));
            Button restartButton = ChickenHuntUiUtility.CreateButton("RestartButton", panel, "Restart", new Vector2(0f, -20f), new Vector2(280f, 64f));
            Button mainMenuButton = ChickenHuntUiUtility.CreateButton("MainMenuButton", panel, "Main Menu", new Vector2(0f, -110f), new Vector2(280f, 64f));
            Button quitButton = ChickenHuntUiUtility.CreateButton("QuitButton", panel, "Quit", new Vector2(0f, -200f), new Vector2(280f, 64f));

            _resumeButton.onClick.AddListener(() => SetPaused(false));
            restartButton.onClick.AddListener(RestartScene);
            mainMenuButton.onClick.AddListener(LoadMainMenu);
            quitButton.onClick.AddListener(QuitGame);

            _pauseMenuRoot = overlay.gameObject;
            _pauseMenuRoot.SetActive(false);
        }

        private static void ConfigureText(
            TMP_Text text,
            Vector2 anchorMin,
            Vector2 anchorMax,
            Vector2 anchoredPosition,
            Vector2 sizeDelta,
            Vector2 pivot,
            float fontSize,
            TextAlignmentOptions alignment)
        {
            RectTransform rectTransform = text.rectTransform;
            ChickenHuntUiUtility.SetRect(rectTransform, anchorMin, anchorMax, anchoredPosition, sizeDelta, pivot);
            text.fontSize = fontSize;
            text.alignment = alignment;
            text.fontStyle = FontStyles.Bold;
            text.raycastTarget = false;
        }

        private void SetPaused(bool paused)
        {
            if (_pauseMenuRoot == null || IsPaused == paused)
            {
                return;
            }

            IsPaused = paused;
            _pauseMenuRoot.SetActive(paused);

            if (_crosshairController != null)
            {
                _crosshairController.gameObject.SetActive(!paused);
            }

            if (GameAnimationManager.Instance != null)
            {
                GameAnimationManager.Instance.SetAnimationsPaused(paused);
            }
            else
            {
                Time.timeScale = paused ? 0f : 1f;
            }

            Cursor.visible = paused;

            if (paused && EventSystem.current != null && _resumeButton != null)
            {
                EventSystem.current.SetSelectedGameObject(_resumeButton.gameObject);
            }
        }

        private void RestartScene()
        {
            ResumeTime();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        private void LoadMainMenu()
        {
            ResumeTime();
            SceneManager.LoadScene(ChickenHuntScenePaths.MainMenuSceneName);
        }

        private void QuitGame()
        {
            ResumeTime();
            Application.Quit();
#if UNITY_EDITOR
            Debug.Log("Quit requested from Chicken Hunt pause menu.");
#endif
        }

        private void ResumeTime()
        {
            IsPaused = false;

            if (_pauseMenuRoot != null)
            {
                _pauseMenuRoot.SetActive(false);
            }

            if (_crosshairController != null)
            {
                _crosshairController.gameObject.SetActive(true);
            }

            if (GameAnimationManager.Instance != null)
            {
                GameAnimationManager.Instance.SetAnimationsPaused(false);
            }
            else
            {
                Time.timeScale = 1f;
            }
        }
    }
}
