namespace Mazes.Core.Grid

open Mazes.Core
open Mazes.Core.Array2D
open Mazes.Core.Canvas

type Grid = {
    Cells : Cell[,]
    NumberOfRows : int
    NumberOfColumns : int
}

module Grid =

    let create canvas =
        let isPartOfMaze = Canvas.isPartOfMaze canvas
        let cells = Array2D.init canvas.NumberOfRows canvas.NumberOfColumns (fun rowIndex columnIndex -> Cell.create canvas.NumberOfRows canvas.NumberOfColumns { RowIndex = rowIndex; ColumnIndex = columnIndex } isPartOfMaze)

        { Cells = cells; NumberOfRows = canvas.NumberOfRows; NumberOfColumns = canvas.NumberOfColumns }

    let hasCells grid =
        grid.Cells.Length > 0

    let maxRowIndex grid =
        getIndex grid.NumberOfRows

    let maxColumnIndex grid =
        getIndex grid.NumberOfColumns

    let existAt coordinate grid =
        minRowIndex <= coordinate.RowIndex &&
        coordinate.RowIndex <= (maxRowIndex grid) &&
        minColumnIndex <= coordinate.ColumnIndex &&
        coordinate.ColumnIndex <= (maxColumnIndex grid)

    let getCell coordinate grid =
        grid.Cells.[coordinate.RowIndex, coordinate.ColumnIndex]    

    let isPartOfMaze coordinate grid =
        (getCell coordinate grid).CellType = PartOfMaze

    let getWallTypeAtPos position cell =
        match position with
        | Top -> cell.WallTop.WallType
        | Right -> cell.WallRight.WallType
        | Bottom -> cell.WallBottom.WallType
        | Left -> cell.WallLeft.WallType

    let isALimitAt position coordinate grid =
        let cell = getCell coordinate grid

        if cell.CellType = NotPartOfMaze || (getWallTypeAtPos position cell) = Border then
            true
        else
            let neighborCoordinate = Cell.getNeighborCoordinateAtPos coordinate position

            (existAt neighborCoordinate grid) &&
            (getCell neighborCoordinate grid).CellType = NotPartOfMaze

    let isNavigable grid fromCoordinate toCoordinate pos =
        not (isALimitAt pos fromCoordinate grid) &&        
        (getCell fromCoordinate grid) |> getWallTypeAtPos pos = Empty &&
        (getCell toCoordinate grid).CellType = PartOfMaze

    let getNavigableNeighbors coordinate grid =
        let neighbors = ResizeArray<Coordinate>()
        let isNavigable = isNavigable grid coordinate
        
        // to change
        let topCoordinate = Cell.getNeighborCoordinateAtPos coordinate Top
        let rightCoordinate = Cell.getNeighborCoordinateAtPos coordinate Right
        let bottomCoordinate = Cell.getNeighborCoordinateAtPos coordinate Bottom
        let leftCoordinate = Cell.getNeighborCoordinateAtPos coordinate Left
        
        if (isNavigable topCoordinate Top) then
            neighbors.Add(topCoordinate)
        
        if (isNavigable rightCoordinate Top) then
            neighbors.Add(rightCoordinate)
        
        if (isNavigable bottomCoordinate Top) then
            neighbors.Add(bottomCoordinate)
        
        if (isNavigable leftCoordinate Top) then
            neighbors.Add(leftCoordinate)
        
        neighbors

    let updateWallAtPosition wallPosition wallType coordinate grid =
        let r = coordinate.RowIndex
        let c = coordinate.ColumnIndex

        match wallPosition with
        | Top ->
            grid.Cells.[r, c] <- { grid.Cells.[r, c] with WallTop = { WallType = wallType; WallPosition = Top } }
            grid.Cells.[r - 1, c] <- { grid.Cells.[r - 1, c] with WallBottom = { WallType = wallType; WallPosition = Bottom } }
        | Right ->
            grid.Cells.[r, c] <- { grid.Cells.[r, c] with WallRight = { WallType = wallType; WallPosition = Right } }
            grid.Cells.[r, c + 1] <- { grid.Cells.[r, c + 1] with WallLeft = { WallType = wallType; WallPosition = Left } }
        | Bottom ->
            grid.Cells.[r, c] <- { grid.Cells.[r, c] with WallBottom = { WallType = wallType; WallPosition = Bottom } }
            grid.Cells.[r + 1, c] <- { grid.Cells.[r + 1, c] with WallTop = { WallType = wallType; WallPosition = Top } }
        | Left ->
            grid.Cells.[r, c] <- { grid.Cells.[r, c] with WallLeft = { WallType = wallType; WallPosition = Left } }
            grid.Cells.[r, c - 1] <- { grid.Cells.[r, c - 1] with WallRight = { WallType = wallType; WallPosition = Right } }

    let ifNotAtLimitUpdateWallAtPosition wallPosition wallType coordinate grid =
        let isPosAtLimit = (isALimitAt wallPosition coordinate grid)
        if not isPosAtLimit then
            updateWallAtPosition wallPosition wallType coordinate grid