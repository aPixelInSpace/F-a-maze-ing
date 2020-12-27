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
        Dijkstra.Map.create this.Grid.LinkedNeighbors rootCoordinate

module Maze =

    let createEmpty (grid : OrthoGrid) =
        grid.Cells
        |> Array2D.iteri(fun r c _ ->
             let update = grid.IfNotAtLimitLinkCells { RIndex = r; CIndex = c }
             let position = (OrthoCoordinate.neighborCoordinateAt { RIndex = r; CIndex = c })
             update (position Top)
             update (position Right)
             update (position Bottom)
             update (position Left))
        
        { Grid = grid }

type MazeInfo = {
    Name : string
}