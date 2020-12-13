// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Grid.Ortho.GridView

open Mazes.Core
open Mazes.Core.Canvas

let sliceGrid (grid : OrthoGrid) (startCoordinate : Coordinate) (endCoordinate : Coordinate) =
    let zones = grid.Canvas.Zones.[startCoordinate.RowIndex .. endCoordinate.RowIndex, startCoordinate.ColumnIndex .. endCoordinate.ColumnIndex]
    let canvas = { Zones = zones }
    
    let cells = grid.Cells.[startCoordinate.RowIndex .. endCoordinate.RowIndex, startCoordinate.ColumnIndex .. endCoordinate.ColumnIndex]
    let grid = { Canvas = canvas; Cells = cells }
    
    grid

let mergeGrid (sourceGrid : OrthoGrid) (targetGrid : OrthoGrid) (targetCoordinate : Coordinate) =
    Array2D.blit sourceGrid.Cells 0 0 targetGrid.Cells targetCoordinate.RowIndex targetCoordinate.ColumnIndex (sourceGrid.Cells |> Array2D.length1) (sourceGrid.Cells |> Array2D.length2) 