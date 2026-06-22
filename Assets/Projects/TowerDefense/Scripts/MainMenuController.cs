using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Projects.TowerDefense.Scripts
{
    /// <summary>
    /// Main menu: pick one of three races (the selected one is highlighted and its playstyle
    /// shown), then Play to load the game scene. The choice is stored in <see cref="RaceSelection"/>.
    /// </summary>
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private Button[] _raceButtons;
        [SerializeField] private TMP_Text _raceNameText;
        [SerializeField] private TMP_Text _blurbText;
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _quitButton;
        [SerializeField] private string _gameSceneName = "TowerDefense_Game";

        private Image[] _raceImages;
        private Color[] _baseColors;

        private void Awake()
        {
            if (_raceButtons != null)
            {
                _raceImages = new Image[_raceButtons.Length];
                _baseColors = new Color[_raceButtons.Length];
                for (int i = 0; i < _raceButtons.Length; i++)
                {
                    int index = i;
                    if (_raceButtons[i] != null)
                    {
                        _raceImages[i] = _raceButtons[i].GetComponent<Image>();
                        _baseColors[i] = index < RaceConfig.All.Length
                            ? RaceConfig.For(RaceConfig.All[index]).Color
                            : Color.white;
                        _raceButtons[i].onClick.AddListener(() => SelectRace(index));
                    }
                }
            }

            if (_playButton != null) _playButton.onClick.AddListener(Play);
            if (_quitButton != null) _quitButton.onClick.AddListener(Quit);
        }

        private void Start()
        {
            int selected = System.Array.IndexOf(RaceConfig.All, RaceSelection.Selected);
            if (selected < 0) selected = 0;
            ApplySelection(selected);
        }

        private void SelectRace(int index)
        {
            if (index < 0 || index >= RaceConfig.All.Length)
                return;

            RaceSelection.Selected = RaceConfig.All[index];
            ApplySelection(index);
        }

        private void ApplySelection(int index)
        {
            if (_raceButtons != null)
            {
                for (int i = 0; i < _raceButtons.Length; i++)
                {
                    bool isSelected = i == index;
                    if (_raceImages != null && i < _raceImages.Length && _raceImages[i] != null)
                    {
                        Color c = _baseColors[i] * (isSelected ? 1f : 0.4f);
                        c.a = 1f;
                        _raceImages[i].color = c;
                    }
                    if (_raceButtons[i] != null)
                        _raceButtons[i].transform.localScale = isSelected ? new Vector3(1.08f, 1.08f, 1f) : Vector3.one;
                }
            }

            if (index < RaceConfig.All.Length)
            {
                RaceConfig rc = RaceConfig.For(RaceConfig.All[index]);
                if (_raceNameText != null) _raceNameText.text = rc.DisplayName;
                if (_blurbText != null) _blurbText.text = rc.Blurb;
            }
        }

        public void Play()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(_gameSceneName);
        }

        public void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
