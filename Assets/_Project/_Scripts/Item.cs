using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public Sprite graphic;

    public void Consume()
    {
        Inventory.Instance.AddItem(this);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals(InteractableManager.Instance.PlayerTag))
            Consume();
    }
}
