module Mazes.Core.GridCell

open Mazes.Core.Position
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
        WallTop = { WallType = wallTypeTop; WallPosition = Top }
        WallRight = { WallType = wallTypeRight; WallPosition = Right }
        WallBottom = { WallType = wallTypeBottom; WallPosition = Bottom }
        WallLeft = { WallType = wallTypeLeft; WallPosition = Left }
    }

let isPartOfMaze rowIndex columnIndex grid =
    (getCell rowIndex columnIndex grid).CellType = PartOfMaze

let getWallTypeAtPos position cell =
    match position with
    | Top -> cell.WallTop.WallType
    | Right -> cell.WallRight.WallType
    | Bottom -> cell.WallBottom.WallType
    | Left -> cell.WallLeft.WallType

let getNeighborCoordinateAtPos coordinate position =
    match position with
    | Top -> (coordinate.RowIndex - 1, coordinate.ColumnIndex)
    | Right -> (coordinate.RowIndex, coordinate.ColumnIndex + 1)
    | Bottom -> (coordinate.RowIndex + 1, coordinate.ColumnIndex)
    | Left -> (coordinate.RowIndex, coordinate.ColumnIndex - 1)

let isALimitAt position rowIndex columnIndex grid =
    let cell = getCell rowIndex columnIndex grid

    if cell.CellType = NotPartOfMaze || (getWallTypeAtPos position cell) = Border then
        true
    else
        let rowIndexNeighbor, columnIndexNeighbor = getNeighborCoordinateAtPos { RowIndex = rowIndex; ColumnIndex = columnIndex } position

        (existAt rowIndexNeighbor columnIndexNeighbor grid) && (getCell rowIndexNeighbor columnIndexNeighbor grid).CellType = NotPartOfMaze

let isNavigable grid fromCoordinate toCoordinate pos =
    not (isALimitAt pos fromCoordinate.RowIndex fromCoordinate.ColumnIndex grid) &&        
    (getCell fromCoordinate.RowIndex fromCoordinate.ColumnIndex grid) |> getWallTypeAtPos pos = Empty &&
    (getCell toCoordinate.RowIndex toCoordinate.ColumnIndex grid).CellType = PartOfMaze

let getNavigableNeighbors coordinate grid =
    let neighbors = ResizeArray<GridCoordinate>()
    let isNavigable = isNavigable grid coordinate
    
    let topCoordinate = {RowIndex = coordinate.RowIndex - 1; ColumnIndex = coordinate.ColumnIndex}
    let rightCoordinate = {RowIndex = coordinate.RowIndex; ColumnIndex = coordinate.ColumnIndex + 1}
    let bottomCoordinate = {RowIndex = coordinate.RowIndex + 1; ColumnIndex = coordinate.ColumnIndex}
    let leftCoordinate = {RowIndex = coordinate.RowIndex; ColumnIndex = coordinate.ColumnIndex - 1}
    
    if (isNavigable topCoordinate Top) then
        neighbors.Add(topCoordinate)
    
    if (isNavigable rightCoordinate Top) then
        neighbors.Add(rightCoordinate)
    
    if (isNavigable bottomCoordinate Top) then
        neighbors.Add(bottomCoordinate)
    
    if (isNavigable leftCoordinate Top) then
        neighbors.Add(leftCoordinate)
    
    neighbors