// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Maze

open Mazes.Core.Analysis
open Mazes.Core.Analysis.Dijkstra.Tracker
open Mazes.Core.Grid
open Mazes.Core.Grid.Array2D.Ortho

type Maze<'G> =
    {    
        Grid : IGrid<'G>
    }

    member this.createMap rootCoordinate =
        Dijkstra.Map.create this.Grid.LinkedNeighbors PriorityQueueTracker.createEmpty rootCoordinate

module Maze =

    let createEmpty (grid : OrthoGrid) =
        grid.Cells
        |> Array2D.iteri(fun r c _ ->
             let update = grid.ToInterface.IfNotAtLimitLinkCells { RIndex = r; CIndex = c }
             let position = (OrthoCoordinateHandler.Instance.NeighborCoordinateAt { RIndex = r; CIndex = c })
             update (position Top)
             update (position Right)
             update (position Bottom)
             update (position Left))
        
        { Grid = grid }

type MazeInfo = {
    Name : string
}