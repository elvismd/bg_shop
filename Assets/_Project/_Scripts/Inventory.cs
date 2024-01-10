using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Inventory : SingleInstance<Inventory>
{
    [SerializeField] private List<ItemUI> itemsUI;
    [SerializeField] private List<Item> items;
    [SerializeField] private Image itemRefDraggable;
    [SerializeField] private Vector2 draggableOffset;

    public Item DraggedItem => draggedItem;

    Item draggedItem;
    bool dragging;

    int nextIndex;

    void Start()
    {
        RefreshUI();
    }
    void Update()
    {
        if (dragging)
        {
            itemRefDraggable.rectTransform.position = (Mouse.current.position.ReadValue()) + draggableOffset;
        }
        else
        {
            itemRefDraggable.rectTransform.anchoredPosition = Vector2.right * 6000;
        }
    }

    public void DragItem(Item item)
    {
        if (item == null) return;

        draggedItem = item;
        itemRefDraggable.sprite = item.graphic;
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
        if(nextIndex >= items.Count)
        {
            Debug.Log("Exceeded inventory slots");
            item.gameObject.SetActive(true);

            return;
        }

        items[nextIndex++] = item;

        RefreshUI();
    }

    public Item RetrieveDraggedItem()
    {
        Item itemToReturn = draggedItem;
        ReleaseItem();

        items[items.IndexOf(itemToReturn)] = null;

        RefreshUI();

        nextIndex--;
        if (nextIndex < 0)
            nextIndex = 0;

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
