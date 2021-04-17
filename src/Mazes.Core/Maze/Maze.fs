// Copyright 2020-Present Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Maze

open System
open Mazes.Core
open Mazes.Core.Analysis
open Mazes.Core.Analysis.Dijkstra.Tracker
open Mazes.Core.Structure

type Maze<'Grid, 'Position> =
    {    
        NDStruct : NDimensionalStructure<'Grid, 'Position>
    }

    member this.createMap rootCoordinate =
        Dijkstra.Map.create (this.NDStruct.ConnectedWithNeighbors true) this.NDStruct.CostOfCoordinate PriorityQueueTracker.createEmpty rootCoordinate

    member this.OpenMaze (entrance, exit) =
        this.NDStruct.OpenCell entrance
        this.NDStruct.OpenCell exit

module Maze =

    let toMaze nDStruct =
        { NDStruct = nDStruct }

    let braid (rngSeed : int) (ratio : float) (deadEnds : NCoordinate seq) (maze : Maze<_,_>) =
        let rng = Random(rngSeed)

        let linkToNotAlreadyLinkedNeighbor deadEndCoordinate =
            let notLinkedNeighbors =
                maze.NDStruct.ConnectedWithNeighbors false deadEndCoordinate
                |> Seq.toArray
            if not (notLinkedNeighbors |> Array.isEmpty) then
                maze.NDStruct.UpdateConnection Open deadEndCoordinate notLinkedNeighbors.[rng.Next(notLinkedNeighbors.Length)]

        deadEnds
        |> Seq.iter(fun deadEnd ->
            if rng.NextDouble() <= ratio then linkToNotAlreadyLinkedNeighbor deadEnd)

        maze

type MazeInfo = {
    Name : string
}