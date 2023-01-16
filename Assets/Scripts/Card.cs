using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public Engine GameEngine;
    private GameObject _copy;

    [SerializeField] private List<Position> _validPositions = new List<Position>();
    private List<List<Position>> _validPositionGroups = new List<List<Position>>();

    [SerializeField] private CardType _type;
    public CardType Type => _type;
    public bool IsPlayed = false;

    public void OnBeginDrag(PointerEventData eventData)
    {
        _copy = Instantiate(transform.gameObject, transform.parent);

        _validPositions = new List<Position>();
        _validPositionGroups = new List<List<Position>>();

        if (CardType.Move == Type)
            _validPositions = GameEngine.GetValidPositions(Type);   
        else if(CardType.Shoot == Type || CardType.Slash == Type || CardType.ShockWave == Type)
        {
            _validPositionGroups = GameEngine.GetValidPositionsGroups(Type);
            ValidGroupsToValidPositions();
        }
        else if(CardType.Meteor == Type)
        {
            _validPositionGroups = GameEngine.GetValidPositionsGroups(Type);
            _validPositionGroups.Add(GameEngine.GetValidPositions(Type));
            ValidGroupsToValidPositions();
            //_validPositions = GameEngine.GetValidPositions(Type);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        _copy.transform.position = eventData.position;

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100) && hit.collider.tag == "Tile")
        {
            PositionView positionView = hit.transform.gameObject.GetComponent<PositionView>();

            GameEngine.SetHighlights(positionView.HexPosition, Type, _validPositions, _validPositionGroups);
        }
        else
            GameEngine.SetActiveTiles(new List<Position>());
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        RaycastHit hit;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100) && hit.collider.tag == "Tile")
        {
            PositionView positionView = hit.transform.gameObject.GetComponent<PositionView>();
            Destroy(_copy);
            IsPlayed = true;
            positionView.OnPointerClick(eventData);
        }
        else
            Destroy(_copy);

        GameEngine.SetActiveTiles(new List<Position>());
    }

    private void ValidGroupsToValidPositions()
    {
        foreach (List<Position> positions in _validPositionGroups)
        {
            foreach (Position position in positions)
            {
                _validPositions.Add(position);
            }
        }
    }
}