using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shopkeeper : MonoBehaviour
{
    [SerializeField] private Shop shop;

    [SerializeField] private Dialogue welcomeDialogue;
    [SerializeField] private Dialogue offerShopDialogue;

    bool firstTime = true;

    private void Start()
    {
        offerShopDialogue.onClickYes = () =>
        {
            shop.SetupShop();
        };

        offerShopDialogue.onClickNo = () =>
        {
            EndInteraction();
        };

        offerShopDialogue.onEnd = () =>
        {
            if (shop.MenuControl != MenuControl.Current) //Did not opened shop
                EndInteraction();
        };
    }


    public void StartInteraction()
    {
        if (firstTime)
        {
            // Can call Play one after another, it enqueue each dialogue
            DialogueSystem.Instance.Play(welcomeDialogue);
            DialogueSystem.Instance.Play(offerShopDialogue);

            firstTime = false;
        }
        else
        {
            shop.SetupShop();
        }
    }

    public void EndInteraction()
    {
        InteractableManager.Instance.EndCurrentInteraction();
    }
}
