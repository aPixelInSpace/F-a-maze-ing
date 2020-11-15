module Mazes.Core.Grid.Shape.Rectangle

open Mazes.Core.Cell
open Mazes.Core.Grid.Grid

let private getCell numberOfRows numberOfColumns rowIndex columnIndex =
    let wallTop =
        match rowIndex with
        | 0 -> { WallType = Border; WallPosition = WallPosition.Top }
        | _ -> { WallType = Normal; WallPosition = WallPosition.Top }

    let wallRight =
        let isLastColumn = columnIndex = (numberOfColumns - 1)
        match isLastColumn with
        | true -> { WallType = Border; WallPosition = WallPosition.Right }
        | false -> { WallType = Normal; WallPosition = WallPosition.Right }

    let WallBottom =
        let isLastRow = rowIndex = (numberOfRows - 1)
        match isLastRow with
        | true -> { WallType = Border; WallPosition = WallPosition.Bottom }
        | false -> { WallType = Normal; WallPosition = WallPosition.Bottom }

    let WallLeft =
        match columnIndex with
        | 0 -> { WallType = Border; WallPosition = WallPosition.Left }
        | _ -> { WallType = Normal; WallPosition = WallPosition.Left }

    {
        CellType = PartOfMaze
        WallTop = wallTop
        WallRight = wallRight
        WallBottom = WallBottom
        WallLeft = WallLeft
    }

/// Simple grid with border walls on the exterior and normal walls internally
/// Every cell is marked as part of the maze
let create numberOfRows numberOfColumns =
    let cells = Array2D.init numberOfRows numberOfColumns (fun rowIndex columnIndex -> getCell numberOfRows numberOfColumns rowIndex columnIndex)

    { Cells = cells; NumberOfRows = numberOfRows; NumberOfColumns = numberOfColumns }