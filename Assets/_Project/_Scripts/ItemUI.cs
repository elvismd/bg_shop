using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private Image image;
    [SerializeField] private Item item;

    public void SetItem(Item targetItem, UnityAction onClickButton = null)
    {
        button.onClick.RemoveAllListeners();
        if(onClickButton != null)
        {
            button.onClick.AddListener(onClickButton);
        }

        if(targetItem == null)
        {
            image.gameObject.SetActive(false);
            item = null;
        }
        else
        {
            image.gameObject.SetActive(true);
            item = targetItem;
            image.sprite = item.graphic;
        }
    }
}
