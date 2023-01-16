using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    private static Action onLoaderCallback;
    public enum State
    {
        HexenGame, Menu
    }

    public static void CanvasActivateDeactivate(Canvas activate, Canvas deactivate)
    {
        onLoaderCallback = () =>
        {
        };

        activate.gameObject.SetActive(false);
        deactivate.gameObject.SetActive(true);
    }

    public static void LoaderCallback()
    {
        if (onLoaderCallback != null)
        {
            onLoaderCallback();
            onLoaderCallback = null;
        }
    }
}
