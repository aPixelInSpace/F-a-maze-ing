// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Maze.Generate.AldousBroder

open System
open Mazes.Core.Grid
open Mazes.Core.Maze

let transformIntoMaze
    randomCoordinatePartOfMazeAndNotLinked
    neighbors
    isCellConnected
    connectCells
    totalOfMazeCells
    (rng : Random) =

    let mutable currentCoordinate = randomCoordinatePartOfMazeAndNotLinked rng

    let unvisitedCount = ref (totalOfMazeCells - 1)

    while unvisitedCount.Value > 0 do
        let nextCoordinate =
            let neighbors = neighbors currentCoordinate |> Seq.toArray
            neighbors.[rng.Next(neighbors.Length)]

        if not (isCellConnected nextCoordinate) then
            connectCells currentCoordinate nextCoordinate
            decr unvisitedCount

        currentCoordinate <- nextCoordinate

let createMaze rngSeed (grid : unit -> IGrid<'G>) =

    let grid = grid()

    let rng = Random(rngSeed)

    transformIntoMaze
        grid.RandomCoordinatePartOfMazeAndNotLinked
        grid.Neighbors
        grid.IsCellConnected
        grid.ConnectCells
        grid.TotalOfMazeCells
        rng

    { Grid = grid }