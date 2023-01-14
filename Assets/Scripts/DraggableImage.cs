using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableImage : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public Engine GameEngine;
    private GameObject _copy;

    private List<Position> _validPositions;
    private List<List<Position>> _validPostionGroups;

    [SerializeField] private Cards _type;
    public Cards Type => _type;
    public bool IsPlayed = false;

    //when holding a card, make a copy of it and move that to where the mouse is
    public void OnBeginDrag(PointerEventData eventData)
    {
        _copy = Instantiate(transform.gameObject, transform.parent);

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
        _copy.transform.position = eventData.position;    //location of mouse relative to eventsystem

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // highlight possible tiles
        if (Physics.Raycast(ray, out hit, 100) && hit.collider.tag == "Tile")
        {
            PositionView posView = hit.transform.gameObject.GetComponent<PositionView>();
            GameEngine.SetHighlights(posView.HexPosition, Type, _validPositions, _validPostionGroups);
        }
        else
            GameEngine.SetActiveTiles(new List<Position>());

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100) && hit.collider.tag == "Tile")
        {
            PositionView posView = hit.transform.gameObject.GetComponent<PositionView>();
            Destroy(_copy);
            IsPlayed = true;
            posView.OnPointerClick(eventData);
        }
        else
            Destroy(_copy);

        GameEngine.SetActiveTiles(new List<Position>());
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
