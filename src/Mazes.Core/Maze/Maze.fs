// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Maze

open Mazes.Core.Position
open Mazes.Core.Analysis
open Mazes.Core.Grid
open Mazes.Core.Grid.Ortho

type Maze =
    {    
        Grid : Grid<OrthoGrid>
    }

    member this.createDijkstraMap rootCoordinate =
        Dijkstra.Map.create this.Grid.LinkedNeighborsWithCoordinates this.Grid.NumberOfRows this.Grid.NumberOfColumns rootCoordinate

module Maze =

    let createEmpty (grid : OrthoGrid) =
        grid.Cells
        |> Array2D.iteri(fun r c _ ->
             let update = grid.IfNotAtLimitLinkCellAtPosition { RowIndex = r; ColumnIndex = c }
             update Top
             update Right
             update Bottom
             update Left)
        
        { Grid = grid.ToGrid }

type MazeInfo = {
    Name : string
}