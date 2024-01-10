using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameManager : SingleInstance<GameManager>
{
    [SerializeField] private Texture2D initialCursorSprite;

    public Action OnInteractionStart;
    public Action OnInteractionEnd;
    public Action<bool> OnTogglePause;

    public bool IsPaused => paused;

    private bool paused;

    private void Start()
    {
        ResetCursorSprite();
    }

    public void SetCursorSprite(Texture2D sprite)
    {
        Cursor.SetCursor(sprite, Vector2.zero, CursorMode.Auto);
    }

    public void ResetCursorSprite()
    {
        Cursor.SetCursor(initialCursorSprite, Vector2.zero, CursorMode.Auto);
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        paused = true;

        OnTogglePause?.Invoke(paused);

        ResetCursorSprite();
    }

    public void UnPause()
    {
        Time.timeScale = 1f;
        paused = false;

        OnTogglePause?.Invoke(paused);
    }
}
