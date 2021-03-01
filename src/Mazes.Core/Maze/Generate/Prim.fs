// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Maze.Generate

open System
open System.Collections.Generic
open System.Linq
open Priority_Queue
open Mazes.Core
open Mazes.Core.Structure

module PrimSimple =

    let createMaze rngSeed (ndStruct : NDimensionalStructure<_,_>) : Maze.Maze<_,_> =

        let rng = Random(rngSeed)

        let randomStartCoordinate = ndStruct.RandomCoordinatePartOfMazeAndNotConnected rng

        let actives = HashSet<_>()

        let count () = actives.Count

        let add coordinate =
            actives.Add(coordinate) |> ignore

        let next () =
            actives.ElementAt(rng.Next(actives.Count))

        let remove coordinate =
            actives.Remove(coordinate) |> ignore

        let chooseNeighbor _ (unlinked : array<'T>) =
            unlinked.[rng.Next(unlinked.Length)]

        GrowingTree.baseAlgorithmNDimensionalStructure ndStruct randomStartCoordinate count add next remove chooseNeighbor

        { NDStruct = ndStruct }

module PrimSimpleModified =

    let createMaze rngSeed (ndStruct : NDimensionalStructure<_,_>) : Maze.Maze<_,_> =

        let rng = Random(rngSeed)

        let randomStartCoordinate = ndStruct.RandomCoordinatePartOfMazeAndNotConnected rng

        let inSet = HashSet<_>()
        let frontierSet = HashSet<_>()

        let randomFromInset (coordinates : NCoordinate seq) =
            let found =
                [|
                   for coordinate in coordinates do
                        if inSet.Contains(coordinate) then
                            coordinate
                |]
            found.[rng.Next(found.Length)]

        inSet.Add(randomStartCoordinate) |> ignore
        frontierSet.UnionWith((randomStartCoordinate |> ndStruct.ConnectedNeighbors false))

        while frontierSet.Count > 0 do
            let next = frontierSet.ElementAt(rng.Next(frontierSet.Count))

            frontierSet.Remove(next) |> ignore
            inSet.Add(next) |> ignore

            let unlinkedNeighbors = next |> ndStruct.Neighbors
            let inSetCoordinate = randomFromInset unlinkedNeighbors

            ndStruct.UpdateConnection Open inSetCoordinate next

            frontierSet.UnionWith((next |> ndStruct.ConnectedNeighbors false))

        { NDStruct = ndStruct }

module PrimWeighted =

    let createMaze rngSeed maxWeight (ndStruct : NDimensionalStructure<_,_>) : Maze.Maze<_,_> =

        let rng = Random(rngSeed)

        let randomStartCoordinate = ndStruct.RandomCoordinatePartOfMazeAndNotConnected rng

        let actives = SimplePriorityQueue<_, int>()

        let count () = actives.Count

        let add coordinate =
            actives.Enqueue(coordinate, rng.Next(maxWeight))

        let next () =
            actives.First

        let remove coordinate =
            actives.Remove(coordinate)

        let chooseNeighbor _ (unlinked : array<'T>) =
            unlinked.[rng.Next(unlinked.Length)]

        GrowingTree.baseAlgorithmNDimensionalStructure ndStruct randomStartCoordinate count add next remove chooseNeighbor

        { NDStruct = ndStruct }