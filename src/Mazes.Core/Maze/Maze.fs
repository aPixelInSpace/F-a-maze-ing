// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Maze

open Mazes.Core.Position
open Mazes.Core.Analysis
open Mazes.Core.Grid

type Maze =
    {    
        Grid : Grid
    }

    member this.createDijkstraMap rootCoordinate =
        Dijkstra.Map.create this.Grid.NavigableNeighborsCoordinates this.Grid.Canvas.NumberOfRows this.Grid.Canvas.NumberOfColumns rootCoordinate

module Maze =

    let createEmpty (grid : Grid) =
        grid.Cells
        |> Array2D.iteri(fun r c _ ->
             let update = grid.IfNotAtLimitUpdateWallAtPosition { RowIndex = r; ColumnIndex = c }
             update Top Empty
             update Right Empty
             update Bottom Empty
             update Left Empty)
        
        { Grid = grid }

type MazeInfo = {
    Name : string
}