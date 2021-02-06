// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Maze.Generate.Wilson

open System
open System.Collections.Generic
open System.Linq
open Mazes.Core
open Mazes.Core.Grid
open Mazes.Core.Maze

let createMaze rngSeed (grid : unit -> IGrid<'G>) =

    let grid = grid()

    let rng = Random(rngSeed)

    let unvisited = HashSet<Coordinate>(grid.TotalOfMazeCells)
    unvisited.UnionWith(grid.CoordinatesPartOfMaze)

    let firstCoordinate = grid.RandomCoordinatePartOfMazeAndNotLinked rng
    unvisited.Remove(firstCoordinate) |> ignore

    while unvisited.Count > 0 do

        let path = ResizeArray<Coordinate>()
        let pathTracker = Dictionary<Coordinate, int>()

        let mutable nextCoordinate = unvisited.ElementAt(rng.Next(unvisited.Count))
        path.Add(nextCoordinate) |> ignore
        pathTracker.Add(nextCoordinate, path.Count - 1)

        while unvisited.Contains(nextCoordinate) do
            nextCoordinate <-
                    let neighbors = grid.Neighbors nextCoordinate |> Seq.toArray
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
            grid.ConnectCells path.[i] path.[i + 1]
            unvisited.Remove(path.[i]) |> ignore

    { Grid = grid }