// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Grid.Hex

open Mazes.Core
open Mazes.Core.Array2D
open Mazes.Core.Grid
open Mazes.Core.Grid.Hex

[<Struct>]
type HexCell =

    { Walls : Wall<HexPosition> array }

    static member WallIndex position =
        match position with
        | TopLeft -> 0
        | Top -> 1
        | TopRight -> 2
        | BottomLeft -> 3
        | Bottom -> 4
        | BottomRight -> 5

    member this.WallTopLeft =
        this.Walls.[HexCell.WallIndex TopLeft]

    member this.WallTop =
        this.Walls.[HexCell.WallIndex Top]

    member this.WallTopRight =
        this.Walls.[HexCell.WallIndex TopRight]

    member this.WallBottomLeft =
        this.Walls.[HexCell.WallIndex BottomLeft]

    member this.WallBottom =
        this.Walls.[HexCell.WallIndex Bottom]

    member this.WallBottomRight =
        this.Walls.[HexCell.WallIndex BottomRight]

    member this.WallTypeAtPosition position =
        match position with
        | TopLeft -> this.WallTopLeft.WallType
        | Top -> this.WallTop.WallType
        | TopRight -> this.WallTopRight.WallType
        | BottomLeft -> this.WallBottomLeft.WallType
        | Bottom -> this.WallBottom.WallType
        | BottomRight -> this.WallBottomRight.WallType

    static member IsALink wallType =
        wallType = Empty

    member this.IsLinkedAt pos =
        HexCell.IsALink (this.WallTypeAtPosition pos)

    member this.AreLinked (coordinate : Coordinate) (otherCoordinate : Coordinate) =
        this.IsLinkedAt (HexCoordinate.neighborPositionAt coordinate otherCoordinate)

    /// Returns true if the cell has at least one link
    member this.IsLinked =
        (this.Walls
        |> Array.where(fun wall -> HexCell.IsALink wall.WallType)).Length > 0

module HexCell =

    let create numberOfRows numberOfColumns (coordinate : Coordinate) isCellPartOfMaze =

        let isCurrentCellPartOfMaze = isCellPartOfMaze coordinate

        let getWallType isOnEdge position =
            if isOnEdge then
                WallType.getWallTypeForEdge isCurrentCellPartOfMaze
            else
                let isNeighborPartOfMaze = isCellPartOfMaze (HexCoordinate.neighborCoordinateAt coordinate position)
                WallType.getWallTypeForInternal isCurrentCellPartOfMaze isNeighborPartOfMaze

        let wallTypeTopLeft = getWallType (isFirstRow coordinate.RIndex || isFirstColumn coordinate.CIndex) TopLeft

        let wallTypeTop = getWallType (isFirstRow coordinate.RIndex) Top

        let wallTypeTopRight = getWallType (isFirstRow coordinate.RIndex || isLastColumn coordinate.CIndex numberOfColumns) TopRight

        let wallTypeBottomLeft = getWallType (isFirstColumn coordinate.CIndex || isLastRow coordinate.RIndex numberOfRows) BottomLeft

        let wallTypeBottom = getWallType (isLastRow coordinate.RIndex numberOfRows) Bottom

        let wallTypeBottomRight = getWallType (isLastRow coordinate.RIndex numberOfRows || isLastColumn coordinate.CIndex numberOfColumns) BottomRight

        {
            Walls =
                [| { WallType = wallTypeTopLeft; WallPosition = TopLeft }
                   { WallType = wallTypeTop; WallPosition = Top }
                   { WallType = wallTypeTopRight; WallPosition = TopRight }
                   { WallType = wallTypeBottomLeft; WallPosition = BottomLeft }
                   { WallType = wallTypeBottom; WallPosition = Bottom }
                   { WallType = wallTypeBottomRight; WallPosition = BottomRight } |]
        }