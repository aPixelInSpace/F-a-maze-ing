module Mazes.Core.Grid.GridView

open Mazes.Core
open Mazes.Core.Canvas

let sliceGrid (grid : Grid) (startCoordinate : Coordinate) (endCoordinate : Coordinate) =
    let zones = grid.Canvas.Zones.[startCoordinate.RowIndex .. endCoordinate.RowIndex, startCoordinate.ColumnIndex .. endCoordinate.ColumnIndex]
    let canvas = { Zones = zones }
    
    let cells = grid.Cells.[startCoordinate.RowIndex .. endCoordinate.RowIndex, startCoordinate.ColumnIndex .. endCoordinate.ColumnIndex]
    let grid = { Canvas = canvas; Cells = cells }
    
    grid

let mergeGrid (sourceGrid : Grid) (targetGrid : Grid) (targetCoordinate : Coordinate) =
    Array2D.blit sourceGrid.Cells 0 0 targetGrid.Cells targetCoordinate.RowIndex targetCoordinate.ColumnIndex (sourceGrid.Cells |> Array2D.length1) (sourceGrid.Cells |> Array2D.length2) 