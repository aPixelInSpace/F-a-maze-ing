// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Grid.Ortho

open Mazes.Core
open Mazes.Core.Array2D
open Mazes.Core.Grid.Ortho

[<Struct>]
type OrthoCell =

    { Walls : OrthoWall array }

    static member WallIndex position =
        match position with
        | Top -> 0
        | Right -> 1
        | Bottom -> 2
        | Left -> 3

    member this.WallTop =
        this.Walls.[OrthoCell.WallIndex Top]

    member this.WallRight =
        this.Walls.[OrthoCell.WallIndex Right]

    member this.WallBottom =
        this.Walls.[OrthoCell.WallIndex Bottom]

    member this.WallLeft =
        this.Walls.[OrthoCell.WallIndex Left]

    member this.WallTypeAtPosition position =
        match position with
        | Top -> this.WallTop.WallType
        | Right -> this.WallRight.WallType
        | Bottom -> this.WallBottom.WallType
        | Left -> this.WallLeft.WallType

    static member IsALink wallType =
        wallType = Empty

    member this.IsLinkedAt pos =
        OrthoCell.IsALink (this.WallTypeAtPosition pos)

    member this.AreLinked (coordinate : Coordinate) (otherCoordinate : Coordinate) =
        this.IsLinkedAt (OrthoCoordinate.neighborPositionAt coordinate otherCoordinate)

    /// Returns true if the cell has at least one link
    member this.IsLinked =
        (this.Walls
        |> Array.where(fun wall -> OrthoCell.IsALink wall.WallType)).Length > 0

module OrthoCell =

    let create numberOfRows numberOfColumns (coordinate : Coordinate) isCellPartOfMaze =

        let isCurrentCellPartOfMaze = isCellPartOfMaze coordinate

        let getWallType isOnEdge position =
            if isOnEdge then
                WallType.getWallTypeForEdge isCurrentCellPartOfMaze
            else
                let isNeighborPartOfMaze = isCellPartOfMaze (OrthoCoordinate.neighborCoordinateAt coordinate position)
                WallType.getWallTypeForInternal isCurrentCellPartOfMaze isNeighborPartOfMaze

        let wallTypeTop = getWallType (isFirstRow coordinate.RIndex) Top

        let wallTypeRight = getWallType (isLastColumn coordinate.CIndex numberOfColumns) Right

        let wallTypeBottom = getWallType (isLastRow coordinate.RIndex numberOfRows) Bottom

        let wallTypeLeft = getWallType (isFirstColumn coordinate.CIndex) Left                

        {
            Walls =
                [| { WallType = wallTypeTop; WallPosition = Top }
                   { WallType = wallTypeRight; WallPosition = Right }
                   { WallType = wallTypeBottom; WallPosition = Bottom }
                   { WallType = wallTypeLeft; WallPosition = Left } |]                
        }