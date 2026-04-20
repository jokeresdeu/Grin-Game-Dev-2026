using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ClassicPlatformer
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button _playButton;

        private void Start()
        {
            _playButton.onClick.AddListener(OnPlayClicked);
        }

        private void OnPlayClicked()
        {
            SceneManager.LoadScene("ClassicPlatformer");
        }
    }
}