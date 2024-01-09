using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
public class Interactable : MonoBehaviour
{
    public UnityEvent OnInteract;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag.Equals(InteractableManager.Instance.PlayerTag))
            InteractableManager.Instance.AddInteractable(this);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals(InteractableManager.Instance.PlayerTag))
            InteractableManager.Instance.RemoveInteractable(this);
    }
}
