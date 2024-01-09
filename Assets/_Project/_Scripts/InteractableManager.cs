using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractableManager : SingleInstance<InteractableManager>
{
    [SerializeField] private string playerTag;
    [SerializeField] private LayerMask interactableLayer;

    public string PlayerTag => playerTag;

    private List<Interactable> interactables = new List<Interactable>();
    private Interactable selectedInteractable;

    void Start()
    {
        
    }

    void Update()
    {
        selectedInteractable = null;
        if (interactables.Count > 0)
        {
            foreach(var interactable in interactables) 
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()), Vector2.zero, 10f, interactableLayer);
                if(hit.collider != null)
                {
                    selectedInteractable = interactable;
                }
            }
        }

        if(selectedInteractable != null)
        {
            if(InputManager.Instance.Interact.WasPressedThisFrame())
            {
                selectedInteractable.OnInteract.Invoke();
                Debug.Log("Invoked Interactable");
            }
        }
    }

    public void AddInteractable(Interactable interactable) => interactables.Add(interactable);
    public void RemoveInteractable(Interactable interactable) => interactables.Remove(interactable);
}
