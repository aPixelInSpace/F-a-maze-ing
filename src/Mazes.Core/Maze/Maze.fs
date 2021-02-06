// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Maze

open System
open Mazes.Core
open Mazes.Core.Analysis
open Mazes.Core.Analysis.Dijkstra.Tracker
open Mazes.Core.Grid

type Maze<'G> =
    {    
        Grid : IGrid<'G>
    }

    member this.createMap rootCoordinate =
        Dijkstra.Map.create this.Grid.LinkedNeighbors this.Grid.CostOfCoordinate PriorityQueueTracker.createEmpty rootCoordinate

module Maze =

    let toMaze grid =
        { Grid = grid() }

    let braid (rngSeed : int) (ratio : float) (deadEnds : Coordinate seq) (maze : Maze<'G>) =
        let rng = Random(rngSeed)

        let linkToNotAlreadyLinkedNeighbor deadEndCoordinate =
            let notLinkedNeighbors =
                maze.Grid.NotLinkedNeighbors deadEndCoordinate
                |> Seq.toArray
            if notLinkedNeighbors.Length > 0 then
                maze.Grid.UpdateConnection Open deadEndCoordinate notLinkedNeighbors.[rng.Next(notLinkedNeighbors.Length)]

        deadEnds
        |> Seq.iter(fun deadEnd ->
            if rng.NextDouble() <= ratio then linkToNotAlreadyLinkedNeighbor deadEnd)

        maze

type MazeInfo = {
    Name : string
}