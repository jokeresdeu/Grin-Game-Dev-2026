using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Makes an object interactable with a key press when player is nearby.
/// Use for doors, NPCs, chests, switches, or any interactive element.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class Interactable : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    [SerializeField] private bool oneTimeInteraction = false;
    
    [Header("Visual")]
    [SerializeField] private GameObject interactionPrompt;
    
    [Header("Events")]
    [SerializeField] private UnityEvent onInteract;
    [SerializeField] private UnityEvent onPlayerEnter;
    [SerializeField] private UnityEvent onPlayerExit;
    
    private bool playerInRange;
    private bool hasInteracted;
    
    private void Awake()
    {
        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(false);
        }
    }
    
    private void Update()
    {
        if (!playerInRange) return;
        if (oneTimeInteraction && hasInteracted) return;
        
        if (Input.GetKeyDown(interactKey))
        {
            Interact();
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        
        playerInRange = true;
        
        if (interactionPrompt != null && !(oneTimeInteraction && hasInteracted))
        {
            interactionPrompt.SetActive(true);
        }
        
        onPlayerEnter?.Invoke();
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        
        playerInRange = false;
        
        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(false);
        }
        
        onPlayerExit?.Invoke();
    }
    
    public void Interact()
    {
        if (oneTimeInteraction && hasInteracted) return;
        
        hasInteracted = true;
        onInteract?.Invoke();
        
        if (oneTimeInteraction && interactionPrompt != null)
        {
            interactionPrompt.SetActive(false);
        }
    }
    
    public void ResetInteraction()
    {
        hasInteracted = false;
    }
}
