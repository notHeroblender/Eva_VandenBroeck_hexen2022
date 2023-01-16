using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    public enum State
    {
        HexenGame, Menu
    }

    private static Action onLoaderCallback;

    public static void Load(Scene scene)
    {
        onLoaderCallback = () =>
        {
        };
        SceneManager.LoadScene(scene.ToString());

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
