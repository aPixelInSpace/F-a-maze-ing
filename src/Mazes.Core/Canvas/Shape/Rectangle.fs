module Mazes.Core.Canvas.Shape.Rectangle

open Mazes.Core
open Mazes.Core.Canvas

let create numberOfRows numberOfColumns =
    let cellsType = Array2D.create numberOfRows numberOfColumns (CellType.create true)
    
    { CellsType = cellsType; NumberOfRows = numberOfRows; NumberOfColumns = numberOfColumns }