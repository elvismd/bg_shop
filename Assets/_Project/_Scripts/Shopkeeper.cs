using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shopkeeper : MonoBehaviour
{
    [SerializeField] private Dialogue welcomeDialogue;
    [SerializeField] private Dialogue offerShopDialogue;

    private void Start()
    {
        //welcomeDialogue.onEnd += () => InteractableManager.Instance.EndCurrentInteraction();
        offerShopDialogue.onEnd += () => InteractableManager.Instance.EndCurrentInteraction();
    }

    public void StartInteraction()
    {
        DialogueSystem.Instance.Play(welcomeDialogue);
        DialogueSystem.Instance.Play(offerShopDialogue);
    }
}
