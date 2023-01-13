using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableImage : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public Engine GameEngine;
    private GameObject _copy;
    [SerializeField] private LayerMask _mask;

    private List<Position> _validPositions;
    private List<List<Position>> _validPostionGroups;

    [SerializeField] private Cards _type;
    private Cards Type => _type;


    //when holding a card, make a copy of it and move that to where the mouse is
    public void OnBeginDrag(PointerEventData eventData)
    {
        _copy = Instantiate(this.gameObject);
        _copy.transform.parent = this.transform.parent;
        _copy.transform.position = this.transform.position;

        _validPositions = new List<Position>();
        _validPostionGroups = new List<List<Position>>();

        if (Cards.Teleport == Type)
        {
            _validPositions = GameEngine.GetValidPositions(Type);
        }
        else if (Cards.Shoot == Type || Cards.Swipe == Type || Cards.Push == Type)
        {
            _validPostionGroups = GameEngine.GetValidPositionsGroups(Type);
            ValidGroupsToValidPositions();
        }
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
    private void ValidGroupsToValidPositions()
    {
        foreach (List<Position> positions in _validPostionGroups)
        {

            foreach (Position position in positions)
            {
                _validPositions.Add(position);
            }
        }
    }
}
