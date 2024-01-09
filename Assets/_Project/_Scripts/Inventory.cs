using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class Inventory : SingleInstance<Inventory>
{
    [SerializeField] private CanvasScaler canvasScaler;
    [SerializeField] private List<ItemUI> itemsUI;
    [SerializeField] private List<Item> items;
    [SerializeField] private Image itemRefDraggable;
    [SerializeField] private Vector2 draggableOffset;

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

            //if (InputManager.Instance.Interact.WasPressedThisFrame())
            //{
            //    Debug.Log(EventSystem.current.currentSelectedGameObject);
            //    if(itemsUI.FirstOrDefault(it => it.gameObject == EventSystem.current.currentSelectedGameObject) == null)
            //    {
            //        ReleaseItem();
            //    }
            //}
        }
        else
        {
            itemRefDraggable.rectTransform.anchoredPosition = Vector2.right * 6000;
        }
    }

    void DragItem(Item item)
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
        //items.Add(item);
        items[nextIndex++] = item;

        RefreshUI();
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
