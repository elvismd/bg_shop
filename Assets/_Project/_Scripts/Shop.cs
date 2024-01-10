using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] private MenuControl shopMenu;

    [SerializeField] private List<Item> items;
    [SerializeField] private List<ItemUI> itemsUI;
    [SerializeField] private Transform inventoryItemsParent;
    [SerializeField] private GameObject sellButton;

    public MenuControl MenuControl => shopMenu;

    void Start()
    {
        for (int i = 0; i < itemsUI.Count; i++)
        {
            var itemUI = itemsUI[i];
            if(i < items.Count)
            {
                var item = items[i];
                itemUI.SetItem(item, () =>
                {
                    if(Inventory.Instance.ConsumeCurrency(item.price))
                    {
                        var newBoughtItem = Instantiate(item); //TODO: Use cached version
                        Inventory.Instance.AddItem(newBoughtItem);
                    }
                });
            }
        }
        World.Instance.OnTogglePause += OnPause;
    }

    private void Update()
    {
        if(shopMenu == MenuControl.Current)
        {
            sellButton.SetActive(Inventory.Instance.DraggedItem != null);
        }
    }

    public void OnClickSell()
    {
        if(Inventory.Instance.DraggedItem != null) 
        {
            var item = Inventory.Instance.RetrieveDraggedItem();

            Inventory.Instance.AddCurrency(item.price);

            Destroy(item.gameObject); //TODO: Use cached version
        }
    }
     
    void OnPause(bool pause)
    {
        if(pause && shopMenu == MenuControl.Current)
        {
            CloseShop();
            InteractableManager.Instance.EndCurrentInteraction();
        }
    }

    public void SetupShop()
    {
        shopMenu.Open();
        Inventory.Instance.MoveItemsListTo(inventoryItemsParent);   
    }

    public void CloseShop()
    {
        shopMenu.Close();
        Inventory.Instance.ResetItemsList();
    }
}
