// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Grid.Array2D.Hex

open Mazes.Core
open Mazes.Core.Array2D
open Mazes.Core.Grid
open Mazes.Core.Grid.Array2D
open Mazes.Core.Grid.Array2D.Hex

[<Struct>]
type HexCell =
    private
        { Walls : Wall<HexPosition> array }

    interface ICell<HexPosition> with
        member this.Create walls =
            ({ Walls = walls } :> ICell<HexPosition>)

        member this.Walls =
            this.WallsArray

        member this.WallIndex position =
            HexCell.WallIndex position

        member this.WallTypeAtPosition position =
            this.Walls.[HexCell.WallIndex position].WallType

        member this.IsALink wallType =
            HexCell.IsALink wallType

        member this.IsLinkedAt position =
            this.ToInterface.IsALink (this.ToInterface.WallTypeAtPosition position)

        member this.AreLinked coordinate otherCoordinate =
            this.ToInterface.IsLinkedAt (HexCoordinateHandler.Instance.NeighborPositionAt coordinate otherCoordinate)

        member this.IsLinked =
            this.IsLinked

    member this.WallsArray =
        this.Walls

    member this.IsLinked =
        (this.Walls
        |> Array.where(fun wall -> HexCell.IsALink wall.WallType)).Length > 0

    member this.ToInterface =
        this :> ICell<HexPosition>

    static member IsALink wallType =
        wallType = Empty

    static member WallIndex position =
        match position with
        | TopLeft -> 0
        | Top -> 1
        | TopRight -> 2
        | BottomLeft -> 3
        | Bottom -> 4
        | BottomRight -> 5

    static member Create numberOfRows numberOfColumns (coordinate : Coordinate) isCellPartOfMaze =
        let isCurrentCellPartOfMaze = isCellPartOfMaze coordinate

        let getWallType isOnEdge position =
            if isOnEdge then
                WallType.getWallTypeForEdge isCurrentCellPartOfMaze
            else
                match (HexCoordinateHandler.Instance.NeighborCoordinateAt coordinate position) with
                | Some neighborCoordinate ->
                    let isNeighborPartOfMaze = isCellPartOfMaze neighborCoordinate
                    WallType.getWallTypeForInternal isCurrentCellPartOfMaze isNeighborPartOfMaze
                | None -> failwith $"Could not find a wall type for the neighbor {coordinate} at {position}"

        let isCIndexEven = coordinate.CIndex % 2 = 0
        let isCIndexOdd = not isCIndexEven

        let wallTypeTopLeft = getWallType ((isFirstRow coordinate.RIndex && isCIndexOdd) || isFirstColumn coordinate.CIndex) TopLeft

        let wallTypeTop = getWallType (isFirstRow coordinate.RIndex) Top

        let wallTypeTopRight = getWallType ((isFirstRow coordinate.RIndex && isCIndexOdd) || isLastColumn coordinate.CIndex numberOfColumns) TopRight

        let wallTypeBottomLeft = getWallType (isFirstColumn coordinate.CIndex || (isLastRow coordinate.RIndex numberOfRows && isCIndexEven)) BottomLeft

        let wallTypeBottom = getWallType (isLastRow coordinate.RIndex numberOfRows) Bottom

        let wallTypeBottomRight = getWallType ((isLastRow coordinate.RIndex numberOfRows && isCIndexEven) || isLastColumn coordinate.CIndex numberOfColumns) BottomRight

        {
            Walls =
                [| { WallType = wallTypeTopLeft; WallPosition = TopLeft }
                   { WallType = wallTypeTop; WallPosition = Top }
                   { WallType = wallTypeTopRight; WallPosition = TopRight }
                   { WallType = wallTypeBottomLeft; WallPosition = BottomLeft }
                   { WallType = wallTypeBottom; WallPosition = Bottom }
                   { WallType = wallTypeBottomRight; WallPosition = BottomRight } |]
        }.ToInterface