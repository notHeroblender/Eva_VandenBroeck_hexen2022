using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSetCollection
{
    public static List<List<Position>> GetValidTilesForShoot(CharView player, Board board)
    {
        List<List<Position>> positions = new List<List<Position>>();

        positions.Add(new MoveSetHelper(board, PositionHelper.WorldToHexPosition(player.WorldPosition)).RightUp().CollectValidPositions());
        positions.Add(new MoveSetHelper(board, PositionHelper.WorldToHexPosition(player.WorldPosition)).Right().CollectValidPositions());
        positions.Add(new MoveSetHelper(board, PositionHelper.WorldToHexPosition(player.WorldPosition)).RightDown().CollectValidPositions());
        positions.Add(new MoveSetHelper(board, PositionHelper.WorldToHexPosition(player.WorldPosition)).LeftUp().CollectValidPositions());
        positions.Add(new MoveSetHelper(board, PositionHelper.WorldToHexPosition(player.WorldPosition)).Left().CollectValidPositions());
        positions.Add(new MoveSetHelper(board, PositionHelper.WorldToHexPosition(player.WorldPosition)).LeftDown().CollectValidPositions());

        return positions;
    }

    public static List<List<Position>> GetValidTilesForCones(CharView player, Board board)
    {
        List<List<Position>> positions = new List<List<Position>>();

        positions.Add(GetTileConeRightUp(player, board));
        positions.Add(GetTileConeRight(player, board));
        positions.Add(GetTileConeRightDown(player, board));
        positions.Add(GetTileConeLeftUp(player, board));
        positions.Add(GetTileConeLeft(player, board));
        positions.Add(GetTileConeLeftDown(player, board));

        return positions;
    }

    private static List<Position> GetTileConeRightUp(CharView player, Board board)
    {
        return new MoveSetHelper(board, PositionHelper.WorldToHexPosition(player.WorldPosition))
            .RightUp(1).LeftUp(1).Right(1).CollectValidPositions(); //RightUp
    }
    private static List<Position> GetTileConeRight(CharView player, Board board)
    {
        return new MoveSetHelper(board, PositionHelper.WorldToHexPosition(player.WorldPosition))
            .Right(1).RightUp(1).RightDown(1).CollectValidPositions(); //Right
    }
    private static List<Position> GetTileConeRightDown(CharView player, Board board)
    {
        return new MoveSetHelper(board, PositionHelper.WorldToHexPosition(player.WorldPosition))
            .RightDown(1).Right(1).LeftDown(1).CollectValidPositions(); //RightDown
    }
    private static List<Position> GetTileConeLeftUp(CharView player, Board board)
    {
        return new MoveSetHelper(board, PositionHelper.WorldToHexPosition(player.WorldPosition))
            .LeftUp(1).RightUp(1).Left(1).CollectValidPositions(); //LeftUp
    }
    private static List<Position> GetTileConeLeft(CharView player, Board board)
    {
        return new MoveSetHelper(board, PositionHelper.WorldToHexPosition(player.WorldPosition))
            .Left(1).LeftUp(1).LeftDown(1).CollectValidPositions(); //Left
    }
    private static List<Position> GetTileConeLeftDown(CharView player, Board board)
    {
        return new MoveSetHelper(board, PositionHelper.WorldToHexPosition(player.WorldPosition))
            .LeftDown(1).Left(1).RightDown(1).CollectValidPositions(); //LeftDown
    }
}