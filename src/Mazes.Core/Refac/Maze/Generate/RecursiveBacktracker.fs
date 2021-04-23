// Copyright 2020-Present Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Refac.Maze.Generate.RecursiveBacktracker

open System
open System.Collections.Generic
open Mazes.Core.Refac.Structure.NDimensionalStructure
open Mazes.Core.Refac.Maze

let createMaze rngSeed ndStruct =

    let rng = Random(rngSeed)

    let randomStartCoordinate = randomCoordinatePartOfMazeAndNotConnected ndStruct rng

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

    GrowingTree.baseAlgorithmNDimensionalStructure ndStruct randomStartCoordinate count add next remove chooseNeighbor

    { NDStruct = ndStruct }