using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField] private MenuControl firstMenuControl;

    private RectTransform rectTransform;

    private void Start()
    {
        rectTransform = transform as RectTransform;
    }

    private void Update()
    {
        if (InputManager.Instance.Pause.WasPressedThisFrame())
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        if (!GameManager.Instance.IsPaused)
        {
            rectTransform.anchoredPosition = Vector3.zero;

            GameManager.Instance.Pause();
            firstMenuControl.Open();
        }
        else if (GameManager.Instance.IsPaused)
        {
            rectTransform.anchoredPosition = Vector3.right * 6000;

            GameManager.Instance.UnPause();
            MenuControl.Current?.Close();
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
