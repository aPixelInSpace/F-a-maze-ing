// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Grid

open Mazes.Core
open Mazes.Core.Position
open Mazes.Core.Array2D

[<Struct>]
type Cell =
    { Walls : Wall array }

    static member WallIndex position =
        match position with
        | Top -> 0
        | Right -> 1
        | Bottom -> 2
        | Left -> 3

    member this.WallTop =
        this.Walls.[Cell.WallIndex Top]

    member this.WallRight =
        this.Walls.[Cell.WallIndex Right]

    member this.WallBottom =
        this.Walls.[Cell.WallIndex Bottom]

    member this.WallLeft =
        this.Walls.[Cell.WallIndex Left]

    member this.WallTypeAtPosition position =
        match position with
        | Top -> this.WallTop.WallType
        | Right -> this.WallRight.WallType
        | Bottom -> this.WallBottom.WallType
        | Left -> this.WallLeft.WallType

    static member IsALink wallType =
        wallType = Empty

    member this.IsLinkedAt pos =
        Cell.IsALink (this.WallTypeAtPosition pos)

    /// Returns true if the cell has at least one link
    member this.IsLinked =
        (this.Walls
        |> Array.where(fun wall -> Cell.IsALink wall.WallType)).Length > 0

module Cell =    

    let create numberOfRows numberOfColumns (coordinate : Coordinate) isCellPartOfMaze =

        let getWallTypeForEdge isCurrentCellPartOfMaze =
            match isCurrentCellPartOfMaze with
            | true -> Border
            | false -> Empty

        let getWallTypeForInternal isCurrentCellPartOfMaze isOtherCellPartOfMaze =
            match isCurrentCellPartOfMaze, isOtherCellPartOfMaze with
            | false, false -> Empty
            | true, true -> Normal
            | true, false | false, true -> Border

        let isCurrentCellPartOfMaze = isCellPartOfMaze coordinate

        let getWallType isOnEdge position =
            if isOnEdge then
                getWallTypeForEdge isCurrentCellPartOfMaze
            else
                let isNeighborPartOfMaze = isCellPartOfMaze (coordinate.NeighborCoordinateAtPosition position)
                getWallTypeForInternal isCurrentCellPartOfMaze isNeighborPartOfMaze

        let wallTypeTop = getWallType (isFirstRow coordinate.RowIndex) Top

        let wallTypeRight = getWallType (isLastColumn coordinate.ColumnIndex numberOfColumns) Right

        let wallTypeBottom = getWallType (isLastRow coordinate.RowIndex numberOfRows) Bottom

        let wallTypeLeft = getWallType (isFirstColumn coordinate.ColumnIndex) Left                

        {
            Walls =
                [| { WallType = wallTypeTop; WallPosition = Top }
                   { WallType = wallTypeRight; WallPosition = Right }
                   { WallType = wallTypeBottom; WallPosition = Bottom }
                   { WallType = wallTypeLeft; WallPosition = Left } |]                
        }