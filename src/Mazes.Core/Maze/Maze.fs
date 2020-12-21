// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Maze

open Mazes.Core.Analysis
open Mazes.Core.Grid
open Mazes.Core.Grid.Ortho

type Maze<'G> =
    {    
        Grid : Grid<'G>
    }

    member this.createMap rootCoordinate =
        Dijkstra.Map.create this.Grid.LinkedNeighborsWithCoordinates rootCoordinate

module Maze =

    let createEmpty (grid : OrthoGrid) =
        grid.Cells
        |> Array2D.iteri(fun r c _ ->
             let update = grid.IfNotAtLimitLinkCellAtPosition { RIndex = r; CIndex = c }
             update Top
             update Right
             update Bottom
             update Left)
        
        { Grid = grid }

type MazeInfo = {
    Name : string
}