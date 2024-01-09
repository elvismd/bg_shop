using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleInstance<T> : MonoBehaviour where T : Component
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
                Debug.Log(typeof(T).Name + ": No  Instance Available");

            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (_instance != null && _instance != this)
        {
            DestroyImmediate(gameObject);
            return;
        }

        _instance = this as T;
    }
}
