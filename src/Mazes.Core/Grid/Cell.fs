// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Grid

open Mazes.Core
open Mazes.Core.Array2D

type Cell =
    { Walls : Wall array }

    member this.WallTop =
        this.Walls.[Wall.wallIndex Top]

    member this.WallRight =
        this.Walls.[Wall.wallIndex Right]

    member this.WallBottom =
        this.Walls.[Wall.wallIndex Bottom]

    member this.WallLeft =
        this.Walls.[Wall.wallIndex Left]

    member this.WallTypeAtPosition position =
        match position with
        | Top -> this.WallTop.WallType
        | Right -> this.WallRight.WallType
        | Bottom -> this.WallBottom.WallType
        | Left -> this.WallLeft.WallType

module Cell =    

    let getNeighborCoordinateAtPosition coordinate position =    
        match position with
        | Top -> { coordinate with RowIndex = coordinate.RowIndex - 1; }
        | Right -> { coordinate with ColumnIndex = coordinate.ColumnIndex + 1 }
        | Bottom -> { coordinate with RowIndex = coordinate.RowIndex + 1; }
        | Left -> { coordinate with ColumnIndex = coordinate.ColumnIndex - 1 }

    module Instance =

        let create numberOfRows numberOfColumns coordinate isCellPartOfMaze =

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
                    let neighborCoordinate = getNeighborCoordinateAtPosition coordinate
                    let isNeighborPartOfMaze = isCellPartOfMaze (neighborCoordinate position)
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