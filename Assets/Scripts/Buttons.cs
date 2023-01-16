using Unity.VisualScripting;
using UnityEngine;

public class Buttons : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private Canvas _menu;
    public void StartButton()
    {
        _menu.gameObject.SetActive(false);
        _canvas.gameObject.SetActive(true);
    }
}
