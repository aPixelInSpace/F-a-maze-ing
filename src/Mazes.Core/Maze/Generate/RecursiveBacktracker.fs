// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Maze.Generate.RecursiveBacktracker

open System
open System.Collections.Generic
open Mazes.Core

let createMaze rngSeed (grid : Grid.NDimensionalStructure<_,_>) : Maze.Maze<_,_> =

    let rng = Random(rngSeed)

    let randomStartCoordinate = grid.RandomCoordinatePartOfMazeAndNotConnected rng

    let actives = Stack<_>()

    let count () = actives.Count

    let add coordinate =
        actives.Push(coordinate)

    let next () =
        actives.Peek()

    let remove _ =
        actives.Pop() |> ignore

    let chooseNeighbor _ (unlinked : array<'T>) =
        unlinked.[rng.Next(unlinked.Length)]

    grid |> GrowingTree.baseAlgorithmNDimensionalStructure randomStartCoordinate count add next remove chooseNeighbor

    { NDimensionalStructure = grid }