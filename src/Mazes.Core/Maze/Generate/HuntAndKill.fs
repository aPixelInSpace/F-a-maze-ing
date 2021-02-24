// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Maze.Generate.HuntAndKill

open System
open System.Collections.Generic
open System.Linq
open Mazes.Core

let transformIntoMaze
    randomCoordinatePartOfMazeAndNotConnected
    connectedNeighbors
    connectCells
    (rng : Random) =

    let randomStartCoordinate = randomCoordinatePartOfMazeAndNotConnected rng

    let frontier = HashSet<_>()
    frontier.Add(randomStartCoordinate) |> ignore

    while frontier.Count > 0 do
        let mutable headCoordinate = frontier.ElementAt(rng.Next(frontier.Count))
        frontier.UnionWith(headCoordinate |> connectedNeighbors false)
        frontier.Remove(headCoordinate) |> ignore

        let headLinkedNeighbors =
            headCoordinate
            |> connectedNeighbors true
            |> Seq.toArray

        if headLinkedNeighbors.Length > 0 then
            let randomLinkedNeighbor = headLinkedNeighbors.[rng.Next(headLinkedNeighbors.Length)]
            connectCells headCoordinate randomLinkedNeighbor

        let mutable unlinkedNeighbors = connectedNeighbors false headCoordinate |> Seq.toArray

        while unlinkedNeighbors.Length > 0 do
            let nextCoordinate = unlinkedNeighbors.[rng.Next(unlinkedNeighbors.Length)]

            connectCells headCoordinate nextCoordinate
            frontier.Remove(nextCoordinate) |> ignore

            unlinkedNeighbors <- connectedNeighbors false nextCoordinate |> Seq.toArray

            frontier.UnionWith(unlinkedNeighbors)

            headCoordinate <- nextCoordinate

let createMaze rngSeed (grid : Grid.NDimensionalStructure<_,_>) : Maze.Maze<_,_> =

    let rng = Random(rngSeed)

    transformIntoMaze
        grid.RandomCoordinatePartOfMazeAndNotConnected
        grid.ConnectedNeighbors
        (grid.UpdateConnection ConnectionType.Open)
        rng

    { NDimensionalStructure = grid }