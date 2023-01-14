using System;
using System.Collections.Generic;

public class PieceMovedEventArgs : EventArgs
{
    public CharView Piece { get; }
    public Position FromPosition { get; }
    public Position ToPosition { get; }

    public PieceMovedEventArgs(CharView piece, Position fromPosition, Position toPosition)
    {
        Piece = piece;
        FromPosition = fromPosition;
        ToPosition = toPosition;
    }
}

public class PieceTakenEventArgs : EventArgs
{
    public CharView Piece { get; }
    public Position FromPosition { get; }
    public PieceTakenEventArgs(CharView piece, Position fromPosition)
    {
        Piece = piece;
        FromPosition = fromPosition;
    }
}

public class PiecePlacedEventArgs : EventArgs
{
    public CharView Piece { get; }
    public Position ToPosition { get; }
    public PiecePlacedEventArgs(CharView piece, Position toPosition)
    {
        Piece = piece;
        ToPosition = toPosition;
    }
}

public class Board //<> represents what's in the dictionary
{
    public event EventHandler<PieceMovedEventArgs> PieceMoved;
    public event EventHandler<PieceTakenEventArgs> PieceTaken;
    public event EventHandler<PiecePlacedEventArgs> PiecePlaced;

    private Dictionary<Position, CharView> _pieces = new Dictionary<Position, CharView>();

    private readonly int _distance;

    public Board(int distance)
    {
        _distance = distance;
    }
    public bool TryGetPiece(Position position, out CharView piece)
            => _pieces.TryGetValue(position, out piece);

    public bool TryGetPieceAt(Position position, out CharView piece)
        => _pieces.TryGetValue(position, out piece);

    public bool IsValidPosition(Position position)
        => (_distance >= HexHelper.AxialDistance(new Position(0, 0), position));

    //places new piece on position
    public bool Place(Position position, CharView piece)
    {
        if (piece == null)
            return false;

        if (!IsValidPosition(position))
            return false;

        if (_pieces.ContainsKey(position))
            return false;

        if (_pieces.ContainsValue(piece))
            return false;

        _pieces[position] = piece;

        OnPiecePlaced(new PiecePlacedEventArgs(piece, position));

        return true;
    }

    //changes piece position
    public bool Move(Position fromPosition, Position toPosition)
    {
        if (!IsValidPosition(toPosition))
            return false;

        if (_pieces.ContainsKey(toPosition))
            return false;

        if (!_pieces.TryGetValue(fromPosition, out var piece))
            return false;

        _pieces.Remove(fromPosition);
        _pieces[toPosition] = piece;

        OnPieceMoved(new PieceMovedEventArgs(piece, fromPosition, toPosition));

        return true;
    }

    //removes piece from position
    public bool Take(Position fromPosition)
    {
        if (!IsValidPosition(fromPosition))
            return false;

        if (!_pieces.ContainsKey(fromPosition))
            return false;

        if (!_pieces.TryGetValue(fromPosition, out var piece))
            return false;

        _pieces.Remove(fromPosition);

        OnPieceTaken(new PieceTakenEventArgs(piece, fromPosition));

        return true;
    }

    protected virtual void OnPieceMoved(PieceMovedEventArgs eventArgs)
    {
        var handler = PieceMoved;
        handler?.Invoke(this, eventArgs);
    }

    protected virtual void OnPieceTaken(PieceTakenEventArgs eventArgs)
    {
        var handler = PieceTaken;
        handler?.Invoke(this, eventArgs);
    }

    protected virtual void OnPiecePlaced(PiecePlacedEventArgs eventArgs)
    {
        var handler = PiecePlaced;
        handler?.Invoke(this, eventArgs);
    }
}
