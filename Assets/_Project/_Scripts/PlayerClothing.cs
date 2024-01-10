using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerClothing : MonoBehaviour
{
    // use enum to set Cloth and Hat indexes?

    [SerializeField] private ClothItem clothItem;
    [SerializeField] private ClothItem hatItem;

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
        clothButtonUI.onClick.AddListener(() =>
        {
            if (clothItem == null)
            {
                if (Inventory.Instance.DraggedItem != null && Inventory.Instance.DraggedItem.GetType() == typeof(ClothItem))
                {
                    EquipCloth(Inventory.Instance.RetrieveDraggedItem() as ClothItem);
                }
            }
            else
            {
                Inventory.Instance.AddItem(clothItem);
                Inventory.Instance.DragItem(clothItem);

                clothSlotUI.color = new Color(0.79f, 0.79f, 0.79f, 0.34f);
                clothUI.color = Color.clear;

                clothItem = null;
                clothAnimator.runtimeAnimatorController = null;
            }
        });
    }

    void EquipCloth(ClothItem cloth)
    {
        clothAnimator.runtimeAnimatorController = cloth.animator;
        clothUI.sprite = cloth.graphic;
        clothSlotUI.sprite = cloth.graphic;
        clothItem = cloth;

        clothSlotUI.color = Color.white;
        clothUI.color = Color.white;
    }

    void EquipHat(ClothItem cloth)
    {

    }
}
