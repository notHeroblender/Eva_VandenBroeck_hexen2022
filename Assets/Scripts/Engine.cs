using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
