using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableImage : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private Image _image;
    private GameObject _copy;
    [SerializeField] private LayerMask _mask;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //_image.color = Color.green;
        _copy = Instantiate(this.gameObject);
        _copy.transform.parent = this.transform.parent;
        _copy.transform.position = this.transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;    //location of mouse
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Ray Ray = Camera.main.ScreenPointToRay(eventData.position);

        if (Physics.Raycast(Ray, 100, _mask))
        {
            Destroy(this.gameObject);
        }
        else
        {
            this.transform.position = _copy.transform.position;
        }
        Destroy(_copy);
    }
}
