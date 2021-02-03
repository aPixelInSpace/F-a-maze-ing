// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Grid.Array2D.OctaSquare

open Mazes.Core
open Mazes.Core.Array2D
open Mazes.Core.Grid
open Mazes.Core.Grid.Array2D
open Mazes.Core.Grid.Array2D.OctaSquare

[<Struct>]
type OctaSquareCell =
    private
        { Walls : Wall<OctaSquarePosition> array }

    interface ICell<OctaSquarePosition> with
        member this.Create walls =
            ({ Walls = walls } :> ICell<OctaSquarePosition>)

        member this.Walls =
            this.WallsArray

        member this.WallIndex position =
            OctaSquareCell.WallIndex position

        member this.WallTypeAtPosition position =
            this.Walls.[OctaSquareCell.WallIndex position].WallType

        member this.IsALink wallType =
            OctaSquareCell.IsALink wallType

        member this.IsLinkedAt position =
            this.ToInterface.IsALink (this.ToInterface.WallTypeAtPosition position)

        member this.AreLinked coordinate otherCoordinate =
            this.ToInterface.IsLinkedAt (OctaSquareCoordinateHandler.Instance.NeighborPositionAt coordinate otherCoordinate)

        member this.IsLinked =
            this.IsLinked

    member this.WallsArray =
        this.Walls

    member this.IsLinked =
        (this.Walls
        |> Array.where(fun wall -> OctaSquareCell.IsALink wall.WallType)).Length > 0

    member this.ToInterface =
        this :> ICell<OctaSquarePosition>

    static member IsALink wallType =
        wallType = Empty

    static member WallIndex position =
        match position with
        | Left -> 0
        | Top -> 1
        | Right -> 2
        | Bottom -> 3
        | TopLeft -> 4
        | TopRight -> 5
        | BottomLeft -> 6
        | BottomRight -> 7

    static member Create numberOfRows numberOfColumns internalWallType (coordinate : Coordinate) isCellPartOfMaze =
        let isCurrentCellPartOfMaze = isCellPartOfMaze coordinate

        let getWallType isOnEdge position =
            if isOnEdge then
                WallType.getWallTypeForEdge isCurrentCellPartOfMaze
            else
                match (OctaSquareCoordinateHandler.Instance.NeighborCoordinateAt coordinate position) with
                | Some neighborCoordinate ->
                    let isNeighborPartOfMaze = isCellPartOfMaze neighborCoordinate
                    WallType.getWallTypeForInternal internalWallType isCurrentCellPartOfMaze isNeighborPartOfMaze
                | None -> failwith $"Could not find a wall type for the neighbor {coordinate} at {position}"

        let isOctagon = OctaSquarePositionHandler.IsOctagon coordinate
        let isFirstRow = isFirstRow coordinate.RIndex
        let isLastRow = isLastRow coordinate.RIndex numberOfRows
        let isFirstColumn = isFirstColumn coordinate.CIndex
        let isLastColumn = isLastColumn coordinate.CIndex numberOfColumns

        {
            Walls =
                [| { WallType = (getWallType isFirstColumn Left); WallPosition = Left }
                   { WallType = (getWallType isFirstRow Top); WallPosition = Top }
                   { WallType = (getWallType isLastColumn Right); WallPosition = Right }
                   { WallType = (getWallType isLastRow Bottom); WallPosition = Bottom }
                   
                   if isOctagon then
                       { WallType = (getWallType (isFirstRow || isFirstColumn) TopLeft); WallPosition = TopLeft }
                       { WallType = (getWallType (isFirstRow || isLastColumn) TopRight); WallPosition = TopRight }
                       { WallType = (getWallType (isLastRow || isFirstColumn) BottomLeft); WallPosition = BottomLeft }
                       { WallType = (getWallType (isLastRow || isLastColumn) BottomRight); WallPosition = BottomRight } |]
        }.ToInterface