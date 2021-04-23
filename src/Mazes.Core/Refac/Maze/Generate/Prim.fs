// Copyright 2020-Present Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Refac.Maze.Generate

open System
open System.Collections.Generic
open System.Linq
open Priority_Queue
open Mazes.Core.Refac
open Mazes.Core.Refac.Structure.NDimensionalStructure
open Mazes.Core.Refac.Maze

module PrimSimple =

    let createMaze rngSeed ndStruct =

        let rng = Random(rngSeed)

        let randomStartCoordinate = randomCoordinatePartOfMazeAndNotConnected ndStruct rng

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

    let createMaze rngSeed ndStruct =

        let rng = Random(rngSeed)

        let randomStartCoordinate = randomCoordinatePartOfMazeAndNotConnected ndStruct rng

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
        frontierSet.UnionWith(randomStartCoordinate |> connectedNeighbors ndStruct false)

        while frontierSet.Count > 0 do
            let next = frontierSet.ElementAt(rng.Next(frontierSet.Count))

            frontierSet.Remove(next) |> ignore
            inSet.Add(next) |> ignore

            let unlinkedNeighbors = next |> neighbors ndStruct
            let inSetCoordinate = randomFromInset unlinkedNeighbors

            updateConnectionState ndStruct Open inSetCoordinate next

            frontierSet.UnionWith(next |> connectedNeighbors ndStruct false)

        { NDStruct = ndStruct }

module PrimWeighted =

    let createMaze rngSeed maxWeight ndStruct =

        let rng = Random(rngSeed)

        let randomStartCoordinate = randomCoordinatePartOfMazeAndNotConnected ndStruct rng

        let actives = SimplePriorityQueue<_,_>()

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