using EMD;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerClothing : MonoBehaviour
{
    // use enum to set Cloth and Hat indexes?

    [SerializeField, ReadOnly] private ClothItem clothItem;
    [SerializeField, ReadOnly] private ClothItem hatItem;

    [SerializeField] private Animator clothAnimator;
    [SerializeField] private Animator hatAnimator;

    [SerializeField] private Image clothUI;
    [SerializeField] private Image hatUI;

    [SerializeField] private Image clothSlotUI;
    [SerializeField] private Image hatSlotUI;

    [SerializeField] private Button clothButtonUI;
    [SerializeField] private Button hatButtonUI;

    public Animator ClothAnimator => clothAnimator;
    public Animator HatAnimator => hatAnimator;

    private void Start()
    {
        clothButtonUI.onClick.AddListener(EquipClothItem);
        hatButtonUI.onClick.AddListener(EquipHatItem);
    }

    void EquipClothItem()
    {
        if (clothItem == null)
        {
            EquipDraggedItem();
        }
        else
        {
            clothSlotUI.color = new Color(0.79f, 0.79f, 0.79f, 0.34f);
            clothUI.color = Color.clear;

           
            clothAnimator.runtimeAnimatorController = null;

            var previousItem = clothItem;
            Inventory.Instance.AddItem(clothItem);
            if (Inventory.Instance.DraggedItem != null)
            {
                EquipDraggedItem();
            }
            else
            {
                clothItem = null;
            }

            Inventory.Instance.DragItem(previousItem);
        }
    }

    void EquipHatItem()
    {
        if (hatItem == null)
        {
            EquipDraggedItem();
        }
        else
        {
            hatSlotUI.color = new Color(0.79f, 0.79f, 0.79f, 0.34f);
            hatUI.color = Color.clear;

            hatAnimator.runtimeAnimatorController = null;

            var previousItem = hatItem;
            Inventory.Instance.AddItem(hatItem);
            if (Inventory.Instance.DraggedItem != null)
            {
                EquipDraggedItem();
            }
            else
            {
                hatItem = null;
            }
            
            Inventory.Instance.DragItem(previousItem);
        }
    }

    void EquipDraggedItem()
    {
        if (Inventory.Instance.DraggedItem != null && Inventory.Instance.DraggedItem.GetType() == typeof(ClothItem))
        {
            var item = Inventory.Instance.RetrieveDraggedItem() as ClothItem;
            switch (item.type)
            {
                case ClothType.CLOTH:
                    EquipCloth(item);
                    break;
                case ClothType.HAT:
                    EquipHat(item);
                    break;
            }
        }
    }

    void EquipCloth(ClothItem cloth)
    {
        clothAnimator.runtimeAnimatorController = cloth.animator;
        clothUI.sprite = cloth.graphicWear;
        clothSlotUI.sprite = cloth.graphicUI;
        clothItem = cloth;

        clothSlotUI.color = Color.white;
        clothUI.color = Color.white;
    }

    void EquipHat(ClothItem cloth)
    {
        hatAnimator.runtimeAnimatorController = cloth.animator;
        hatUI.sprite = cloth.graphicWear;
        hatSlotUI.sprite = cloth.graphicUI;
        hatItem = cloth;

        hatSlotUI.color = Color.white;
        hatUI.color = Color.white;
    }
}
