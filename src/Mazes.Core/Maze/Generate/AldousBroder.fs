﻿// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Maze.Generate.AldousBroder

open System
open Mazes.Core

let transformIntoMaze
    randomCoordinatePartOfMazeAndNotConnected
    neighbors
    isCellConnected
    connectCells
    totalOfMazeCells
    (rng : Random) =

    let mutable currentCoordinate = randomCoordinatePartOfMazeAndNotConnected rng

    let unvisitedCount = ref (totalOfMazeCells - 1)

    while unvisitedCount.Value > 0 do
        let nextCoordinate =
            let neighbors = neighbors currentCoordinate |> Seq.toArray
            neighbors.[rng.Next(neighbors.Length)]

        if not (isCellConnected nextCoordinate) then
            connectCells currentCoordinate nextCoordinate
            decr unvisitedCount

        currentCoordinate <- nextCoordinate

let createMaze rngSeed (grid : Grid.NDimensionalStructure<_,_>) : Maze.Maze<_,_> =

    let rng = Random(rngSeed)

    transformIntoMaze
        grid.RandomCoordinatePartOfMazeAndNotConnected
        grid.Neighbors
        grid.IsCellConnected
        (grid.UpdateConnection ConnectionType.Open)
        grid.TotalOfMazeCells
        rng

    { NDimensionalStructure = grid }