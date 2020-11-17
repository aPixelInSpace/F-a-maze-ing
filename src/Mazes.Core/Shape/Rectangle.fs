module Mazes.Core.Shape.Rectangle

open Mazes.Core

/// Rectangle grid with border walls on the exterior and normal walls internally
/// Every cell is marked as part of the maze
let create numberOfRows numberOfColumns =
    let cells = Array2D.init numberOfRows numberOfColumns (fun rowIndex columnIndex -> GridCell.getCellInstance numberOfRows numberOfColumns rowIndex columnIndex (fun _ _ -> true))

    { Cells = cells; NumberOfRows = numberOfRows; NumberOfColumns = numberOfColumns }