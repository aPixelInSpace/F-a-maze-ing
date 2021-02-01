// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Maze.Generate

open System
open System.Collections.Generic
open System.Linq
open Priority_Queue
open Mazes.Core
open Mazes.Core.Grid
open Mazes.Core.Maze

module PrimSimple =

    let createMaze rngSeed (grid : unit -> IGrid<'G>) =

        let grid = grid()

        let rng = Random(rngSeed)

        let randomStartCoordinate = grid.RandomCoordinatePartOfMazeAndNotLinked rng

        let actives = HashSet<Coordinate>()

        let count () = actives.Count

        let add coordinate =
            actives.Add(coordinate) |> ignore

        let next () =
            actives.ElementAt(rng.Next(actives.Count))

        let remove coordinate =
            actives.Remove(coordinate) |> ignore

        let chooseNeighbor _ (unlinked : array<'T>) =
            unlinked.[rng.Next(unlinked.Length)]

        let grid = grid |> GrowingTree.baseAlgorithm randomStartCoordinate count add next remove chooseNeighbor

        { Grid = grid }

module PrimSimpleModified =

    let createMaze rngSeed (grid : unit -> IGrid<'G>) =

        let grid = grid()

        let rng = Random(rngSeed)

        let randomStartCoordinate = grid.RandomCoordinatePartOfMazeAndNotLinked rng

        let inSet = HashSet<Coordinate>()
        let frontierSet = HashSet<Coordinate>()

        let randomFromInset (coordinates : Coordinate seq) =
            let found =
                [|
                   for coordinate in coordinates do
                        if inSet.Contains(coordinate) then
                            coordinate
                |]
            found.[rng.Next(found.Length)]

        inSet.Add(randomStartCoordinate) |> ignore
        frontierSet.UnionWith((randomStartCoordinate |> grid.NeighborsThatAreLinked false))

        while frontierSet.Count > 0 do
            let next = frontierSet.ElementAt(rng.Next(frontierSet.Count))

            frontierSet.Remove(next) |> ignore
            inSet.Add(next) |> ignore

            let unlinkedNeighbors = next |> grid.Neighbors
            let inSetCoordinate = randomFromInset unlinkedNeighbors

            grid.LinkCells inSetCoordinate next

            frontierSet.UnionWith((next |> grid.NeighborsThatAreLinked false))

        { Grid = grid }

module PrimWeighted =

    let createMaze rngSeed maxWeight (grid : unit -> IGrid<'G>) =

        let grid = grid()

        let rng = Random(rngSeed)

        let randomStartCoordinate = grid.RandomCoordinatePartOfMazeAndNotLinked rng

        let actives = SimplePriorityQueue<Coordinate, int>()

        let count () = actives.Count

        let add coordinate =
            actives.Enqueue(coordinate, rng.Next(maxWeight))

        let next () =
            actives.First

        let remove coordinate =
            actives.Remove(coordinate)

        let chooseNeighbor _ (unlinked : array<'T>) =
            unlinked.[rng.Next(unlinked.Length)]

        let grid = grid |> GrowingTree.baseAlgorithm randomStartCoordinate count add next remove chooseNeighbor

        { Grid = grid }