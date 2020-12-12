﻿// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Maze.Generate.Wilson

open System
open System.Collections.Generic
open System.Linq
open Mazes.Core
open Mazes.Core.Array2D
open Mazes.Core.Grid
open Mazes.Core.Maze

let createMaze rngSeed grid =

    let rng = Random(rngSeed)

    let unvisited = HashSet<Coordinate>(grid.Canvas.NumberOfRows * grid.Canvas.NumberOfColumns)
    unvisited.UnionWith(
        grid.Canvas.GetZoneByZone RowsAscendingColumnsAscending (fun zone _ -> zone.IsAPartOfMaze)
        |> Seq.map(fun (_, coordinate) -> coordinate))

    let firstCoordinate = grid.RandomCoordinatePartOfMazeAndNotLinked rng
    unvisited.Remove(firstCoordinate) |> ignore

    while unvisited.Count > 0 do

        let path = ResizeArray<Coordinate>()
        let pathTracker = Dictionary<Coordinate, int>()

        let mutable nextCoordinate = unvisited.ElementAt(rng.Next(unvisited.Count))
        path.Add(nextCoordinate) |> ignore
        pathTracker.Add(nextCoordinate, path.Count - 1)

        while unvisited.Contains(nextCoordinate) do

            nextCoordinate <- grid.RandomNeighborFrom rng nextCoordinate

            if pathTracker.ContainsKey(nextCoordinate) then
                let index = pathTracker.Item(nextCoordinate)
                for i in index + 1 .. path.Count - 1 do
                    pathTracker.Remove(path.[i]) |> ignore
                path.RemoveRange(index + 1, path.Count - index - 1)
            else
                path.Add(nextCoordinate)
                pathTracker.Add(nextCoordinate, path.Count - 1)

        for i in 0 .. path.Count - 2 do
            grid.LinkCellsAtCoordinates path.[i] path.[i + 1]
            unvisited.Remove(path.[i]) |> ignore

    { Grid = grid }