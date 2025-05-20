using UnityEngine;
using UnityEngine.Events;


public class InteractTrigger : MonoBehaviour
{
    public UnityEvent onInteract;
    public string promptText = "F ¤¬°Ê";
    public bool triggerOnce = true;
    [SerializeField] KeyCode interactKey = KeyCode.F;

    private bool canInteract = false;
    private bool hasInteracted = false;

    void Update()
    {
        if (canInteract && !hasInteracted && Input.GetKeyDown(interactKey))
        {
            onInteract.Invoke();
            if (triggerOnce) hasInteracted = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) canInteract = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) canInteract = false;
    }
}
