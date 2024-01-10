using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Inventory : SingleInstance<Inventory>
{
    [SerializeField] private int currencyAmount = 50;
    [SerializeField] private TextMeshProUGUI currencyLabel;

    [SerializeField] private List<ItemUI> itemsUI;
    [SerializeField] private List<Item> items;
    [SerializeField] private Image itemRefDraggable;
    [SerializeField] private Vector2 draggableOffset;
    [SerializeField] private Transform itemsRoot;
    [SerializeField] private Transform itemsParent;
    [SerializeField] private GameObject dropButton;

    public Item DraggedItem => draggedItem;
    public int CurrencyAmount => currencyAmount;

    Item draggedItem;
    bool dragging;

    int nextIndex;

    void Start()
    {
        RefreshUI();

        GameManager.Instance.OnTogglePause += OnPause;
    }

    void OnPause(bool pause)
    {
        //if (!pause)
        {
            if(draggedItem != null)
                ReleaseItem();
        }
    }

    void Update()
    {
        currencyLabel.text = currencyAmount.ToString();

        if (dragging)
        {
            itemRefDraggable.rectTransform.position = (Mouse.current.position.ReadValue()) + draggableOffset;
        }
        else
        {
            itemRefDraggable.rectTransform.anchoredPosition = Vector2.right * 6000;
        }

        dropButton.SetActive(dragging);
    }

    public bool ConsumeCurrency(int amount)
    {
        if (currencyAmount - amount < 0)
        {
            Debug.Log("Not enough coins");
            return false;
        }

        currencyAmount -= amount;
        return true;
    }

    public void AddCurrency(int amount)
    {
        currencyAmount += amount;
    }

    public void MoveItemsListTo(Transform parent)
    {
        itemsRoot.SetParent(parent);
    }

    public void ResetItemsList()
    {
        itemsRoot.SetParent(itemsParent);
        itemsRoot.transform.SetAsFirstSibling();
    }

    public void DropCurrentItem()
    {
        var item = RetrieveDraggedItem();

        item.transform.position = PlayerController.Instance.transform.position + Vector3.right * 1.2f;

        item.gameObject.SetActive(true);
    }

    public void DragItem(Item item)
    {
        if (item == null) return;

        draggedItem = item;
        itemRefDraggable.sprite = item.graphicUI;
        dragging = true;
    }

    public void ReleaseItem()
    {
        if (!dragging) return;

        draggedItem = null;
        dragging = false;
        itemRefDraggable.sprite = null;
    }

    public void AddItem(Item item)
    {
        nextIndex = items.Count;
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] == null) //empty slot
            {
                nextIndex = i;
                break;
            }
        }

        if (nextIndex >= items.Count)
        {
            Debug.Log("Exceeded inventory slots");
            item.gameObject.SetActive(true);

            return;
        }

        items[nextIndex] = item;

        RefreshUI();
    }

    public Item RetrieveDraggedItem()
    {
        Item itemToReturn = draggedItem;
        ReleaseItem();

        items[items.IndexOf(itemToReturn)] = null;

        RefreshUI();

        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] == null) //empty slot
            {
                nextIndex = i;
                break;
            }
        }

        return itemToReturn;
    }

    public void RefreshUI()
    {
        for (int i = 0; i < itemsUI.Count; i++)
        {
            var itemUI = itemsUI[i];
            if (i >= items.Count)
            {
                items.Add(null); //empty slot
            }
        }

        for (int i = 0; i < items.Count; i++)
        {
            var item = items[i];
            var itemUI = itemsUI[i];

            int referenceIndex = i;

            itemUI.SetItem(item,
            () =>
                {
                    Debug.Log("Ref Index: " + referenceIndex);

                    if (!dragging)
                        DragItem(item);
                    else
                    {
                        if (item == draggedItem)
                        {
                            ReleaseItem();
                        }
                        else
                        {
                            var originalItem = items[referenceIndex];
                            var sourceIndex = items.IndexOf(draggedItem);

                            items[referenceIndex] = draggedItem;
                            items[sourceIndex] = originalItem;

                            RefreshUI();
                            ReleaseItem();
                        }
                    }
                });
        }
    }
}
