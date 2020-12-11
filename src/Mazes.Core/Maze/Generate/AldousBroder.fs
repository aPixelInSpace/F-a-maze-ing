// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Maze.Generate.AldousBroder

open System
open Mazes.Core.Grid
open Mazes.Core.Maze

let createMaze rngSeed (grid : Grid) =

    let rng = Random(rngSeed)

    let mutable currentCoordinate = grid.RandomCoordinatePartOfMazeAndNotLinked rng

    let unvisitedCount = ref (grid.Canvas.TotalOfMazeZones - 1)

    while unvisitedCount.Value > 0 do
        let nextCoordinate = grid.RandomNeighborFrom rng currentCoordinate

        if not (grid.Cell nextCoordinate).IsLinked then
            grid.LinkCellsAtCoordinates currentCoordinate nextCoordinate
            decr unvisitedCount

        currentCoordinate <- nextCoordinate

    { Grid = grid }