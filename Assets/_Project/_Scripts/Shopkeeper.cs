using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shopkeeper : MonoBehaviour
{
    [SerializeField] private MenuControl shopMenu;

    [SerializeField] private Dialogue welcomeDialogue;
    [SerializeField] private Dialogue offerShopDialogue;

    private void Start()
    {
        offerShopDialogue.onClickYes = () =>
        {
            shopMenu.Open();
        };

        offerShopDialogue.onClickNo = () =>
        {
            InteractableManager.Instance.EndCurrentInteraction();
        };

        offerShopDialogue.onEnd = () =>
        {
            if(shopMenu != MenuControl.Current) //Did not opened shop
                InteractableManager.Instance.EndCurrentInteraction();
        };
    }


    public void StartInteraction()
    {
        // Can call Play one after another, it enqueue each dialogue
        DialogueSystem.Instance.Play(welcomeDialogue);
        DialogueSystem.Instance.Play(offerShopDialogue);
    }
}
