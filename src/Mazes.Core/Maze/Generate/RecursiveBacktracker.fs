// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Maze.Generate.RecursiveBacktracker

open System
open System.Collections.Generic
open Mazes.Core
open Mazes.Core.Grid
open Mazes.Core.Maze

let createMaze rngSeed (grid : unit -> IGrid<'G>) =

    let grid = grid()

    let rng = Random(rngSeed)

    let randomStartCoordinate = grid.RandomCoordinatePartOfMazeAndNotLinked rng

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

    let grid = grid |> GrowingTree.baseAlgorithm randomStartCoordinate count add next remove chooseNeighbor

    { Grid = grid }