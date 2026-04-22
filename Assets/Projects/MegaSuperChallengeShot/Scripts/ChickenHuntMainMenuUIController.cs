using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

namespace Projects.MegaSuperChallengeShot.Scripts
{
    public class ChickenHuntMainMenuUIController : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Bootstrap()
        {
            Scene activeScene = SceneManager.GetActiveScene();
            if (activeScene.path != ChickenHuntScenePaths.MainMenuScenePath)
            {
                return;
            }

            if (Object.FindFirstObjectByType<ChickenHuntMainMenuUIController>() != null)
            {
                return;
            }

            GameObject controllerObject = new("ChickenHuntMainMenuUIController");
            controllerObject.AddComponent<ChickenHuntMainMenuUIController>();
        }

        private void Awake()
        {
            Time.timeScale = 1f;
            Cursor.visible = true;
        }

        private void Start()
        {
            ChickenHuntUiUtility.EnsureEventSystem();
            Canvas canvas = ChickenHuntUiUtility.EnsureNamedCanvas("MainMenuCanvas", 50);
            BuildMainMenu(canvas.transform);
        }

        private void BuildMainMenu(Transform canvasTransform)
        {
            RectTransform overlay = ChickenHuntUiUtility.FindChildRectTransform(canvasTransform, "MainMenuOverlay");
            if (overlay == null)
            {
                overlay = ChickenHuntUiUtility.CreatePanel(
                    "MainMenuOverlay",
                    canvasTransform,
                    new Color(0.03f, 0.08f, 0.05f, 0.72f),
                    Vector2.zero,
                    Vector2.one,
                    Vector2.zero,
                    Vector2.zero,
                    new Vector2(0.5f, 0.5f));
            }
            else
            {
                overlay.gameObject.SetActive(true);
            }

            if (ChickenHuntUiUtility.FindChildRectTransform(overlay, "MainMenuPanel") != null)
            {
                return;
            }

            RectTransform panel = ChickenHuntUiUtility.CreatePanel(
                "MainMenuPanel",
                overlay,
                new Color(0.13f, 0.18f, 0.12f, 0.95f),
                new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f),
                Vector2.zero,
                new Vector2(720f, 520f),
                new Vector2(0.5f, 0.5f));

            ChickenHuntUiUtility.CreateText(
                "Title",
                panel,
                "Chicken Hunt",
                72f,
                TextAlignmentOptions.Center,
                new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f),
                new Vector2(0f, 150f),
                new Vector2(560f, 90f),
                new Vector2(0.5f, 0.5f),
                FontStyles.Bold);

            ChickenHuntUiUtility.CreateText(
                "Subtitle",
                panel,
                "Shoot the birds, reload with right mouse button, and pause anytime with Esc.",
                28f,
                TextAlignmentOptions.Center,
                new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f),
                new Vector2(0f, 50f),
                new Vector2(560f, 110f),
                new Vector2(0.5f, 0.5f),
                FontStyles.Normal,
                new Color(0.92f, 0.96f, 0.9f, 1f));

            Button playButton = ChickenHuntUiUtility.CreateButton("PlayButton", panel, "Play", new Vector2(0f, -60f), new Vector2(300f, 72f));
            Button quitButton = ChickenHuntUiUtility.CreateButton("QuitButton", panel, "Quit", new Vector2(0f, -160f), new Vector2(300f, 72f));

            playButton.onClick.AddListener(StartGame);
            quitButton.onClick.AddListener(QuitGame);
        }

        private void StartGame()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(ChickenHuntScenePaths.GameplayBuildIndex);
        }

        private void QuitGame()
        {
            Application.Quit();
#if UNITY_EDITOR
            Debug.Log("Quit requested from Chicken Hunt main menu.");
#endif
        }
    }
}
