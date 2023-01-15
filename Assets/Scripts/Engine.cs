using System.Collections.Generic;
using System;

public class Engine
{
    private List<Position> _selectedPositions = new List<Position>();
    public List<Position> SelectedPositions => _selectedPositions;

    private Board _board;
    private PieceView _player;
    private Deck _deck;
    private PieceView[] _pieces;
    private BoardView _boardView;
    private CommandQueue _commandQueue;

    public Engine(Board board, BoardView boardView, PieceView player, Deck deck, PieceView[] pieces, CommandQueue commandQueue)
    {
        _board = board;
        _player = player;
        _deck = deck;
        _pieces = pieces;
        _boardView = boardView;
        _commandQueue = commandQueue;
    }

    public void CardLogic(Position position)
    {
        var cards = _deck.GetComponentsInChildren<Card>();
        foreach (Card card in cards)
        {
            if (card.IsPlayed)
            {
                var cardIndex = _deck._cards.IndexOf(card.gameObject);

                if (card.Type == CardType.Move)
                {
                    var playerPosition = PositionHelper.WorldToHexPosition(_player.WorldPosition);
                    _commandQueue.ReturnCommands();
                    Action execute = () =>
                    {
                        card.IsPlayed = _board.Move(playerPosition, position);
                        card.IsPlayed = true;
                        _deck.DeckUpdate();
                    };
                    Action undo = () =>
                    {
                        _board.Move(position, playerPosition);
                        card.IsPlayed = false;
                        _deck.ReturnCard(card, cardIndex);
                    };

                    var command = new DelegateCommand(execute, undo);
                    _commandQueue.Execute(command);
                }
                else if (!_selectedPositions.Contains(position))
                {
                    card.IsPlayed = false;
                    return;
                }
                else if (card.Type == CardType.Slash)
                {
                    List<PieceView> takenPieces = new List<PieceView>();
                    _commandQueue.ReturnCommands();
                    Action execute = () =>
                    {
                        foreach (Position pos in _selectedPositions)
                        {
                            foreach (var piece in _pieces)
                            {
                                var piecePos = PositionHelper.WorldToHexPosition(piece.WorldPosition);
                                if (pos.Q == piecePos.Q && pos.R == piecePos.R && piece.gameObject.activeSelf)
                                {
                                    takenPieces.Add(piece);
                                }
                            }
                            _board.Take(pos);
                        }
                        _deck.DeckUpdate();
                    };
                    Action undo = () =>
                    {
                        foreach (var piece in takenPieces)
                        {
                            var piecePos = PositionHelper.WorldToHexPosition(piece.WorldPosition);
                            _board.Place(piecePos,piece);
                            piece.gameObject.SetActive(true);
                        }
                        card.IsPlayed = false;
                        _deck.ReturnCard(card, cardIndex);
                    };

                    var command = new DelegateCommand(execute, undo);
                    _commandQueue.Execute(command);
                }
                else if (card.Type == CardType.Shoot)
                {
                    List<PieceView> takenPieces = new List<PieceView>();
                    _commandQueue.ReturnCommands();
                    Action execute = () =>
                    {
                        foreach (Position pos in _selectedPositions)
                        {
                            foreach (var piece in _pieces)
                            {
                                var piecePos = PositionHelper.WorldToHexPosition(piece.WorldPosition);
                                if (pos.Q == piecePos.Q && pos.R == piecePos.R && piece.gameObject.activeSelf)
                                {
                                    takenPieces.Add(piece);
                                }
                            }
                            _board.Take(pos);
                        }
                        _deck.DeckUpdate();
                    };
                    Action undo = () =>
                    {
                        foreach (var piece in takenPieces)
                        {
                            var piecePos = PositionHelper.WorldToHexPosition(piece.WorldPosition);
                            _board.Place(piecePos, piece);
                            piece.gameObject.SetActive(true);
                        }
                        card.IsPlayed = false;
                        _deck.ReturnCard(card, cardIndex);
                    };

                    var command = new DelegateCommand(execute, undo);
                    _commandQueue.Execute(command);
                }
                else if (card.Type == CardType.ShockWave)
                {
                    List<PieceView> takenPieces = new List<PieceView>();
                    List<PieceView> movedPieces = new List<PieceView>();
                    List<Position> moveToPos = new List<Position>();
                    _commandQueue.ReturnCommands();
                    Action execute = () =>
                    {
                        foreach (Position pos in _selectedPositions)
                        {
                            Position offset = HexHelper.AxialSubtract(pos, PositionHelper.WorldToHexPosition(_player.WorldPosition));
                            Position moveTo = HexHelper.AxialAdd(pos, offset);

                            if (_board.IsValidPosition(moveTo))
                            {
                                _board.Move(pos, moveTo);
                                foreach (var piece in _pieces)
                                {
                                    var piecePos = PositionHelper.WorldToHexPosition(piece.WorldPosition);
                                    if (pos.Q == piecePos.Q && pos.R == piecePos.R && piece.gameObject.activeSelf)
                                    {
                                        movedPieces.Add(piece);
                                        moveToPos.Add(moveTo);
                                    }
                                }
                            }
                            else
                            {
                                foreach (var piece in _pieces)
                                {
                                    var piecePos = PositionHelper.WorldToHexPosition(piece.WorldPosition);
                                    if (pos.Q == piecePos.Q && pos.R == piecePos.R && piece.gameObject.activeSelf)
                                    {
                                        takenPieces.Add(piece);
                                    }
                                }
                                _board.Take(pos);
                            }
                        }
                        _deck.DeckUpdate();
                    };
                    Action undo = () =>
                    {
                        foreach (var piece in takenPieces)
                        {
                            if(piece != null)
                            {
                                var piecePos = PositionHelper.WorldToHexPosition(piece.WorldPosition);
                                _board.Place(piecePos, piece);
                                piece.gameObject.SetActive(true);
                            }
                        }
                        foreach (var piece in movedPieces)
                        {
                            if (piece != null)
                            {
                                var piecePos = PositionHelper.WorldToHexPosition(piece.WorldPosition);
                                _board.Move(moveToPos[movedPieces.IndexOf(piece)], piecePos);
                            }
                        }
                        card.IsPlayed = false;
                        _deck.ReturnCard(card, cardIndex);
                    };

                    var command = new DelegateCommand(execute, undo);
                    _commandQueue.Execute(command);
                }
            }
        }
        _deck.DeckUpdate();
    }

