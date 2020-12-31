// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Grid.Ortho.GridView

open Mazes.Core
open Mazes.Core.Grid.Teleport
open Mazes.Core.Canvas.Array2D

let sliceGrid (grid : OrthoGrid) (startCoordinate : Coordinate) (endCoordinate : Coordinate) =
    let zones = grid.Canvas.Zones.[startCoordinate.RIndex .. endCoordinate.RIndex, startCoordinate.CIndex .. endCoordinate.CIndex]
    let canvas = { Zones = zones }
    
    let cells = grid.Cells.[startCoordinate.RIndex .. endCoordinate.RIndex, startCoordinate.CIndex .. endCoordinate.CIndex]
    let grid = { Canvas = canvas; Cells = cells; Teleports = Teleports.createEmpty }
    
    grid

let mergeGrid (sourceGrid : OrthoGrid) (targetGrid : OrthoGrid) (targetCoordinate : Coordinate) =
    Array2D.blit sourceGrid.Cells 0 0 targetGrid.Cells targetCoordinate.RIndex targetCoordinate.CIndex (sourceGrid.Cells |> Array2D.length1) (sourceGrid.Cells |> Array2D.length2) 