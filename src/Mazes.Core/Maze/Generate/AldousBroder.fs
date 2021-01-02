// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Maze.Generate.AldousBroder

open System
open Mazes.Core.Grid
open Mazes.Core.Maze

let createMaze rngSeed (grid : unit -> IGrid<'G>) =

    let grid = grid()

    let rng = Random(rngSeed)

    let mutable currentCoordinate = grid.RandomCoordinatePartOfMazeAndNotLinked rng

    let unvisitedCount = ref (grid.TotalOfMazeCells - 1)

    while unvisitedCount.Value > 0 do
        let nextCoordinate = grid.RandomNeighbor rng currentCoordinate

        if not (grid.IsCellLinked nextCoordinate) then
            grid.LinkCells currentCoordinate nextCoordinate
            decr unvisitedCount

        currentCoordinate <- nextCoordinate

    { Grid = grid }