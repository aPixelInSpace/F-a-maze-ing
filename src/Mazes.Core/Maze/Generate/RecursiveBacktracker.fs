// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Maze.Generate.RecursiveBacktracker

open System
open System.Collections.Generic
open Mazes.Core
open Mazes.Core.Grid.Ortho
open Mazes.Core.Maze

let createMaze rngSeed (grid : OrthoGrid) =

    let rng = Random(rngSeed)

    let randomStartCoordinate = grid.RandomCoordinatePartOfMazeAndNotLinked rng

    let stack = Stack<Coordinate>()
    stack.Push(randomStartCoordinate)

    while stack.Count > 0 do

        let currentCoordinate = stack.Peek()

        let unlinkedNeighbors = grid.NeighborsThatAreLinked false currentCoordinate |> Seq.toArray

        if unlinkedNeighbors.Length > 0 then
            let nextCoordinate = unlinkedNeighbors.[rng.Next(unlinkedNeighbors.Length)]
            grid.LinkCellsAtCoordinates currentCoordinate nextCoordinate
            stack.Push(nextCoordinate)
        else
            stack.Pop() |> ignore

    { Grid = grid }