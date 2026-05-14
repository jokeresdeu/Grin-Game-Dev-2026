using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private float displayTime = 10f; // скільки показувати туторіал

    private void Start()
    {
        if (tutorialPanel != null)
        {
            tutorialPanel.SetActive(true);
            Invoke(nameof(HideTutorial), displayTime);
        }
    }

    private void HideTutorial()
    {
        if (tutorialPanel != null)
            tutorialPanel.SetActive(false);
    }

    // Можна додати метод, щоб ховати туторіал після першої взаємодії
    public void OnPlayerDidAction()
    {
        if (tutorialPanel.activeSelf)
            tutorialPanel.SetActive(false);
    }
}