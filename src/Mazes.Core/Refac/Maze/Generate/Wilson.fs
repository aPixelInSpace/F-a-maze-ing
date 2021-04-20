// Copyright 2020-Present Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Refac.Maze.Generate.Wilson

open System
open System.Collections.Generic
open System.Linq
open Mazes.Core.Refac
open Mazes.Core.Refac.Structure.NDimensionalStructure
open Mazes.Core.Refac.Maze

let transformIntoMaze
    randomCoordinatePartOfMazeAndNotConnected
    coordinatesPartOfMaze
    neighbors
    connectCells
    (totalOfMazeCells : int)
    (rng : Random) =

    let unvisited = HashSet<_>(totalOfMazeCells)
    unvisited.UnionWith(coordinatesPartOfMaze)

    let firstCoordinate = randomCoordinatePartOfMazeAndNotConnected rng
    unvisited.Remove(firstCoordinate) |> ignore

    while unvisited.Count > 0 do

        let path = ResizeArray<_>()
        let pathTracker = Dictionary<_, int>()

        let mutable nextCoordinate = unvisited.ElementAt(rng.Next(unvisited.Count))
        path.Add(nextCoordinate)
        pathTracker.Add(nextCoordinate, path.Count - 1)

        while unvisited.Contains(nextCoordinate) do
            nextCoordinate <-
                    let neighbors = neighbors nextCoordinate |> Seq.toArray
                    neighbors.[rng.Next(neighbors.Length)]

            if pathTracker.ContainsKey(nextCoordinate) then
                let index = pathTracker.Item(nextCoordinate)
                for i in index + 1 .. path.Count - 1 do
                    pathTracker.Remove(path.[i]) |> ignore
                path.RemoveRange(index + 1, path.Count - index - 1)
            else
                path.Add(nextCoordinate)
                pathTracker.Add(nextCoordinate, path.Count - 1)

        for i in 0 .. path.Count - 2 do
            connectCells path.[i] path.[i + 1]
            unvisited.Remove(path.[i]) |> ignore

let createMaze rngSeed ndStruct =

    let rng = Random(rngSeed)

    transformIntoMaze
        (randomCoordinatePartOfMazeAndNotConnected ndStruct)
        (coordinatesPartOfMaze ndStruct)
        (neighbors ndStruct)
        (updateConnectionState ndStruct ConnectionState.Open)
        (totalOfMazeCells ndStruct)
        rng

    { NDStruct = ndStruct }