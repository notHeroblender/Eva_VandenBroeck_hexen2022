using System;
using System.Collections.Generic;

    public class PieceMovedEventArgs<TPiece> : EventArgs
    {
        public TPiece Piece { get; }

        public Position FromPosition { get; }

        public Position ToPosition { get; }

        public PieceMovedEventArgs(TPiece piece, Position fromPosition, Position toPosition)
        {
            Piece = piece;
            FromPosition = fromPosition;
            ToPosition = toPosition;
        }
    }

    public class PieceTakenEventArgs<TPiece> : EventArgs
    {
        public TPiece Piece { get; }

        public Position FromPosition { get; }

        public PieceTakenEventArgs(TPiece piece, Position fromPosition)
        {
            Piece = piece;
            FromPosition = fromPosition;
        }
    }

    public class PiecePlacedEventArgs<TPiece> : EventArgs
    {
        public TPiece Piece { get; }

        public Position ToPosition { get; }

        public PiecePlacedEventArgs(TPiece piece, Position toPosition)
        {
            Piece = piece;
            ToPosition = toPosition;
        }
    }

    public class Board<TPiece> //<> represents what's in the dictionary
    {
        public event EventHandler<PieceMovedEventArgs<TPiece>> PieceMoved;
        public event EventHandler<PieceTakenEventArgs<TPiece>> PieceTaken;
        public event EventHandler<PiecePlacedEventArgs<TPiece>> PiecePlaced;

        private Dictionary<Position, TPiece> _pieces = new Dictionary<Position, TPiece>();

        private readonly int _rows;
        private readonly int _columns;

        public Board(int rows, int columns)
        {
            _rows = rows;
            _columns = columns;
        }

        public bool TryGetPieceAt(Position position, out TPiece piece)
            => _pieces.TryGetValue(position, out piece);

        public bool IsValid(Position position)
            => (0 <= position.Q && position.Q < _columns) && (0 <= position.R && position.R < _rows);

        public bool Place(Position position, TPiece piece)
        {
            if (piece == null)
                return false;

            if (!IsValid(position))
                return false;

            if (_pieces.ContainsKey(position))
                return false;

            if (_pieces.ContainsValue(piece))
                return false;

            _pieces[position] = piece;

            OnPiecePlaced(new PiecePlacedEventArgs<TPiece>(piece, position));

            return true;
        }

        public bool Move(Position fromPosition, Position toPosition)
        {
            if (!IsValid(toPosition))
                return false;

            if (_pieces.ContainsKey(toPosition))
                return false;

            if (!_pieces.TryGetValue(fromPosition, out var piece))
                return false;

            _pieces.Remove(fromPosition);
            _pieces[toPosition] = piece;

            OnPieceMoved(new PieceMovedEventArgs<TPiece>(piece, fromPosition, toPosition));

            return true;
        }

        public bool Take(Position fromPosition)
        {
            if (!IsValid(fromPosition))
                return false;

            if (!_pieces.ContainsKey(fromPosition))
                return false;

            if (!_pieces.TryGetValue(fromPosition, out var piece))
                return false;

            _pieces.Remove(fromPosition);

            OnPieceTaken(new PieceTakenEventArgs<TPiece>(piece, fromPosition));

            return true;
        }

        protected virtual void OnPieceMoved(PieceMovedEventArgs<TPiece> eventArgs)
        {
            var handler = PieceMoved;
            handler?.Invoke(this, eventArgs);
        }

        protected virtual void OnPieceTaken(PieceTakenEventArgs<TPiece> eventArgs)
        {
            var handler = PieceTaken;
            handler?.Invoke(this, eventArgs);
        }

        protected virtual void OnPiecePlaced(PiecePlacedEventArgs<TPiece> eventArgs)
        {
            var handler = PiecePlaced;
            handler?.Invoke(this, eventArgs);
        }
    }
