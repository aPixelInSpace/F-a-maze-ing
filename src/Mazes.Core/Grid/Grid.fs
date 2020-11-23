namespace Mazes.Core.Grid

open Mazes.Core
open Mazes.Core.Array2D
open Mazes.Core.Canvas
open Mazes.Core.Canvas.Canvas

type Grid = {
    Canvas : Canvas
    Cells : Cell[,]
}

module Grid =

    let create canvas =
        let isPartOfMaze = Canvas.isPartOfMaze canvas
        let cells =
            Array2D.init
                canvas.NumberOfRows canvas.NumberOfColumns
                (fun rowIndex columnIndex -> Cell.Instance.create canvas.NumberOfRows canvas.NumberOfColumns { RowIndex = rowIndex; ColumnIndex = columnIndex } isPartOfMaze)

        { Canvas = canvas; Cells = cells; }

    let hasCells grid =
        grid.Cells.Length > 0

    let getWallTypeAtPosition position cell =
        match position with
        | Top -> cell.WallTop.WallType
        | Right -> cell.WallRight.WallType
        | Bottom -> cell.WallBottom.WallType
        | Left -> cell.WallLeft.WallType

    let isALimitAt position coordinate grid =
        let zone = coordinate |> getZone grid.Canvas
        let cell = get coordinate grid.Cells

        if zone = NotPartOfMaze || (getWallTypeAtPosition position cell) = Border then
            true
        else
            let neighborCoordinate = Cell.getNeighborCoordinateAtPosition coordinate position

            (existAt grid.Canvas neighborCoordinate) &&
            neighborCoordinate |> getZone grid.Canvas  = NotPartOfMaze

    let isNavigable grid fromCoordinate toCoordinate pos =
        not (isALimitAt pos fromCoordinate grid) &&        
        (get fromCoordinate grid.Cells) |> getWallTypeAtPosition pos = Empty &&
        toCoordinate |> getZone grid.Canvas = PartOfMaze

    let getNavigableNeighbors coordinate grid =
        let neighbors = ResizeArray<Coordinate>()
        let isNavigable = isNavigable grid coordinate
        let neighborCoordinate = Cell.getNeighborCoordinateAtPosition coordinate
        
        let topCoordinate = neighborCoordinate Top
        let rightCoordinate = neighborCoordinate Right
        let bottomCoordinate = neighborCoordinate Bottom
        let leftCoordinate = neighborCoordinate Left
        
        if (isNavigable topCoordinate Top) then
            neighbors.Add(topCoordinate)
        
        if (isNavigable rightCoordinate Right) then
            neighbors.Add(rightCoordinate)
        
        if (isNavigable bottomCoordinate Bottom) then
            neighbors.Add(bottomCoordinate)
        
        if (isNavigable leftCoordinate Left) then
            neighbors.Add(leftCoordinate)
        
        neighbors

    let updateWallAtPosition position wallType coordinate grid =
        let r = coordinate.RowIndex
        let c = coordinate.ColumnIndex

        match position with
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

    let ifNotAtLimitUpdateWallAtPosition position wallType coordinate grid =        
        if not (isALimitAt position coordinate grid) then
            updateWallAtPosition position wallType coordinate grid