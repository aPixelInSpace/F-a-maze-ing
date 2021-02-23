// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Maze.Generate.RecursiveBacktracker

open System
open System.Collections.Generic
open Mazes.Core

let createMaze rngSeed (grid : Grid.IGrid<_>) : Maze.Maze<_> =

    let rng = Random(rngSeed)

    let randomStartCoordinate = grid.RandomCoordinatePartOfMazeAndNotConnected rng

    let actives = Stack<Coordinate>()

    let count () = actives.Count

    let add coordinate =
        actives.Push(coordinate)

    let next () =
        actives.Peek()

    let remove _ =
        actives.Pop() |> ignore

    let chooseNeighbor _ (unlinked : array<'T>) =
        unlinked.[rng.Next(unlinked.Length)]

    let grid = grid |> GrowingTree.baseAlgorithmGrid randomStartCoordinate count add next remove chooseNeighbor

    { Grid = grid }