using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractableManager : SingleInstance<InteractableManager>
{
    [SerializeField] private string playerTag;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private Texture2D interactionCursor;

    public string PlayerTag => playerTag;

    private List<Interactable> interactables = new List<Interactable>();
    private Interactable highlightedInteractable;

    bool interacting = false;

    void Start()
    {
        
    }

    void Update()
    {
        if (interacting || GameManager.Instance.IsPaused) return;

        highlightedInteractable = null;
        if (interactables.Count > 0)
        {
            foreach (var interactable in interactables) 
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()), Vector2.zero, 10f, interactableLayer);
                if(hit.collider != null && hit.collider.transform.parent == interactable.transform)
                {
                    highlightedInteractable = interactable;
                }
            }
        }

        if(highlightedInteractable != null)
        {
            GameManager.Instance.SetCursorSprite(interactionCursor);

            if (InputManager.Instance.Interact.WasPressedThisFrame())
            {
                highlightedInteractable.OnInteract.Invoke();
                Debug.Log("Started Interaction");

                GameManager.Instance.OnInteractionStart.Invoke();

                GameManager.Instance.ResetCursorSprite();
                interacting = true;
            }
        }
        else
        {
            GameManager.Instance.ResetCursorSprite();
        }
    }

    public void EndCurrentInteraction()
    {
        interacting = false;

        GameManager.Instance.OnInteractionEnd.Invoke();
    }

    public void AddInteractable(Interactable interactable) => interactables.Add(interactable);
    public void RemoveInteractable(Interactable interactable) => interactables.Remove(interactable);
}
