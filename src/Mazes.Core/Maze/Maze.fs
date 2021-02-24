// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Maze

open System
open Mazes.Core
open Mazes.Core.Analysis
open Mazes.Core.Analysis.Dijkstra.Tracker

type Maze<'Grid, 'Position> =
    {    
        NDimensionalStructure : Grid.NDimensionalStructure<'Grid, 'Position>
    }

    member this.createMap rootCoordinate =
        Dijkstra.Map.create (this.NDimensionalStructure.ConnectedWithNeighbors true) this.NDimensionalStructure.CostOfCoordinate PriorityQueueTracker.createEmpty rootCoordinate

    member this.OpenMaze (entrance, exit) =
        this.NDimensionalStructure.OpenCell entrance
        this.NDimensionalStructure.OpenCell exit

module Maze =

    let toMaze nDStruct =
        { NDimensionalStructure = nDStruct }

    let braid (rngSeed : int) (ratio : float) (deadEnds : NCoordinate seq) (maze : Maze<_,_>) =
        let rng = Random(rngSeed)

        let linkToNotAlreadyLinkedNeighbor deadEndCoordinate =
            let notLinkedNeighbors =
                maze.NDimensionalStructure.ConnectedWithNeighbors false deadEndCoordinate
                |> Seq.toArray
            if notLinkedNeighbors.Length > 0 then
                maze.NDimensionalStructure.UpdateConnection Open deadEndCoordinate notLinkedNeighbors.[rng.Next(notLinkedNeighbors.Length)]

        deadEnds
        |> Seq.iter(fun deadEnd ->
            if rng.NextDouble() <= ratio then linkToNotAlreadyLinkedNeighbor deadEnd)

        maze

type MazeInfo = {
    Name : string
}