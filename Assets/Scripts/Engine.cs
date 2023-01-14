using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Engine
{
    private List<Position> _selectedPositions = new List<Position>();
    public List<Position> SelectedPositions => _selectedPositions;

    private Board _board;
    private CharView _player;
    private Deck _deck;
    private CharView[] _pieces;
    private BoardView _boardView;

    public Engine(Board board, BoardView boardView, CharView player, Deck deck, CharView[] pieces)
    {
        _board = board;
        _player = player;
        _deck = deck;
        _pieces = pieces;
        _boardView = boardView;
    }

    internal void CardLogic(Position position)
    {
        var cards = _deck.GetComponentsInChildren<DraggableImage>();
        foreach (DraggableImage card in cards)
        {
            if (card.IsPlayed)
            {
                if (card.Type == Cards.Teleport)
                {
                    card.IsPlayed = _board.Move(PositionHelper.WorldToHexPosition(_player.WorldPosition), position);
                }
                else if (!_selectedPositions.Contains(position))
                {
                    card.IsPlayed = false;
                    return;
                }
                else if (card.Type == Cards.Swipe)
                {
                    foreach (Position pos in _selectedPositions)
                    {
                        _board.Take(pos);
                    }
                }
                else if (card.Type == Cards.Shoot)
                {
                    foreach (Position pos in _selectedPositions)
                    {
                        _board.Take(pos);
                    }
                }
                else if (card.Type == Cards.Push)
                {
                    foreach (Position pos in _selectedPositions)
                    {
                        Position offset = HexHelper.AxialSubtract(pos, PositionHelper.WorldToHexPosition(_player.WorldPosition));
                        Position moveTo = HexHelper.AxialAdd(pos, offset);

                        if (_board.IsValidPosition(moveTo))
                        {
                            _board.Move(pos, moveTo);
                        }
                        else
                            _board.Take(pos);
                    }
                }
            }
        }
        _deck.DeckUpdate();
    }

    internal List<Position> GetValidPositions(Cards card)
    {
        List<Position> positions = new List<Position>();

        if (card == Cards.Teleport)
        {

            foreach (var position in _boardView.TilePositions)
            {
                bool positionIsFree = true;

                foreach (var piece in _pieces)
                {
                    var pos = PositionHelper.WorldToHexPosition(piece.WorldPosition);
                    if (pos.Q == position.Q && pos.R == position.R && piece.gameObject.activeSelf)
                    {
                        positionIsFree = false;
                        break;
                    }
                }
                if (positionIsFree)
                {
                    positions.Add(position);
                }
            }
            return positions;
        }
        return null;
    }
    public List<List<Position>> GetValidPositionsGroups(Cards card)
    {
        if (card == Cards.Shoot)
        {
            return MoveSetCollection.GetValidTilesForShoot(_player, _board);
        }
        else if (card == Cards.Swipe || card == Cards.Push)
        {
            return MoveSetCollection.GetValidTilesForCones(_player, _board);
        }
        return null;
    }

    internal void SetActiveTiles(List<Position> positions)
    {
        _boardView.SetActivePosition = positions;
    }

    internal void SetHighlights(Position hexPosition, Cards type, List<Position> validPositions, List<List<Position>> validPositionGroups)
    {
        switch (type)
        {
            case Cards.Teleport:
                if (validPositions.Contains(hexPosition))
                {
                    List<Position> positions = new List<Position>();

                    positions.Add(hexPosition);
                    SetActiveTiles(positions);
                }
                break;

            case Cards.Shoot:
            case Cards.Swipe:
            case Cards.Push:
                if (!validPositions.Contains(hexPosition))
                {
                    SetActiveTiles(validPositions);
                }
                else
                {
                    foreach (List<Position> positions in validPositionGroups)
                    {
                        if (positions.Count == 0) continue;

                        if ((type == Cards.Shoot && positions.Contains(hexPosition)) || 
                            (type == Cards.Swipe && positions[0] == hexPosition) || 
                            (type == Cards.Push && positions[0] == hexPosition))
                        {
                            SetActiveTiles(positions);
                            _selectedPositions = positions;
                            break;
                        }
                    }
                }
                break;
            default:
                _selectedPositions = new List<Position>();
                break;
        }
    }
}
