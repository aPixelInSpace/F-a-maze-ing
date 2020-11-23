namespace Mazes.Core.Grid

open Mazes.Core
open Mazes.Core.Array2D

type Cell = {
    WallTop : Wall
    WallRight : Wall
    WallBottom : Wall
    WallLeft : Wall
}

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
                WallTop = { WallType = wallTypeTop; WallPosition = Top }
                WallRight = { WallType = wallTypeRight; WallPosition = Right }
                WallBottom = { WallType = wallTypeBottom; WallPosition = Bottom }
                WallLeft = { WallType = wallTypeLeft; WallPosition = Left }
            }