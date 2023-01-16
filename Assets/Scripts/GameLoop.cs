using System.Collections.Generic;
using UnityEngine;

public class GameLoop : MonoBehaviour
{
    [SerializeField] private GameObject _enemy;
    private Board _board;
    private Deck _deck;
    private BoardView _boardView;
    private Engine _engine;
    private PieceView[] _pieces;
    private PositionView[] _positionViews;
    private PositionView _positionView;
    [SerializeField] private Canvas _canvas;


    void Start()
    {
        _canvas.gameObject.SetActive(false);

        SpawnHelper.SpawnEnemies(_enemy, 8);

        _deck = FindObjectOfType<Deck>();

        _board = new Board(PositionHelper.Distance);
        _board.PieceMoved += (s, e)
             => e.Piece.MoveTo(PositionHelper.HexToWorldPosition(e.ToPosition));

        _board.PieceTaken += (s, e)
            => e.Piece.Taken();

        _board.PiecePlaced += (s, e)
           => e.Piece.Placed(PositionHelper.HexToWorldPosition(e.ToPosition));

        var piecesViews = FindObjectsOfType<PieceView>();

        foreach (var pieceView in piecesViews)
            _board.Place(PositionHelper.WorldToHexPosition(pieceView.WorldPosition), pieceView);

        PieceView player = null;
        foreach (var pieceView in piecesViews)
         if (pieceView.Player == Player.Player1)
         {
            player = pieceView;
            break;
         }
        _pieces = piecesViews;

        var boardView = FindObjectOfType<BoardView>();
        boardView.PositionClicked += OnPositionClicked;
        _boardView = boardView;

        _engine = new Engine(_board, _boardView, player, _deck, _pieces, _positionView);

        if(StartGame())
            _deck.SetupCards(_engine);
    }

    private void OnPositionClicked(object sender, PositionEventArgs e)
    {
        _engine.CardLogic(e.Position);
    }
    public bool StartGame()
    {
        if (_canvas.gameObject.activeSelf == true)
        {
            return true;
        }
        else
            return false;
    }
}