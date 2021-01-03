// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Maze

open Mazes.Core
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
             
             let coordinate = { RIndex = r; CIndex = c }
             let update = grid.ToInterface.IfNotAtLimitLinkCells coordinate
             let neighbor = (OrthoCoordinateHandler.Instance.NeighborCoordinateAt coordinate)

             OrthoPositionHandler.Instance.Values coordinate
             |> Array.iter(fun position ->
                                match (neighbor position) with
                                | Some neighbor -> update neighbor
                                | None -> ()))
        
        { Grid = grid }

type MazeInfo = {
    Name : string
}