using TMPro;
using UnityEngine;

namespace RPG
{
    public class NPC : MonoBehaviour
    {
        [Header("Dialogue")]
        [SerializeField] [TextArea] private string[] _dialogueLines;

        [Header("UI")]
        [SerializeField] private GameObject _dialoguePanel;
        [SerializeField] private TextMeshProUGUI _dialogueText;

        [Header("Prompt")]
        [SerializeField] private GameObject _interactPrompt;
        [SerializeField] private float _interactRange = 2f;

        [Header("Enemy")]
        [SerializeField] private GameObject _enemyPrefab;
        [SerializeField] private Transform _spawnPoint;

        private int _currentLine;
        private bool _isDialogueOpen;
        private Transform _player;

        public bool IsInRange => _player != null && 
            Vector2.Distance(transform.position, _player.position) <= _interactRange;

        private void Start()
        {
            _player = FindAnyObjectByType<Player>().transform;

            if (_dialoguePanel != null)
                _dialoguePanel.SetActive(false);

            if (_interactPrompt != null)
                _interactPrompt.SetActive(false);
        }

        private void Update()
        {
            UpdatePrompt();
            HandleInput();
        }

        private void HandleInput()
        {
            if (IsInRange && Input.GetKeyDown(KeyCode.E))
            {
                Interact();
            }
        }

        private void UpdatePrompt()
        {
            if (_interactPrompt == null) return;
            _interactPrompt.SetActive(IsInRange && !_isDialogueOpen);
        }

        public void Interact()
        {
            if (!IsInRange) return;

            if (_isDialogueOpen)
                NextLine();
            else
                OpenDialogue();
        }

        private void OpenDialogue()
        {
            if (_dialogueLines == null || _dialogueLines.Length == 0) return;

            _isDialogueOpen = true;
            _currentLine = 0;
            ShowLine();

            if (_dialoguePanel != null)
                _dialoguePanel.SetActive(true);
        }

        private void NextLine()
        {
            _currentLine++;

            if (_currentLine >= _dialogueLines.Length)
                CloseDialogue();
            else
                ShowLine();
        }

        private void ShowLine()
        {
            if (_dialogueText != null && _currentLine < _dialogueLines.Length)
                _dialogueText.text = _dialogueLines[_currentLine];
        }

        private void CloseDialogue()
        {
            _isDialogueOpen = false;

            if (_dialoguePanel != null)
                _dialoguePanel.SetActive(false);

            if (_interactPrompt != null)
                _interactPrompt.SetActive(false);

            if (_enemyPrefab != null && _spawnPoint != null)
            {
                Instantiate(_enemyPrefab, _spawnPoint.position, Quaternion.identity);
            }

            Destroy(gameObject);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _interactRange);
        }
    }
}