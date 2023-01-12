using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PositionEventArgs : EventArgs
{
    public Position Position { get; }
    public PositionEventArgs(Position position)
    {
        Position = position;
    }
}

public class BoardView : MonoBehaviour
{
    public event EventHandler<PositionEventArgs> PositionClicked;
    private Dictionary<Position, PositionView> _positionViews = new Dictionary<Position, PositionView>();
    private List<Position> _activePosition = new List<Position>();
    private List<Position> _tilePositons = new List<Position>();
    public List<Position> TilePositions => _tilePositons;

    public List<Position> SetActivePosition
    {
        set
        {
            foreach (var position in _activePosition)
                _positionViews[position].DeActivate();

            if (value == null)
                _activePosition.Clear();
            else
                _activePosition = value;

            foreach (var position in value)
                _positionViews[position].Activate();
        }
    }

    internal void ChildClicked(PositionView positionView)
    {
        throw new NotImplementedException();
    }
}
