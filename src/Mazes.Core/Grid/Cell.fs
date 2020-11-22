namespace Mazes.Core.Grid

open Mazes.Core
open Mazes.Core.Array2D

type Cell = {
    CellType : CellType
    WallTop : Wall
    WallRight : Wall
    WallBottom : Wall
    WallLeft : Wall
}

module Cell =

    let private getWallTypeForEdge isCurrentCellPartOfMaze =
        match isCurrentCellPartOfMaze with
        | true -> Border
        | false -> Empty

    let private getWallTypeForInternal isCurrentCellPartOfMaze isOtherCellPartOfMaze =
        match isCurrentCellPartOfMaze, isOtherCellPartOfMaze with
        | false, false -> Empty
        | true, true -> Normal
        | true, false | false, true -> Border

    let getNeighborCoordinateAtPos coordinate position =    
        match position with
        | Top -> { coordinate with RowIndex = coordinate.RowIndex - 1; }
        | Right -> { coordinate with ColumnIndex = coordinate.ColumnIndex + 1 }
        | Bottom -> { coordinate with RowIndex = coordinate.RowIndex + 1; }
        | Left -> { coordinate with ColumnIndex = coordinate.ColumnIndex - 1 }

    let create numberOfRows numberOfColumns coordinate isCellPartOfMaze =
        let isCurrentCellPartOfMaze = isCellPartOfMaze coordinate
        
        let cellType = CellType.create isCurrentCellPartOfMaze
        
        let neighborCoordinate = getNeighborCoordinateAtPos coordinate
        
        let wallTypeTop =
            if isFirstRow coordinate.RowIndex then
                getWallTypeForEdge isCurrentCellPartOfMaze
            else
                let isTopCellPartOfMaze = isCellPartOfMaze (neighborCoordinate Top)

                getWallTypeForInternal isCurrentCellPartOfMaze isTopCellPartOfMaze

        let wallTypeRight =
            if isLastColumn coordinate.ColumnIndex numberOfColumns then
                getWallTypeForEdge isCurrentCellPartOfMaze
            else
                let isRightCellPartOfMaze = isCellPartOfMaze (neighborCoordinate Right)

                getWallTypeForInternal isCurrentCellPartOfMaze isRightCellPartOfMaze

        let wallTypeBottom =
            if isLastRow coordinate.RowIndex numberOfRows then
                getWallTypeForEdge isCurrentCellPartOfMaze
            else
                let isBottomCellPartOfMaze = isCellPartOfMaze (neighborCoordinate Bottom)

                getWallTypeForInternal isCurrentCellPartOfMaze isBottomCellPartOfMaze

        let wallTypeLeft =
            if isFirstColumn coordinate.ColumnIndex then
                getWallTypeForEdge isCurrentCellPartOfMaze
            else
                let isLeftCellPartOfMaze = isCellPartOfMaze (neighborCoordinate Left)

                getWallTypeForInternal isCurrentCellPartOfMaze isLeftCellPartOfMaze

        {
            CellType = cellType
            WallTop = { WallType = wallTypeTop; WallPosition = Top }
            WallRight = { WallType = wallTypeRight; WallPosition = Right }
            WallBottom = { WallType = wallTypeBottom; WallPosition = Bottom }
            WallLeft = { WallType = wallTypeLeft; WallPosition = Left }
        }