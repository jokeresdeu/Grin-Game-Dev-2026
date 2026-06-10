using TMPro;
using UnityEngine;

namespace ChickenHunt
{
    public class BuffPopupUI : MonoBehaviour
    {
        [SerializeField] private GameObject _root;
        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private TextMeshProUGUI _descriptionText;

        public void Show(string title, string description)
        {
            if (_titleText != null)
                _titleText.text = title;

            if (_descriptionText != null)
                _descriptionText.text = description;

            if (_root != null)
                _root.SetActive(true);
            else
                gameObject.SetActive(true);
        }

        public void Hide()
        {
            if (_root != null)
                _root.SetActive(false);
            else
                gameObject.SetActive(false);
        }
    }
}