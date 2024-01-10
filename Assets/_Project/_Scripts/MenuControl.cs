using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuControl : MonoBehaviour
{
    static MenuControl _current;
    public static MenuControl Current => _current;

    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = transform as RectTransform;
    }
    void Update()
    {
        
    }

    public void Open()
    {
        _current = this;
        rectTransform.anchoredPosition = Vector3.zero;
    }

    public void Close()
    {
        if (_current == this) _current = null;

        rectTransform.anchoredPosition = Vector3.right * 6000;
    }
}
