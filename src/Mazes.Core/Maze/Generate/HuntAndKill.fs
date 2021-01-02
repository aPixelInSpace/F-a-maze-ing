// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Maze.Generate.HuntAndKill

open System
open System.Collections.Generic
open System.Linq
open Mazes.Core
open Mazes.Core.Grid
open Mazes.Core.Maze

let createMaze rngSeed (grid : unit -> IGrid<'G>) =

    let grid = grid()

    let rng = Random(rngSeed)

    let randomStartCoordinate = grid.RandomCoordinatePartOfMazeAndNotLinked rng

    let edges = HashSet<Coordinate>()
    edges.Add(randomStartCoordinate) |> ignore

    while edges.Count > 0 do
        let mutable headCoordinate = edges.ElementAt(rng.Next(edges.Count))
        edges.UnionWith(headCoordinate |> grid.NeighborsThatAreLinked false)
        edges.Remove(headCoordinate) |> ignore

        let headLinkedNeighbors =
            headCoordinate
            |> grid.NeighborsThatAreLinked true
            |> Seq.toArray

        if headLinkedNeighbors.Length > 0 then
            let randomLinkedNeighbor = headLinkedNeighbors.[rng.Next(headLinkedNeighbors.Length)]
            grid.LinkCells headCoordinate randomLinkedNeighbor

        let mutable unlinkedNeighbors = grid.NeighborsThatAreLinked false headCoordinate |> Seq.toArray

        while unlinkedNeighbors.Length > 0 do
            let nextCoordinate = unlinkedNeighbors.[rng.Next(unlinkedNeighbors.Length)]

            grid.LinkCells headCoordinate nextCoordinate
            edges.Remove(nextCoordinate) |> ignore

            unlinkedNeighbors <- grid.NeighborsThatAreLinked false nextCoordinate |> Seq.toArray

            edges.UnionWith(unlinkedNeighbors)

            headCoordinate <- nextCoordinate
            

    { Grid = grid }