    public void SetHighlights(Position position, CardType type, List<Position> validPositions, List<List<Position>> validPositionGroups = null)
    {
        switch (type)
        {
            case CardType.Move:
                if (validPositions.Contains(position))
                {
                    List<Position> positions = new List<Position>();

                    positions.Add(position);
                    SetActiveTiles(positions);
                }
                break;

            case CardType.Shoot:
            case CardType.Slash:
            case CardType.ShockWave:
                if (!validPositions.Contains(position))
                {
                    SetActiveTiles(validPositions);
                }
                else
                {
                    foreach (List<Position> positions in validPositionGroups)
                    {
                        if (positions.Count == 0) continue;

                        if ((type == CardType.Shoot && positions.Contains(position)) || (type == CardType.Slash && positions[0] == position) || (type == CardType.ShockWave && positions[0] == position))
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
    public void SetActiveTiles(List<Position> positions)
    {
        _boardView.SetActivePosition = positions;
    }

    public List<Position> GetValidPositions(CardType card)
    {
        List<Position> positions = new List<Position>();

        if (card == CardType.Move)
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

    public List<List<Position>> GetValidPositionsGroups(CardType card)
    {
        if (card == CardType.Shoot)
        {
            return MoveSetCollection.GetValidTilesForShoot(_player, _board);
        }
        else if (card == CardType.Slash || card == CardType.ShockWave)
        {
            return MoveSetCollection.GetValidTilesForCones(_player, _board);
        }
        return null;
    }
}