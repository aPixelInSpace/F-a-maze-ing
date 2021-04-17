// Copyright 2020-Present Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Refac.Maze.Generate.AldousBroder

open System
open Mazes.Core.Refac
open Mazes.Core.Refac.Structure.NDimensionalStructure
open Mazes.Core.Refac.Maze

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

let createMaze rngSeed ndStruct =

    let rng = Random(rngSeed)

    transformIntoMaze
        (randomCoordinatePartOfMazeAndNotConnected ndStruct)
        (neighbors ndStruct)
        (isCellConnected ndStruct)
        (updateConnectionState ndStruct ConnectionState.Open)
        (totalOfMazeCells ndStruct)
        rng

    { NDStruct = ndStruct }