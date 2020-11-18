module Mazes.Core.GridCell

open Mazes.Core.Grid

let getWallTypeForEdge isCurrentCellPartOfMaze =
    match isCurrentCellPartOfMaze with
    | true -> Border
    | false -> Empty

let getWallTypeForInternal isCurrentCellPartOfMaze isOtherCellPartOfMaze =
    match isCurrentCellPartOfMaze, isOtherCellPartOfMaze with
    | false, false -> Empty
    | true, true -> Normal
    | false, true | true, false -> Border

let getCellInstance numberOfRows numberOfColumns rowIndex columnIndex isCellPartOfMaze =            
    let isCurrentCellPartOfMaze = isCellPartOfMaze rowIndex columnIndex
    
    let cellType =
        match isCurrentCellPartOfMaze with
        | true -> PartOfMaze
        | false -> NotPartOfMaze
    
    let wallTypeTop =
        if isFirstRow rowIndex then
            getWallTypeForEdge isCurrentCellPartOfMaze
        else
            let isTopCellPartOfMaze = isCellPartOfMaze (rowIndex - 1) columnIndex

            getWallTypeForInternal isCurrentCellPartOfMaze isTopCellPartOfMaze

    let wallTypeRight =
        if isLastColumn columnIndex numberOfColumns then
            getWallTypeForEdge isCurrentCellPartOfMaze
        else
            let isRightCellPartOfMaze = isCellPartOfMaze rowIndex (columnIndex + 1)

            getWallTypeForInternal isCurrentCellPartOfMaze isRightCellPartOfMaze

    let wallTypeBottom =
        if isLastRow rowIndex numberOfRows then
            getWallTypeForEdge isCurrentCellPartOfMaze
        else
            let isBottomCellPartOfMaze = isCellPartOfMaze (rowIndex + 1) columnIndex

            getWallTypeForInternal isCurrentCellPartOfMaze isBottomCellPartOfMaze

    let wallTypeLeft =
        if isFirstColumn columnIndex then
            getWallTypeForEdge isCurrentCellPartOfMaze
        else
            let isLeftCellPartOfMaze = isCellPartOfMaze rowIndex (columnIndex - 1)

            getWallTypeForInternal isCurrentCellPartOfMaze isLeftCellPartOfMaze

    {
        CellType = cellType
        WallTop = { WallType = wallTypeTop; WallPosition = Position.Top }
        WallRight = { WallType = wallTypeRight; WallPosition = Position.Right }
        WallBottom = { WallType = wallTypeBottom; WallPosition = Position.Bottom }
        WallLeft = { WallType = wallTypeLeft; WallPosition = Position.Left }
    }

let isPartOfMaze rowIndex columnIndex grid =
    (getCell rowIndex columnIndex grid).CellType = PartOfMaze

let isALimitAt position rowIndex columnIndex grid =
    let cell = getCell rowIndex columnIndex grid

    let wallType =
        match position with
        | Top -> cell.WallTop.WallType
        | Right -> cell.WallRight.WallType
        | Bottom -> cell.WallBottom.WallType
        | Left -> cell.WallLeft.WallType

    if cell.CellType = NotPartOfMaze || wallType = Border then
        true
    else
        let rowIndexNeighbor, columnIndexNeighbor =
                match position with
                | Top -> (rowIndex - 1, columnIndex)
                | Right -> (rowIndex, columnIndex + 1)
                | Bottom -> (rowIndex + 1, columnIndex)
                | Left -> (rowIndex, columnIndex - 1)

        (existAt rowIndexNeighbor columnIndexNeighbor grid) && (getCell rowIndexNeighbor columnIndexNeighbor grid).CellType = NotPartOfMaze